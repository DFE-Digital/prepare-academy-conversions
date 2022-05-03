using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Models
{
	public class SupportGrantValidator : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{ 
			var reason = value as string;

			if (reason == String.Empty)
			{
				var context = validationContext.ObjectInstance as AcademyConversionProjectPostModel;
				if (context.ConversionSupportGrantAmount < 25000)
				{
					return new ValidationResult("Give a reason why you have changed the support grant amount");
				}
			}
			return ValidationResult.Success;
		}
	}
}
