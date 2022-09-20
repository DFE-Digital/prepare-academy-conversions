using ApplyToBecome.Data;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.LegalRequirements
{
	public class LegalModelBase : PageModel
	{
		protected readonly IAcademyConversionProjectRepository AcademyConversionProjectRepository;

		public LegalModelBase(IAcademyConversionProjectRepository academyConversionProjectRepository)
		{
			AcademyConversionProjectRepository = academyConversionProjectRepository;
		}

		public int Id { get; private set; }
		public string SchoolName { get; private set; }
		public ApplyToBecome.Data.Models.AcademyConversion.LegalRequirements Requirements { get; private set; }

		public override async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
		{
			if (context.HandlerArguments.ContainsKey(nameof(Id)))
			{
				Id = (int)context.HandlerArguments[nameof(Id)];

				ApiResponse<AcademyConversionProject> projectResponse = await AcademyConversionProjectRepository.GetProjectById(Id);
				if (projectResponse.Success)
				{
					SchoolName = projectResponse.Body.SchoolName;
				}
				else
				{
					context.Result = NotFound();
				}

				ApiResponse<AcademyConversionProject> project =
					await AcademyConversionProjectRepository.GetProjectById(Id);
				if (project.Success)
				{
					Requirements = ApplyToBecome.Data.Models.AcademyConversion.LegalRequirements.From(project.Body);
				}
				else
				{
					context.Result = NotFound();
				}

				await next();
			}

			context.Result ??= NotFound();
		}
	}
}
