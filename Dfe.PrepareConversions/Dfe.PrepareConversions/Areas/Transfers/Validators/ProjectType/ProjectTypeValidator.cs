using Dfe.PrepareTransfers.Web.Models;
using FluentValidation;
using Dfe.PrepareTransfers.Web.Models;

namespace Dfe.PrepareTransfers.Web.Transfers.Validators.ProjectType
{
	public class ProjectTypeValidator : AbstractValidator<ProjectTypeViewModel>
	{
		public ProjectTypeValidator()
		{
			RuleFor(vm => vm.Type)
				.NotNull()
				.WithMessage("Select a project type");
		}
	}
}
