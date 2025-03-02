using Microsoft.Data;
using System.Data;
using WorkoutApi.Models;

namespace WorkoutApi.Repositories
{
    public class AuthRepository: IAuthRepository
    {
        private readonly IDbConnection _connection;

        public AuthRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        /// <inheritdoc />
        public Guid? AuthenticateUser(LoginModel loginModel)
        {
            Guid? userKey = null;

            using (IDbCommand command = _connection.CreateCommand())
            {
                command.CommandText = "AuthUser";
                command.CommandType = CommandType.StoredProcedure;

                var emailParameter = command.CreateParameter();
                emailParameter.ParameterName = "@Email";
                emailParameter.DbType = DbType.String;
                emailParameter.Value = loginModel.Email;
                command.Parameters.Add(emailParameter);

                _connection.Open();
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userKey = reader.GetGuid(reader.GetOrdinal("UserKey"));
                        string passwordHash = reader.GetString(reader.GetOrdinal("PasswordHash"));

                        if (!BCrypt.Net.BCrypt.Verify(loginModel.Password, passwordHash)) return null;
                    }
                }
                _connection.Close();
            }

            return userKey;
        }

        /// <inheritdoc />
        public Guid? RegisterUser(LoginModel loginModel)
        {
            Guid? userKey = null;

            using (IDbCommand command = _connection.CreateCommand())
            {
                command.CommandText = "RegisterUser";
                command.CommandType = CommandType.StoredProcedure;

                var emailParameter = command.CreateParameter();
                emailParameter.ParameterName = "@Email";
                emailParameter.DbType = DbType.String;
                emailParameter.Value = loginModel.Email;
                command.Parameters.Add(emailParameter);

                var passwordHashParameter = command.CreateParameter();
                passwordHashParameter.ParameterName = "@PasswordHash";
                passwordHashParameter.DbType = DbType.String;
                passwordHashParameter.Value = loginModel.Password;
                command.Parameters.Add(passwordHashParameter);

                _connection.Open();
                object result = command.ExecuteScalar();
                _connection.Close();

                if (result != null && result != DBNull.Value)
                {
                    userKey = (Guid)result;
                }
            }

            return userKey;
        }
    }
}
