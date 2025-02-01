using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using WorkoutApi.Models;

namespace WorkoutApi.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates the User
        /// </summary>
        /// <param name="credentials">LoginModel with credentials.</param>
        /// <returns>JWT if the user can be authenticated otherwise is null.</returns>
        string? AuthenticateUser(LoginModel credentials);

        /// <summary>
        /// Registers the User
        /// </summary>
        /// <param name="credentials">LoginModel with credentials.</param>
        /// <returns>JWT or null if the user can be registered otherwise is null.</returns>
        string? RegisterUser(LoginModel credentials);
    }
}
