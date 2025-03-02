using WorkoutApi.Models;

namespace WorkoutApi.Repositories
{
    public interface IWorkoutRepository
    {
        /// <summary>
        /// Writes the workout to the SQL database.
        /// </summary>
        /// <param name="userKey">The specified users guid</param>
        /// <param name="workoutModel">The data stored in the workout.</param>
        void CreateWorkout(Guid userKey, WorkoutModel workoutModel);

        /// <summary>
        /// Deletes a workout from the database by the workoutKey
        /// </summary>
        /// <param name="userKey">The specified users guid</param>
        /// <param name="workoutKey">The Guid of the workout</param>
        void DeleteWorkout(Guid userKey, Guid workoutKey);

        /// <summary>
        /// Retrieves a workout model for a given workout guid.
        /// </summary>
        /// <param name="userKey">The specified users guid</param>
        /// <param name="WorkoutKey">The specified guid of the workout</param>
        /// <returns>WorkoutModel with the workout data.</returns>
        WorkoutModel GetWorkout(Guid userKey, Guid WorkoutKey);

        /// <summary>
        /// Retrieves all relative information for a users workout home screen.
        /// </summary>
        /// <param name="userKey">The specified users guid</param>
        /// <returns>Basic Workout Information for the homescreen.</returns>
        WorkoutCollection GetAllWorkouts(Guid userKey);
    }
}
