using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.ViewComponents
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
            IsDao = project.ApplicationReceivedDate is null,
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
				ConversionSupportGrantAmount = project.ConversionSupportGrantAmount?.ToMoneyString(true),
				ConversionSupportGrantChangeReason = project.ConversionSupportGrantChangeReason,
				ProposedAcademyOpeningDate = project.ProposedAcademyOpeningDate.ToDateString(true),
            DaoPackSentDate = project.DaoPackSentDate.ToDateString()
			};

			return View(viewModel);
		}
	}
}
