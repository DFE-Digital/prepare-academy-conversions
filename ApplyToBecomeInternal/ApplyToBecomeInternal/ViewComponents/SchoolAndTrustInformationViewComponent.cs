using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.ViewComponents
{
	public class SchoolAndTrustInformationViewComponent : ViewComponent
	{
		private readonly IAcademyConversionProjectRepository _repository;

		public SchoolAndTrustInformationViewComponent(IAcademyConversionProjectRepository repository)
		{
			_repository = repository;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var id = int.Parse(ViewContext.RouteData.Values["id"].ToString());

			var response = await _repository.GetProjectById(id);
			if (!response.Success)
			{
				throw new InvalidOperationException();
			}

			var project = response.Body;

			var viewModel = new SchoolAndTrustInformationViewModel
			{
				Id = project.Id.ToString(),
				RecommendationForProject = project.RecommendationForProject,
				Author = project.Author,
				ClearedBy = project.ClearedBy,
				AcademyOrderRequired = project.AcademyOrderRequired,
				HeadTeacherBoardDate = project.HeadTeacherBoardDate.ToDateString(),
				PreviousHeadTeacherBoardDate = project.PreviousHeadTeacherBoardDateQuestion == "No" ? "No" : project.PreviousHeadTeacherBoardDate.ToDateString(),
				PreviousHeadTeacherBoardLink = project.PreviousHeadTeacherBoardLink,
				SchoolName = project.SchoolName,
				SchoolUrn = project.Urn.ToString(),
				LocalAuthority = project.LocalAuthority,
				TrustReferenceNumber = project.TrustReferenceNumber,
				NameOfTrust = project.NameOfTrust,
				SponsorReferenceNumber = project.SponsorReferenceNumber ?? "Not applicable",
				SponsorName = project.SponsorName ?? "Not applicable",
				AcademyTypeAndRoute = project.AcademyTypeAndRoute,
				ProposedAcademyOpeningDate = project.ProposedAcademyOpeningDate.ToDateString(true)
			};

			return View(viewModel);
		}
	}
}
