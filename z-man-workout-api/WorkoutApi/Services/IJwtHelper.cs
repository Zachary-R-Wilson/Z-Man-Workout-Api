namespace WorkoutApi.Services
{
    public interface IJwtHelper
    {
        /// <summary>
        /// Generates a JwtToken so the valid user can access the api
        /// </summary>
        /// <param name="userKey">The Guid that designates which user this is.</param>
        /// <returns>JwtToken</returns>
        Guid? ExtractUserKey(string token);

        /// <summary>
        /// Retrives the UserKey from the valid JwtToken.
        /// </summary>
        /// <param name="token">The JwtToken given to the client on login.</param>
        /// <returns>Extracted UserKey</returns>
        string GenerateAccessToken(Guid? userKey);
    }
}
