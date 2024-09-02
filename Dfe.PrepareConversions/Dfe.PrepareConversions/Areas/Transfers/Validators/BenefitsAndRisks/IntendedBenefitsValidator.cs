using Dfe.PrepareConversions.Areas.Transfers.Models.Benefits;
using Dfe.PrepareTransfers.Data.Models.Projects;
using FluentValidation;
using Dfe.PrepareConversions.Areas.Transfers.Models.Benefits;

namespace Dfe.PrepareConversions.Areas.Transfers.Validators.BenefitsAndRisks
{
    public class IntendedBenefitsValidator : AbstractValidator<IntendedBenefitsViewModel>
    {
        public IntendedBenefitsValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.SelectedIntendedBenefits)
                .NotEmpty()
                .WithMessage("Select at least one intended benefit");

            RuleFor(x => x.OtherBenefit)
                .NotEmpty()
                .When(x => x.SelectedIntendedBenefits.Contains(TransferBenefits.IntendedBenefit.Other))
                .WithMessage("Enter the other benefit");
        }
    }
}