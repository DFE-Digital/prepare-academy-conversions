using Dfe.PrepareTransfers.Web.Models.Rationale;
using FluentValidation;
using Dfe.PrepareTransfers.Web.Transfers.Validators.Rationale;

namespace Dfe.PrepareTransfers.Web.Transfers.Validators.Rationale
{
    public class RationaleTrustOrSponsorValidator : AbstractValidator<RationaleTrustOrSponsorViewModel>
    {
        public RationaleTrustOrSponsorValidator()
        {
            RuleFor(x => x.TrustOrSponsorRationale)
                .NotEmpty()
                .WithMessage("Enter the rationale for the incoming trust or sponsor");
        }
    }
}