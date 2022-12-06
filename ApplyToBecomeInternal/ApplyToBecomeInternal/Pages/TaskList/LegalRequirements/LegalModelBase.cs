using ApplyToBecome.Data;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.AcademyConversion;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.LegalRequirements
{
	public class LegalModelBase : PageModel
	{
		protected readonly IAcademyConversionProjectRepository AcademyConversionProjectRepository;

		public LegalModelBase(IAcademyConversionProjectRepository _academyConversionProjectRepository)
		{
			AcademyConversionProjectRepository = _academyConversionProjectRepository;
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

            if (context.Result == default)
               await next();
         }

			context.Result ??= NotFound();
		}
		protected static bool ReturnPage(string returnPage)
		{
			return !string.IsNullOrWhiteSpace(returnPage);
		}
		protected (string, string, string) GetReturnPageAndFragment()
		{
			Request.Query.TryGetValue("return", out var returnQuery);
			Request.Query.TryGetValue("fragment", out var fragmentQuery);
			Request.Query.TryGetValue("back", out var backQuery);
			return (returnQuery, fragmentQuery, backQuery);
		}

		protected IActionResult ActionResult(int id, string fragment, string back)
		{
			string returnPage;
			(returnPage, fragment, back) = GetReturnPageAndFragment();
			if (ReturnPage(returnPage))
			{
				return !string.IsNullOrEmpty(back)
					? RedirectToPage(returnPage, null,
						new { id, @return = back, back }, fragment)
					: RedirectToPage(returnPage, null, new { id }, fragment);
			}

			return RedirectToPage(Links.LegalRequirements.Summary.Page, new { id });
		}
		protected ThreeOptions? ToLegalRequirementsEnum(ThreeOptions? requirements, string approved)
		{
			var result = approved switch
			{
				nameof(ThreeOptions.Yes) => ThreeOptions.Yes,
				nameof(ThreeOptions.No) => ThreeOptions.No,
				nameof(ThreeOptions.NotApplicable) => ThreeOptions.NotApplicable,
				_ => requirements
			};
			return result;
		}
	}
}
