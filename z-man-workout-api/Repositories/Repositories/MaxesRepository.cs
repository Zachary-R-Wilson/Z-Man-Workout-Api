using Microsoft.Data.SqlClient;
using System.Data;
using WorkoutApi.Models;

namespace WorkoutApi.Repositories
{
    public class MaxesRepository : IMaxesRepository
    {
        private readonly IDbConnection _connection;

        public MaxesRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        /// <inheritdoc />
        public MaxModel GetMaxes(Guid userKey)
        {
            using (IDbCommand command = _connection.CreateCommand())
            {
                command.CommandText = "GetMaxes";
                command.CommandType = CommandType.StoredProcedure;

                var userKeyParameter = command.CreateParameter();
                userKeyParameter.ParameterName = "@UserKey";
                userKeyParameter.DbType = DbType.Guid;
                userKeyParameter.Value = userKey;
                command.Parameters.Add(userKeyParameter);

                _connection.Open();

                MaxModel? maxModel = null;

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        int squat = reader.GetInt32(0);
                        int deadlift = reader.GetInt32(1);
                        int benchpress = reader.GetInt32(2);

                        maxModel = new MaxModel
                        { 
                            Squat = squat,
                            Deadlift = deadlift,
                            Benchpress = benchpress
                        };
                    }
                }

                _connection.Close();

                if (maxModel == null) throw new Exception("No Maxes Found");

                return maxModel;
            }
        }

        /// <inheritdoc />
        public void UpdateMaxes(Guid userKey, MaxModel maxModel)
        {
            using (IDbCommand command = _connection.CreateCommand())
            {
                command.CommandText = "UpdateMaxes";
                command.CommandType = CommandType.StoredProcedure;

                var userKeyParameter = command.CreateParameter();
                userKeyParameter.ParameterName = "@UserKey";
                userKeyParameter.DbType = DbType.Guid;
                userKeyParameter.Value = userKey;
                command.Parameters.Add(userKeyParameter);

                var squatParameter = command.CreateParameter();
                squatParameter.ParameterName = "@Squat";
                squatParameter.DbType = DbType.Int32;
                squatParameter.Value = maxModel.Squat;
                command.Parameters.Add(squatParameter);

                var DeadliftParameter = command.CreateParameter();
                DeadliftParameter.ParameterName = "@Deadlift";
                DeadliftParameter.DbType = DbType.Int32;
                DeadliftParameter.Value = maxModel.Deadlift;
                command.Parameters.Add(DeadliftParameter);

                var benchpressParameter = command.CreateParameter();
                benchpressParameter.ParameterName = "@Benchpress";
                benchpressParameter.DbType = DbType.Int32;
                benchpressParameter.Value = maxModel.Benchpress;
                command.Parameters.Add(benchpressParameter);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }
    }
}
