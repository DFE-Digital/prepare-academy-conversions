using Dfe.PrepareTransfers.Helpers;
using Dfe.PrepareTransfers.Web.Models.TransferDates;
using Dfe.PrepareTransfers.Web.Transfers.Validators.TransferDates;
using FluentValidation;

namespace Dfe.PrepareConversions.Areas.Transfers.Validators.TransferDates
{
   public class PreviouslyConsideredDateValidator : AbstractValidator<PreviouslyConsideredViewModel>
   {
      public PreviouslyConsideredDateValidator()
      {
         ClassLevelCascadeMode = CascadeMode.Stop;

         RuleFor(x => x.PreviouslyConsideredDate)
            .SetValidator(new DateValidator { ErrorDisplayName = "previously considered date" });

         RuleFor(x => x.PreviouslyConsideredDate.Date.Day)
            .Custom((day, context) =>
            {
               if (!context.RootContextData.TryGetValue("TargetDate", out var targetDate))
               {
                  return;
               }

               PreviouslyConsideredViewModel dateVm = context.InstanceToValidate;
               if (string.IsNullOrWhiteSpace((string)targetDate))
               {
                  return;
               }

               if (!dateVm.PreviouslyConsideredDate.UnknownDate && DatesHelper.SourceDateStringIsGreaterThanToTargetDateString(
                       dateVm.PreviouslyConsideredDate.DateInputAsString(),
                       (string)targetDate))
               {
                  context.AddFailure(
                   "The previously considered date must be on or before the target date for the transfer");
               }
            });
      }
   }
}
