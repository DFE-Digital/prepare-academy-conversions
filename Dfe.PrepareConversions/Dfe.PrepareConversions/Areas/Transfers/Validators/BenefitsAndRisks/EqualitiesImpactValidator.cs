using Dfe.PrepareTransfers.Web.Models.Benefits;
using FluentValidation;
using Dfe.PrepareTransfers.Web.Models.Benefits;

namespace Dfe.PrepareTransfers.Web.Transfers.Validators.BenefitsAndRisks
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
