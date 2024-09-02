using FluentValidation;
using Dfe.PrepareConversions.Areas.Transfers.Models.Rationale;
using Dfe.PrepareTransfers.Web.Models.Rationale;

namespace Dfe.PrepareConversions.Areas.Transfers.Validators.Rationale
{
    public class RationaleProjectValidator : AbstractValidator<RationaleProjectViewModel>
    {
        public RationaleProjectValidator()
        {
            RuleFor(x => x.ProjectRationale)
                .NotEmpty()
                .WithMessage("Enter the rationale for the project");
        }
    }
}