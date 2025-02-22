using Microsoft.Data.SqlClient;
using System.Data;
using WorkoutApi.Models;

namespace WorkoutApi.Repositories
{
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly IDbConnection _connection;

        public WorkoutRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        /// <inheritdoc />
        public void CreateWorkout(Guid userKey, WorkoutModel model)
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            using (IDbTransaction transaction = _connection.BeginTransaction())
            {
                try
                {
                    Guid? workoutKey = null;

                    using (IDbCommand command = _connection.CreateCommand())
                    {
                        command.CommandText = "CreateWorkout";
                        command.Transaction = transaction;
                        command.CommandType = CommandType.StoredProcedure;

                        var userKeyParameter = command.CreateParameter();
                        userKeyParameter.ParameterName = "@UserKey";
                        userKeyParameter.DbType = DbType.Guid;
                        userKeyParameter.Value = userKey;
                        command.Parameters.Add(userKeyParameter);

                        var workoutNameParameter = command.CreateParameter();
                        workoutNameParameter.ParameterName = "@WorkoutName";
                        workoutNameParameter.DbType = DbType.String;
                        workoutNameParameter.Size = 256;
                        workoutNameParameter.Value = model.Name;
                        command.Parameters.Add(workoutNameParameter);

                        object result = command.ExecuteScalar();

                        workoutKey = (Guid)result;
                    }

                    model.Days.Keys.ToList().ForEach(dayName =>
                    {
                        Guid dayKey = CreateDay(workoutKey, dayName, transaction);
                        model.Days[dayName].ForEach(exercise =>
                        {
                            CreateExercise(dayKey, exercise, transaction);
                        });
                    });

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        /// <inheritdoc />
        public void DeleteWorkout(Guid userKey, Guid workoutKey)
        {
            using (IDbCommand command = _connection.CreateCommand())
            {
                command.CommandText = "DeleteWorkout";
                command.CommandType = CommandType.StoredProcedure;

                var workoutKeyParameter = command.CreateParameter();
                workoutKeyParameter.ParameterName = "@WorkoutKey";
                workoutKeyParameter.DbType = DbType.Guid;
                workoutKeyParameter.Value = workoutKey;
                command.Parameters.Add(workoutKeyParameter);

                var userKeyParameter = command.CreateParameter();
                userKeyParameter.ParameterName = "@UserKey";
                userKeyParameter.DbType = DbType.Guid;
                userKeyParameter.Value = userKey;
                command.Parameters.Add(userKeyParameter);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        /// <inheritdoc />
        public WorkoutModel GetWorkout(Guid userKey, Guid workoutKey)
        {
            using (IDbCommand command = _connection.CreateCommand())
            {
                command.CommandText = "GetWorkout";
                command.CommandType = CommandType.StoredProcedure;

                var workoutKeyParameter = command.CreateParameter();
                workoutKeyParameter.ParameterName = "@WorkoutKey";
                workoutKeyParameter.DbType = DbType.Guid;
                workoutKeyParameter.Value = workoutKey;
                command.Parameters.Add(workoutKeyParameter);

                var userKeyParameter = command.CreateParameter();
                userKeyParameter.ParameterName = "@UserKey";
                userKeyParameter.DbType = DbType.Guid;
                userKeyParameter.Value = userKey;
                command.Parameters.Add(userKeyParameter);

                _connection.Open();

                WorkoutModel workout = null;
                var daysDictionary = new Dictionary<string, List<Exercise>>();

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (workout == null)
                        {
                            workout = new WorkoutModel
                            {
                                Name = reader.GetString(0),
                                Days = daysDictionary
                            };
                        }

                        string dayName = reader.GetString(1);
                        string exerciseName = reader.GetString(2);
                        string reps = reader.GetString(3);
                        int sets = reader.GetInt32(4);
                        int order = reader.GetInt32(5);

                        if (!daysDictionary.ContainsKey(dayName))
                        {
                            daysDictionary[dayName] = new List<Exercise>();
                        }

                        daysDictionary[dayName].Add(new Exercise
                        {
                            Name = exerciseName,
                            Order = order,
                            Reps = reps,
                            Sets = sets
                        });
                    }
                }

                _connection.Close();

                if (workout == null) throw new Exception("Workout Not Found");

                return workout;
            }
        }

        /// <inheritdoc />
        public  WorkoutCollection GetAllWorkouts(Guid userKey) 
        {
            using (IDbCommand command = _connection.CreateCommand())
            {
                command.CommandText = "GetAllWorkouts";
                command.CommandType = CommandType.StoredProcedure;

                var userKeyParameter = command.CreateParameter();
                userKeyParameter.ParameterName = "@UserKey";
                userKeyParameter.DbType = DbType.Guid;
                userKeyParameter.Value = userKey;
                command.Parameters.Add(userKeyParameter);

                _connection.Open();

                WorkoutCollection workoutCollection = new WorkoutCollection();

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Guid workoutKey = reader.GetGuid(0);
                        string workoutName = reader.GetString(1);
                        Guid dayKey = reader.GetGuid(2);
                        string dayName = reader.GetString(3);

                        if (!workoutCollection.Workouts.ContainsKey(workoutName))
                        {
                            workoutCollection.Workouts[workoutName] = new WorkoutInfo
                            {
                                WorkoutKey = workoutKey,
                                Days = new Dictionary<string, Guid>()
                            };
                        }

                        workoutCollection.Workouts[workoutName].Days[dayName] = dayKey;
                    }
                }

                _connection.Close();

                return workoutCollection;
            }
        }

