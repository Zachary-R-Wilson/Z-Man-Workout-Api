using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;

namespace WorkoutApi.Models
{
    public class WorkoutModel
    {
        [Required(ErrorMessage = "Workout Name is Required.")]
        [RequiredNotEmptyAttribute(ErrorMessage = "Workout Name cannot be empty or whitespace.")]
        public required string Name { get; set; }

        public Dictionary<string, List<Exercise>> Days { get; set; } = new Dictionary<string, List<Exercise>>();
    }

    public class Exercise
    {
        [Required(ErrorMessage = "Exercise Name is Required.")]
        [RequiredNotEmptyAttribute(ErrorMessage = "Exercise Name cannot be empty or whitespace.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Order number is Required.")]
        public required int Order { get; set; }

        [Required(ErrorMessage = "Exercise Reps are Required.")]
        [RequiredNotEmptyAttribute(ErrorMessage = "Reps cannot be empty or whitespace.")]
        public required string Reps { get; set; }
       
        [Required(ErrorMessage = "Exercise Sets are Required.")]
        public required int Sets { get; set; }
    }

    public class WorkoutCollection
    {
        public Dictionary<string, WorkoutInfo> Workouts { get; set; } = new Dictionary<string, WorkoutInfo>();
    }

    public class WorkoutInfo
    {
        public Guid WorkoutKey { get; set; }
        public Dictionary<string, Guid> Days { get; set; } = new Dictionary<string, Guid>();
    }
}
