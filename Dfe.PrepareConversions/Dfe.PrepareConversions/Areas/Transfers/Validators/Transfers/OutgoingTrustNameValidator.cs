using FluentValidation;
using Dfe.PrepareTransfers.Web.Pages.NewTransfer;

namespace Dfe.PrepareTransfers.Web.Validators.Transfers
{
    public class OutgoingTrustNameValidator : AbstractValidator<TrustNameModel>
    {
        public OutgoingTrustNameValidator()
        {
            RuleFor(request => request.SearchQuery)
                .NotEmpty()
                .WithMessage("Enter the outgoing trust name");
        }
    }
}