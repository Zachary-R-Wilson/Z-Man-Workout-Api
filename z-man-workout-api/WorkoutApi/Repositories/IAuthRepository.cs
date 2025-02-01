using WorkoutApi.Models;

namespace WorkoutApi.Repositories
{
    public interface IAuthRepository
    {
        /// <summary>
        /// Authenticates the user in the database.
        /// </summary>
        /// <param name="loginModel">the LoginModel with the user's credentials.</param>
        /// <returns>JWT if the user can be authenticated otherwise is null.</returns>
        Guid? AuthenticateUser(LoginModel loginModel);

        /// <summary>
        /// Registers the user in the database if the email does not already exist.
        /// </summary>
        /// <param name="loginModel">the LoginModel with the user's credentials.</param>
        /// <returns>JWT if the user can be authenticated otherwise is null.</returns>
        Guid? RegisterUser(LoginModel loginModel);
    }
}
