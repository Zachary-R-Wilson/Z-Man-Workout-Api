using Microsoft.AspNetCore.Identity;
using System.Reflection.PortableExecutable;
using WorkoutApi.Models;
using WorkoutApi.Repositories;

namespace WorkoutApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtHelper _jwtHelper;

        public AuthService(IAuthRepository authRepository, IJwtHelper jwtHelper)
        {
            _authRepository = authRepository;
            _jwtHelper = jwtHelper;
        }

        /// <inheritdoc />
        public string? AuthenticateUser(LoginModel credentials)
        {
            Guid? userKey = _authRepository.AuthenticateUser(credentials);
            if(userKey != null)
            {
                return _jwtHelper.GenerateAccessToken(userKey);
            }

            return null;
        }

        /// <inheritdoc />
        public string? RegisterUser(LoginModel credentials)
        {
            credentials.Password = BCrypt.Net.BCrypt.HashPassword(credentials.Password);
            Guid? userKey = _authRepository.RegisterUser(credentials);
            if (userKey != null)
            {
                return _jwtHelper.GenerateAccessToken(userKey);
            }

            return null;
        }
    }
}
