using WorkoutApi.Models;

namespace WorkoutApi.Repositories
{
    public interface IMaxesRepository
    {
        /// <summary>
        /// Gets the users maxes from the database.
        /// </summary>
        /// <param name="userKey">The user's maxes.</param>
        /// <returns>A Maxes model with the users max information.</returns>
        MaxModel GetMaxes(Guid userKey);

        /// <summary>
        /// Creates or updates Max information in the database.
        /// </summary>
        /// <param name="trackingModel">The data that is being inserted into the database.</param>
        void UpdateMaxes(Guid userKey, MaxModel trackingModel);
    }
}
