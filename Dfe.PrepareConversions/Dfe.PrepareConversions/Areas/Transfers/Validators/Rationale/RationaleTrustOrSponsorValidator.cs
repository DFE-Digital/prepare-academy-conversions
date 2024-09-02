using Dfe.PrepareConversions.Areas.Transfers.Models.Rationale;
using FluentValidation;
using Dfe.PrepareConversions.Areas.Transfers.Validators.Rationale;

namespace Dfe.PrepareConversions.Areas.Transfers.Validators.Rationale
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