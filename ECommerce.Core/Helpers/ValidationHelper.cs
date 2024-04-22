using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Helpers
{
    internal class ValidationHelper
    {
        internal static void ValidateModel(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();
            var isValid =
               Validator.TryValidateObject(obj, validationContext, validationResults, true);
            if (!isValid)
            {
                var errorMessages = validationResults.Select(r => r.ErrorMessage);
                throw new ValidationException($"Validation failed: {string.Join(", ", 
                    errorMessages)}");
            }
        }
    }
}
