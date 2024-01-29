using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Errors
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CustomNotNullOrZeroAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null || value is 0)
            {
                return new ValidationResult("The deliveryMethodId field is required..");
            }

            return ValidationResult.Success;
        }
    }

}
