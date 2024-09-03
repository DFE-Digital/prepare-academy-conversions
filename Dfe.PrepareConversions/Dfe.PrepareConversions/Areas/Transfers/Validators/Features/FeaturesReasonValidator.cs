using Dfe.PrepareTransfers.Web.Transfers.Pages.Projects.Features;
using Dfe.PrepareTransfers.Data.Models.Projects;
using FluentValidation;

namespace Dfe.PrepareTransfers.Web.Transfers.Validators.Features
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