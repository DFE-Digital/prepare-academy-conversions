using Dfe.PrepareConversions.Areas.Transfers.Models.Benefits;
using FluentValidation;
using Dfe.PrepareConversions.Areas.Transfers.Models.Benefits;

namespace Dfe.PrepareConversions.Areas.Transfers.Validators.BenefitsAndRisks
{
    public class EqualitiesImpactValidator : AbstractValidator<EqualitiesImpactAssessmentViewModel>
    {
        public EqualitiesImpactValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.EqualitiesImpactAssessmentConsidered)
                .NotNull()
                .WithMessage("Select yes if an Equalities Impact Assessment has been considered");
        }
    }
}
