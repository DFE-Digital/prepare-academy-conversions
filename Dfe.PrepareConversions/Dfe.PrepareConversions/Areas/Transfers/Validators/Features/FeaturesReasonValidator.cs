using Dfe.PrepareConversions.Areas.Transfers.Pages.Projects.Features;
using Dfe.PrepareTransfers.Data.Models.Projects;
using FluentValidation;

namespace Dfe.PrepareConversions.Areas.Transfers.Validators.Features
{
    public class FeaturesReasonValidator : AbstractValidator<Reason>
    {
        public FeaturesReasonValidator()
        {
            RuleFor(x => x.ReasonForTheTransfer)
                .NotNull()
                .NotEqual(TransferFeatures.ReasonForTheTransferTypes.Empty)
                .WithMessage("Select a reason for the transfer");
        }
    }
}