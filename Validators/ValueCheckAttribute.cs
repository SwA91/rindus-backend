using System.ComponentModel.DataAnnotations;

namespace RindusBackend.Validators;

public class ValueCheckAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        switch ((string?)value)
        {
            case "":
            case null:
                return new ValidationResult($"The field {validationContext.MemberName} is required");
            default:
                return ValidationResult.Success;
        }
    }
}