using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VideoRental2.Models
{
    public class Min18YearsIfAMember: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customer = (Customer)validationContext.ObjectInstance;
            if (customer.MembershipTypeId == 1)
                return ValidationResult.Success;
            if (customer.birthday == null)
                return new ValidationResult("Birthday is required");
            var age = DateTime.Today.Year - customer.birthday.Value.Year;
            return (age >= 18) ? ValidationResult.Success : new ValidationResult("Need to be at least 18 years old to go on a membership");
        }
    }
}