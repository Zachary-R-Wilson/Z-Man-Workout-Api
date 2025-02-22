using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using WorkoutApi.Models;

namespace WorkoutApi.Repositories
{
    public class TrackingRepository: ITrackingRepository
    {
        private readonly IDbConnection _connection;

        public TrackingRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        /// <inheritdoc />
        public TrackingProgressModel GetProgress(Guid userKey, Guid dayKey)
        {
            using (IDbCommand command = _connection.CreateCommand())
            {
                command.CommandText = "GetProgress";
                command.CommandType = CommandType.StoredProcedure;

                var dayKeyParameter = command.CreateParameter();
                dayKeyParameter.ParameterName = "@DayKey";
                dayKeyParameter.DbType = DbType.Guid;
                dayKeyParameter.Value = dayKey;
                command.Parameters.Add(dayKeyParameter);

                var userKeyParameter = command.CreateParameter();
                userKeyParameter.ParameterName = "@UserKey";
                userKeyParameter.DbType = DbType.Guid;
                userKeyParameter.Value = userKey;
                command.Parameters.Add(userKeyParameter);

                _connection.Open();

                TrackingProgressModel trackingProgressModel = new TrackingProgressModel();

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string dayName = reader.GetString(reader.GetOrdinal("DayName"));
                        Guid exerciseKey = reader.GetGuid(reader.GetOrdinal("ExerciseKey"));
                        string exerciseName = reader.GetString(reader.GetOrdinal("ExerciseName"));
                        string reps = reader.GetString(reader.GetOrdinal("Reps"));
                        int sets = reader.GetInt32(reader.GetOrdinal("Sets"));
                        string? weight = reader.IsDBNull(reader.GetOrdinal("Weight")) ? null : reader.GetString(reader.GetOrdinal("Weight"));
                        int? completedReps = reader.IsDBNull(reader.GetOrdinal("CompletedReps")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CompletedReps"));
                        int? rpe = reader.IsDBNull(reader.GetOrdinal("RPE")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("RPE"));
                        DateTime? date = reader.IsDBNull(reader.GetOrdinal("LastWorkout")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LastWorkout"));

                        trackingProgressModel.Exercises[exerciseName] = new TrackingProgress
                        {
                            DayKey = dayKey,
                            DayName = dayName,
                            ExerciseKey = exerciseKey,
                            ExerciseName = exerciseName,
                            Reps = reps,
                            Sets = sets,
                            Weight = weight,
                            CompletedReps = completedReps,
                            RPE = rpe,
                            Date = date
                        };
                    }
                }

                _connection.Close();

                return trackingProgressModel;
            }
        }

        /// <inheritdoc />
        public void InsertTracking(Guid userKey, TrackingModel trackingModel)
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            using (IDbTransaction transaction = _connection.BeginTransaction())
            {
                try
                {
                    trackingModel.Exercises.Keys.ToList().ForEach(exerciseName =>
                    {
                        InsertInfo(trackingModel.Exercises[exerciseName], userKey, transaction);
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

        /// <summary>
        /// Inserts Tracking info into the database
        /// </summary>
        /// <param name="info">The tracking data to be stored.</param>
        /// <param name="transaction">The connection to the sql database.</param>
        private void InsertInfo(TrackingInfo info, Guid userKey, IDbTransaction transaction)
        {
            if (info.Weight != null || info.CompletedReps != null)
            {
                using (IDbCommand command = _connection.CreateCommand())
                {
                    command.CommandText = "InsertTracking";
                    command.Transaction = transaction;
                    command.CommandType = CommandType.StoredProcedure;

                    var exerciseKeyParameter = command.CreateParameter();
                    exerciseKeyParameter.ParameterName = "@ExerciseKey";
                    exerciseKeyParameter.DbType = DbType.Guid;
                    exerciseKeyParameter.Value = info.ExerciseKey;
                    command.Parameters.Add(exerciseKeyParameter);

                    var userKeyParameter = command.CreateParameter();
                    userKeyParameter.ParameterName = "@UserKey";
                    userKeyParameter.DbType = DbType.Guid;
                    userKeyParameter.Value = userKey;
                    command.Parameters.Add(userKeyParameter);

                    var dateParameter = command.CreateParameter();
                    dateParameter.ParameterName = "@Date";
                    dateParameter.DbType = DbType.Date;
                    dateParameter.Value = info.Date;
                    command.Parameters.Add(dateParameter);

                    var weightParameter = command.CreateParameter();
                    weightParameter.ParameterName = "@Weight";
                    weightParameter.DbType = DbType.String;
                    weightParameter.Size = 256;
                    weightParameter.Value = info.Weight;
                    command.Parameters.Add(weightParameter);

                    var completedRepsParameter = command.CreateParameter();
                    completedRepsParameter.ParameterName = "@CompletedReps";
                    completedRepsParameter.DbType = DbType.Int32;
                    completedRepsParameter.Value = info.CompletedReps;
                    command.Parameters.Add(completedRepsParameter);

                    var rpeParameter = command.CreateParameter();
                    rpeParameter.ParameterName = "@RPE";
                    rpeParameter.DbType = DbType.Int32;
                    rpeParameter.Value = info.RPE;
                    command.Parameters.Add(rpeParameter);

                    object result = command.ExecuteScalar();
                }
            }
        }
    }
}
