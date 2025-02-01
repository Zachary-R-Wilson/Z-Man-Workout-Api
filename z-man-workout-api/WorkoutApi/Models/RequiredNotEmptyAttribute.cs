using System.ComponentModel.DataAnnotations;

namespace WorkoutApi.Models
{
    public class RequiredNotEmptyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string str && string.IsNullOrWhiteSpace(str))
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} cannot be empty or whitespace.");
            }
            return ValidationResult.Success;
        }
    }
}
