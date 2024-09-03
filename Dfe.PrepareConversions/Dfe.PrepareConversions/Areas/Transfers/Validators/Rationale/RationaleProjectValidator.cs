using FluentValidation;
using Dfe.PrepareTransfers.Web.Models.Rationale;
using Dfe.PrepareTransfers.Web.Models.Rationale;

namespace Dfe.PrepareTransfers.Web.Transfers.Validators.Rationale
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