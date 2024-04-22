using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Helpers.ValidationAttributes
{
    public class LessThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public LessThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }

            var currentValue = Convert.ToDecimal(value);

            var otherProperty = validationContext.ObjectInstance.GetType()
                .GetProperty(_comparisonProperty);

            if (otherProperty is null)
            {
                return null;
            }

            var comparisonValue = Convert.ToDecimal(otherProperty?.GetValue(validationContext.ObjectInstance));

            var result = decimal.Compare(currentValue, comparisonValue);
            if (result >= 0)
            {
                var currentPropertyDisplayName = GetDisplayName(validationContext.ObjectType, 
                    validationContext.MemberName!);
                var comparisonPropertyDisplayName = GetDisplayName(validationContext.ObjectType, 
                    _comparisonProperty);

                return new ValidationResult(string.Format(
                    ErrorMessage ?? $"The {0} must be less than {1}.", 
                    currentPropertyDisplayName, comparisonPropertyDisplayName));
            }

            return ValidationResult.Success;
        }

        private string GetDisplayName(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName);
            var displayAttribute = property?.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.GetName() ?? propertyName;
        }
    }
}
