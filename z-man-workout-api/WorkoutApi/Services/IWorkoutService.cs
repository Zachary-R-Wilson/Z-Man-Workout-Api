using WorkoutApi.Models;

namespace WorkoutApi.Services
{
    public interface IWorkoutService
    {
        /// <summary>
        /// Creates the workout.
        /// </summary>
        /// <param name="token">The bearer token.</param>
        /// <param name="workoutModel">The data stored in the workout.</param>
        void CreateWorkout(string token, WorkoutModel workoutModel);

        /// <summary>
        /// Deletes a workout.
        /// </summary>
        /// <param name="token">The bearer token.</param>
        /// <param name="workoutKey">The guid of the workout being deleted.</param>
        void DeleteWorkout(string token, Guid workoutKey);

        /// <summary>
        /// Retrieves a workout model for a given workout guid.
        /// </summary>
        /// <param name="token">The bearer token.</param>
        /// <param name="WorkoutKey">The specified guid of the workout</param>
        /// <returns>WorkoutModel with the workout data.</returns>
        WorkoutModel GetWorkout(string token, Guid WorkoutKey);

        /// <summary>
        /// Retrieves all relative information for a users workout home screen.
        /// </summary>
        /// <param name="token">The bearer token.</param>
        /// <returns>Basic Workout Information for the homescreen.</returns>
        WorkoutCollection GetAllWorkouts(string token);
    }
}
