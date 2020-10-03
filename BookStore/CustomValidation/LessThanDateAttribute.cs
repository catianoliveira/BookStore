using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.CustomValidation
{
    public class LessThanDateAttribute : ValidationAttribute
    {
        /// <summary>
        /// checks if date is lesser than todays
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            DateTime date = (DateTime)value;
            return date < DateTime.UtcNow;
        }
    }
}
