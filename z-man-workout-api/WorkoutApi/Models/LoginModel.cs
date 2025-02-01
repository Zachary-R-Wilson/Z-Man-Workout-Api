using System.ComponentModel.DataAnnotations;

namespace WorkoutApi.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is Required.")]
        [StringLength(100, ErrorMessage = "Email must be between {2} and {1} characters.", MinimumLength = 5)]
        [RequiredNotEmptyAttribute(ErrorMessage = "Email cannot be empty or whitespace.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is Required.")]
        [DataType(DataType.Password)]
        [RequiredNotEmptyAttribute(ErrorMessage = "Password cannot be empty or whitespace.")]
        public required string Password { get; set; }
    }
}
