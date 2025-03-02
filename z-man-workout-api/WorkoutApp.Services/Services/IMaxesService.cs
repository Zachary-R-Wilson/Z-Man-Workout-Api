using WorkoutApi.Models;

namespace WorkoutApi.Services
{
    public interface IMaxesService
    {
        /// <summary>
        /// Gets the users maxes from the database.
        /// </summary>
        /// <param name="token">The bearer token.</param>
        /// <returns>A Maxes model with the users max information.</returns>
        MaxModel GetMaxes(string token);

        /// <summary>
        /// Creates or updates Max information in the database.
        /// </summary>
        /// <param name="token">The bearer token.</param>
        /// <param name="trackingModel">The data that is being inserted into the database.</param>
        void UpdateMaxes(string token, MaxModel trackingModel);
    }
}
