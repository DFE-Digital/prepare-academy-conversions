using FluentValidation;
using Dfe.PrepareConversions.Areas.Transfers.Models.Benefits;

namespace Dfe.PrepareConversions.Areas.Transfers.Validators.BenefitsAndRisks
{
    public class RisksValidator : AbstractValidator<RisksViewModel>
    {
        public RisksValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.RisksInvolved)
                .NotNull()
                .WithMessage("Select yes if there are risks to consider");
        }
    }
}