using ApplyToBecome.Data;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.LegalRequirements
{
	public class LegalModelBase : PageModel
	{
		private readonly IAcademyConversionProjectRepository _academyConversionProjectRepository;
		protected readonly ILegalRequirementsRepository LegalRequirementsRepository;

		public LegalModelBase(ILegalRequirementsRepository legalRequirementsRepository,
			IAcademyConversionProjectRepository academyConversionProjectRepository)
		{
			LegalRequirementsRepository = legalRequirementsRepository;
			_academyConversionProjectRepository = academyConversionProjectRepository;
		}

		public int Id { get; private set; }
		public string SchoolName { get; private set; }
		public ApplyToBecome.Data.Models.AcademyConversion.LegalRequirements LegalRequirements { get; private set; }

		public override async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
		{
			if (context.HandlerArguments.ContainsKey(nameof(Id)))
			{
				Id = (int)context.HandlerArguments[nameof(Id)];

				ApiResponse<AcademyConversionProject> projectResponse = await _academyConversionProjectRepository.GetProjectById(Id);
				if (projectResponse.Success)
				{
					SchoolName = projectResponse.Body.SchoolName;
				}
				else
				{
					context.Result = NotFound();
				}

				ApiResponse<ApplyToBecome.Data.Models.AcademyConversion.LegalRequirements> legalRequirementsResponse =
					await LegalRequirementsRepository.GetRequirementsByProjectId(Id);
				if (legalRequirementsResponse.Success)
				{
					LegalRequirements = legalRequirementsResponse.Body;
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
