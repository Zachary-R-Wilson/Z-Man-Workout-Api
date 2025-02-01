using System.ComponentModel.DataAnnotations;

namespace WorkoutApi.Models
{
    public class MaxModel
    {
        [Required(ErrorMessage = "Squat is Required.")]
        public int Squat {  get; set; }

        [Required(ErrorMessage = "Deadlift is Required.")]
        public int Deadlift { get; set; }

        [Required(ErrorMessage = "Benchpress is Required.")]
        public int Benchpress { get; set; }
    }
}
