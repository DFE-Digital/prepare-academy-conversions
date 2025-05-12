using FluentValidation;
using Dfe.PrepareTransfers.Web.Pages.Transfers;

namespace Dfe.PrepareTransfers.Web.Validators.Transfers
{
    public class OutgoingTrustNameForSearchValidator : AbstractValidator<TrustSearchModel>
    {
        public OutgoingTrustNameForSearchValidator()
        {
            RuleFor(request => request.SearchQuery)
                .NotEmpty()
                .WithMessage("Enter the outgoing trust name");
        }
    }
}