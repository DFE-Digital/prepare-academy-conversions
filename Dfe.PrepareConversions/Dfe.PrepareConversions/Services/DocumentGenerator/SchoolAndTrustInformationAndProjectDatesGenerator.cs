using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.DocumentGeneration;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
{
   public static class SchoolAndTrustInformationAndProjectDatesGenerator
   {
      public static void AddSchoolAndTrustInfoAndProjectDates(DocumentBuilder documentBuilder, AcademyConversionProject project)
      {
         AddAcademyRouteInfo(documentBuilder, project);
         AddAdvisoryBoardDetails(documentBuilder, project);
         AddLocalAuthorityAndSponsorDetails(documentBuilder, project);
      }

      private static void AddLocalAuthorityAndSponsorDetails(IDocumentBuilder builder, AcademyConversionProject project)
      {
         List<TextElement[]> localAuthorityAndSponsorDetails = new()
            {
                DocumentGeneratorStringSanitiser.CreateTextElements("Local authority", project.LocalAuthority),
                DocumentGeneratorStringSanitiser.CreateTextElements("Sponsor name", project.SponsorName),
                DocumentGeneratorStringSanitiser.CreateTextElements("Sponsor reference number", project.SponsorReferenceNumber)
            };

         builder.ReplacePlaceholderWithContent("LocalAuthorityAndSponsorDetails", body =>
         {
            body.AddTable(localAuthorityAndSponsorDetails);
         });
      }

      private static void AddAdvisoryBoardDetails(IDocumentBuilder builder, AcademyConversionProject project)
      {
         List<TextElement[]> advisoryBoardDetails = new()
            {
                DocumentGeneratorStringSanitiser.CreateTextElements("Date of advisory board", project.HeadTeacherBoardDate.ToDateString()),
                DocumentGeneratorStringSanitiser.CreateTextElements("Proposed academy opening date", project.ProposedConversionDate.ToDateString()),
                DocumentGeneratorStringSanitiser.CreateTextElements("Previous advisory board", project.PreviousHeadTeacherBoardDate.ToDateString())
            };

         builder.ReplacePlaceholderWithContent("AdvisoryBoardDetails", body => body.AddTable(advisoryBoardDetails));
      }

      private static void AddAcademyRouteInfo(IDocumentBuilder builder, AcademyConversionProject project)
      {
         List<TextElement[]> academyRouteInfo = project.AcademyTypeAndRoute switch
         {
            AcademyTypeAndRoutes.Voluntary => VoluntaryRouteInfo(project),
            AcademyTypeAndRoutes.Sponsored => SponsoredRouteInfo(project),
            _ => new List<TextElement[]>()
         };

         builder.ReplacePlaceholderWithContent("AcademyRouteInfo", body =>
         {
            body.AddHeading("Conversion details", HeadingLevel.One);
            body.AddTable(academyRouteInfo);
         });
      }

      private static List<TextElement[]> VoluntaryRouteInfo(AcademyConversionProject project)
      {
         var voluntaryRouteInfo = new List<TextElement[]>
            {
                DocumentGeneratorStringSanitiser.CreateTextElements("Academy type and route", project.AcademyTypeAndRoute),
                DocumentGeneratorStringSanitiser.CreateTextElements("Grant funding amount", project.ConversionSupportGrantAmount.ToMoneyString(true)),
                DocumentGeneratorStringSanitiser.CreateTextElements("Grant funding reason", project.ConversionSupportGrantChangeReason),
                DocumentGeneratorStringSanitiser.CreateTextElements("Recommendation", project.RecommendationForProject)
            };

         if (project.SchoolType.ToLower().Contains("pupil referral unit"))
         {
            voluntaryRouteInfo.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Number of sites", project.ConversionSupportGrantNumberOfSites.ToString()));
         }

         return voluntaryRouteInfo;
      }

      private static List<TextElement[]> SponsoredRouteInfo(AcademyConversionProject project)
      {
         var sponsoredRouteInfo = new List<TextElement[]>
            {
                DocumentGeneratorStringSanitiser.CreateTextElements("Academy type and route", project.AcademyTypeAndRoute),
                DocumentGeneratorStringSanitiser.CreateTextElements("Grant funding type", project.ConversionSupportGrantType),
                DocumentGeneratorStringSanitiser.CreateTextElements("Grant funding amount", project.ConversionSupportGrantAmount.ToMoneyString(true)),
                DocumentGeneratorStringSanitiser.CreateTextElements("Grant funding reason", project.ConversionSupportGrantChangeReason),
                DocumentGeneratorStringSanitiser.CreateTextElements("Is the school applying for an EIG (Environmental Improvement Grant)?", project.ConversionSupportGrantEnvironmentalImprovementGrant),
                DocumentGeneratorStringSanitiser.CreateTextElements("Has the Schools Notification Mailbox (SNM) received a Form 7?", project.Form7Received),
                DocumentGeneratorStringSanitiser.CreateTextElements("Date SNM received Form 7", project.Form7ReceivedDate.ToDateString()),
                DocumentGeneratorStringSanitiser.CreateTextElements("Date directive academy order (DAO) pack sent", project.DaoPackSentDate.ToDateString())
            };

         if (project.SchoolType.ToLower().Contains("pupil referral unit"))
         {
            sponsoredRouteInfo.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Number of sites", project.ConversionSupportGrantNumberOfSites.ToString()));
         }

         return sponsoredRouteInfo;
      }
   }
}
