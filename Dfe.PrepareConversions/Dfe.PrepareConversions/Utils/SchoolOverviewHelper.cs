using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.ViewModels;
using System;
using DocumentFormat.OpenXml.Drawing;
using AngleSharp.Text;

namespace Dfe.PrepareConversions.Utils
{
   public static class SchoolOverviewHelper
   {
      public static SetSchoolOverviewModel CreateUpdateSchoolOverview(ProjectViewModel projectViewModel)
      {
         return new SetSchoolOverviewModel(
             projectViewModel.Id.ToInteger(0),
             projectViewModel.PublishedAdmissionNumber,
             projectViewModel.ViabilityIssues,
             projectViewModel.FinancialDeficit,
             projectViewModel.NumberOfPlacesFundedFor,
             projectViewModel.NumberOfResidentialPlaces,
             projectViewModel.NumberOfFundedResidentialPlaces,
             projectViewModel.PartOfPfiScheme,
             projectViewModel.PfiSchemeDetails,
             projectViewModel.DistanceFromSchoolToTrustHeadquarters,
             projectViewModel.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
             projectViewModel.MemberOfParliamentNameAndParty,
             projectViewModel.PupilsAttendingGroupPermanentlyExcluded,
             projectViewModel.PupilsAttendingGroupMedicalAndHealthNeeds,
             projectViewModel.PupilsAttendingGroupTeenageMums
         );
      }
   }
}
