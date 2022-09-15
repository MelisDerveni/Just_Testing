using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618


public class MoreThan0 : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if ((int)value < 1 )
        {
            return new ValidationResult("Calories must be greater than 0");
        }
        else{
        return ValidationResult.Success;
        }
    }
}

