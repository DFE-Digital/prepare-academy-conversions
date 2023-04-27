using Dfe.PrepareConversions.Pages.TaskList.SchoolAndTrustInformation;
using System.ComponentModel.DataAnnotations;

namespace Dfe.PrepareConversions.Validators;

public class SupportGrantValidatorAttribute : ValidationAttribute
{
   protected override ValidationResult IsValid(object value, ValidationContext validationContext)
   {
      string reason = value as string;

      if (string.IsNullOrWhiteSpace(reason))
      {
         RouteAndGrant.InputModel context = validationContext.ObjectInstance as RouteAndGrant.InputModel;
         if (context!.ConversionSupportGrantAmount < 25000)
         {
            return new ValidationResult("Give a reason why you have changed the support grant amount");
         }
      }

      return ValidationResult.Success;
   }
}
