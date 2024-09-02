using Dfe.PrepareConversions.Areas.Transfers.Models;
using FluentValidation;
using Dfe.PrepareConversions.Areas.Transfers.Models;

namespace Dfe.PrepareConversions.Areas.Transfers.Validators.ProjectType
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
