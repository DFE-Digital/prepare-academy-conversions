using System.ComponentModel.DataAnnotations;
using Dfe.PrepareConversions.Pages.TaskList.SchoolAndTrustInformation;

namespace Dfe.PrepareConversions.Models
{
	public class SupportGrantValidatorAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{ 
			var reason = value as string;

			if (string.IsNullOrWhiteSpace(reason))
			{
				var context = validationContext.ObjectInstance as RouteAndGrant.InputModel;
				if (context.ConversionSupportGrantAmount < 25000)
				{
					return new ValidationResult("Give a reason why you have changed the support grant amount");
				}
			}
			return ValidationResult.Success;
		}
	}
}
