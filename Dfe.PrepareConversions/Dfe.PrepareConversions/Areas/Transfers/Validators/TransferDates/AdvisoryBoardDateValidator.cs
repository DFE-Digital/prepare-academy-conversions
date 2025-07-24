using Dfe.PrepareTransfers.Helpers;
using Dfe.PrepareTransfers.Web.Models.TransferDates;
using FluentValidation;

namespace Dfe.PrepareTransfers.Web.Transfers.Validators.TransferDates;

public class AdvisoryBoardDateValidator : AbstractValidator<AdvisoryBoardViewModel>
{
   public AdvisoryBoardDateValidator()
   {
      ClassLevelCascadeMode = CascadeMode.Stop;

      RuleFor(x => x.ProposedDecisionDate)
         .SetValidator(new DateValidator { ErrorDisplayName = "proposed decision date" });

      RuleFor(x => x.ProposedDecisionDate.Date.Day)
         .Custom((day, context) =>
         {
            if (!context.RootContextData.TryGetValue("TargetDate", out var targetDate))
            {
                return;
            }

            AdvisoryBoardViewModel dateVm = context.InstanceToValidate;
            if (string.IsNullOrWhiteSpace((string)targetDate))
            {
                return;
            }

            if (!dateVm.ProposedDecisionDate.UnknownDate && DatesHelper.SourceDateStringIsGreaterThanToTargetDateString(
                    dateVm.ProposedDecisionDate.DateInputAsString(),
                    (string)targetDate))
            {
                context.AddFailure(
                    "The proposed decision date must be on or before the target date for the transfer");
            }
         });
   }
}