        /// <summary>
        /// Creates a Day in the database
        /// </summary>
        /// <param name="workoutKey">The guid of the linked workout.</param>
        /// <param name="dayName">The name of the day being created.</param>
        /// <param name="transaction">The connection to the sql database.</param>
        /// <returns>The Guid of the day that was created.</returns>
        private Guid CreateDay(Guid? workoutKey, string dayName, IDbTransaction transaction)
        {
            using (IDbCommand command = _connection.CreateCommand())
            {
                command.CommandText = "CreateDay";
                command.Transaction = transaction;
                command.CommandType = CommandType.StoredProcedure;

                var workoutKeyParameter = command.CreateParameter();
                workoutKeyParameter.ParameterName = "@WorkoutKey";
                workoutKeyParameter.DbType = DbType.Guid;
                workoutKeyParameter.Value = workoutKey;
                command.Parameters.Add(workoutKeyParameter);

                var dayNameParameter = command.CreateParameter();
                dayNameParameter.ParameterName = "@DayName";
                dayNameParameter.DbType = DbType.String;
                dayNameParameter.Size = 256;
                dayNameParameter.Value = dayName;
                command.Parameters.Add(dayNameParameter);

                object result = command.ExecuteScalar();
                return (Guid)result;
            }
        }

        /// <summary>
        /// Creates an exercise in the database.
        /// </summary>
        /// <param name="dayKey">The guid of the linked day.</param>
        /// <param name="exercise">The Exercise data to be added to the database</param>
        /// <param name="order">The order exercises are to be placed.</param
        /// <param name="transaction">The connection to the sql database.</param>

        private void CreateExercise(Guid dayKey, Exercise exercise, IDbTransaction transaction)
        {
            using (IDbCommand command = _connection.CreateCommand())
            {
                command.CommandText = "CreateExercise";
                command.Transaction = transaction;
                command.CommandType = CommandType.StoredProcedure;

                var dayKeyParameter = command.CreateParameter();
                dayKeyParameter.ParameterName = "@DayKey";
                dayKeyParameter.DbType = DbType.Guid;
                dayKeyParameter.Value = dayKey;
                command.Parameters.Add(dayKeyParameter);

                var exerciseNameParameter = command.CreateParameter();
                exerciseNameParameter.ParameterName = "@ExerciseName";
                exerciseNameParameter.DbType = DbType.String;
                exerciseNameParameter.Size = 256;
                exerciseNameParameter.Value = exercise.Name;
                command.Parameters.Add(exerciseNameParameter);

                var exerciseRepsParameter = command.CreateParameter();
                exerciseRepsParameter.ParameterName = "@ExerciseReps";
                exerciseRepsParameter.DbType = DbType.String;
                exerciseRepsParameter.Size = 256;
                exerciseRepsParameter.Value = exercise.Reps;
                command.Parameters.Add(exerciseRepsParameter);

                var exerciseSetsParameter = command.CreateParameter();
                exerciseSetsParameter.ParameterName = "@ExerciseSets";
                exerciseSetsParameter.DbType = DbType.Int32;
                exerciseSetsParameter.Value = exercise.Sets;
                command.Parameters.Add(exerciseSetsParameter);

                var orderParameter = command.CreateParameter();
                orderParameter.ParameterName = "@Order";
                orderParameter.DbType = DbType.Int32;
                orderParameter.Value = exercise.Order;
                command.Parameters.Add(orderParameter);

                command.ExecuteNonQuery();
            }
        }
    }
}
