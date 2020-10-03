using System;
using System.ComponentModel.DataAnnotations;

namespace BookStore.CustomValidation
{
    public class GreaterThanDateAttributte : ValidationAttribute
    {
        /// <summary>
        /// checks if date is from today forward
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            DateTime date = (DateTime)value;
            return date > DateTime.UtcNow;
        }
    }
}
