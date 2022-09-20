using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.SchoolAndTrustInformation
{
	public class RouteAndGrant : CommonPageModel
	{
		[BindProperty]
		public InputModel RouteAndGrantViewModel { get; set; }

		public class InputModel 
		{
			[ModelBinder(BinderType = typeof(MonetaryInputModelBinder))]
			[Display(Name = "Conversion support grant")]
			[Range(typeof(decimal), "0", "25000", ErrorMessage = "Enter an amount that is £25,000 or less, for example £20,000")]
			[BindProperty(Name = "conversion-support-grant-amount")]
			public decimal? ConversionSupportGrantAmount { get; set; }

			[BindProperty(Name = "conversion-support-grant-change-reason")]
			[DisplayFormat(ConvertEmptyStringToNull = false)]
			[SupportGrantValidator]
			public string ConversionSupportGrantChangeReason { get; set; }
		}

		public RouteAndGrant(IAcademyConversionProjectRepository repository, ErrorService errorService) :base(repository,errorService)
		{
		}

		public string SuccessPage
		{
			get
			{
				return TempData[nameof(SuccessPage)].ToString();
			}
			set
			{
				TempData[nameof(SuccessPage)] = value;
			}
		}

		public virtual async Task<IActionResult> OnGetAsync()
		{
			var project = await _repository.GetProjectById(Id);
			if (!project.Success)
			{
				// 404 logic
				return NotFound();
			}
			var conversionProject = project.Body;
			RouteAndGrantViewModel = new InputModel
			{
				ConversionSupportGrantAmount = conversionProject.ConversionSupportGrantAmount,
				ConversionSupportGrantChangeReason = conversionProject.ConversionSupportGrantChangeReason
			};

			return Page();
		}

		public virtual async Task<IActionResult> OnPostAsync()
		{
			_errorService.AddErrors(Request.Form.Keys, ModelState);
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var response = await _repository.UpdateProject(Id, Build());

			if (!response.Success)
			{
				_errorService.AddTramsError();
				return Page();
			}

			var (returnPage, fragment) = GetReturnPageAndFragment();
			if (!string.IsNullOrWhiteSpace(returnPage))
			{
				return RedirectToPage(returnPage, null, new { Id }, fragment);
			}

			return RedirectToPage(SuccessPage, new { Id });
		}

		protected UpdateAcademyConversionProject Build()
		{
			return new UpdateAcademyConversionProject
			{
				ConversionSupportGrantAmount = RouteAndGrantViewModel.ConversionSupportGrantAmount,
				ConversionSupportGrantChangeReason = RouteAndGrantViewModel.ConversionSupportGrantChangeReason
			};
		}
	}
}
