using System.Linq;
using FluentValidation;
using Dfe.PrepareConversions.Areas.Transfers.Models.Benefits;

namespace Dfe.PrepareConversions.Areas.Transfers.Validators.BenefitsAndRisks
{
    public class OtherFactorsValidator : AbstractValidator<OtherFactorsViewModel>
    {
        public OtherFactorsValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.OtherFactorsVm)
                .Custom((list, context) =>
                {
                    if (!list.Any(o => o.Checked))
                    {
                        context.AddFailure("Select the risks with this transfer");
                    }
                });
        }
    }
}