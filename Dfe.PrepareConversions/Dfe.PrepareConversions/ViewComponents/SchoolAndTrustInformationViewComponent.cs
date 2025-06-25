using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.ViewComponents;

public class SchoolAndTrustInformationViewComponent : ViewComponent
{
   private readonly IAcademyConversionProjectRepository _repository;

   public SchoolAndTrustInformationViewComponent(IAcademyConversionProjectRepository repository)
   {
      _repository = repository;
   }

   private bool IsVoluntaryConversionSupportGrantVisible(DateTime? applicationReceivedDate, string academyTypeAndRoute)
   {
      var isPreDeadline = applicationReceivedDate.HasValue && DateTime.Compare(applicationReceivedDate.Value, new DateTime(2024, 12, 20, 23, 59, 59, DateTimeKind.Utc)) <= 0;

      return isPreDeadline && academyTypeAndRoute is AcademyTypeAndRoutes.Voluntary;
   }

   public async Task<IViewComponentResult> InvokeAsync()
   {
      int id = int.Parse(ViewContext.RouteData.Values["id"]?.ToString() ?? string.Empty);
      bool isPreview = bool.Parse(ViewData["IsPreview"]?.ToString());

      ApiResponse<AcademyConversionProject> response = await _repository.GetProjectById(id);
      if (!response.Success)
      {
         throw new InvalidOperationException();
      }

      AcademyConversionProject project = response.Body;

      SchoolAndTrustInformationViewModel viewModel = new()
      {
         IsPreview = isPreview,
         Id = project.Id.ToString(),
         IsDao = project.AcademyTypeAndRoute is AcademyTypeAndRoutes.Sponsored,
         RecommendationForProject = project.RecommendationForProject,
         Author = project.Author,
         ClearedBy = project.ClearedBy,
         HeadTeacherBoardDate = project.HeadTeacherBoardDate.ToDateString(),
         PreviousHeadTeacherBoardDate = project.PreviousHeadTeacherBoardDateQuestion == "No" ? "No" : project.PreviousHeadTeacherBoardDate.ToDateString(),
         PreviousHeadTeacherBoardLink = project.PreviousHeadTeacherBoardLink,
         ProposedAcademyOpeningDate = project.ProposedConversionDate.ToDateString(),
         SchoolName = project.SchoolName,
         SchoolUrn = project.Urn.ToString(),
         SchoolType = project?.SchoolType?.ToString(),
         LocalAuthority = project.LocalAuthority,
         TrustReferenceNumber = project.TrustReferenceNumber,
         NameOfTrust = project.NameOfTrust,
         SponsorReferenceNumber = project.SponsorReferenceNumber ?? "Not applicable",
         SponsorName = project.SponsorName ?? "Not applicable",
         AcademyTypeAndRoute = project.AcademyTypeAndRoute,
         Form7Received = project.Form7Received,
         Form7ReceivedDate = project.Form7ReceivedDate.ToDateString(),
         WasForm7Received = project.Form7Received is not null && project.Form7Received.Equals("Yes"),
         IsVoluntaryConversionSupportGrantVisible = IsVoluntaryConversionSupportGrantVisible(project.ApplicationReceivedDate, project.AcademyTypeAndRoute),
         ConversionSupportGrantAmount = project?.ConversionSupportGrantAmount?.ToMoneyString(true),
         ConversionSupportGrantChangeReason = project.ConversionSupportGrantChangeReason,
         ConversionSupportGrantType = project.ConversionSupportGrantType,
         ConversionSupportGrantEnvironmentalImprovementGrant = project.ConversionSupportGrantEnvironmentalImprovementGrant,
         ConversionSupportNumberOfSites = project?.ConversionSupportGrantNumberOfSites,
         DaoPackSentDate = project.DaoPackSentDate.ToDateString(),
         IsReadOnly = project.IsReadOnly
      };

      if (project.ApplicationReceivedDate.HasValue && DateTime.Compare(project.ApplicationReceivedDate.Value, new DateTime(2024, 12, 20, 23, 59, 59, DateTimeKind.Utc)) <= 0)
      {
         viewModel.IsApplicationReceivedBeforeSupportGrantDeadline = true;
      }

      return View(viewModel);
   }
}
