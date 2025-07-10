using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.DocumentGeneration;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
{
   public static class SchoolAndTrustInformationAndProjectDatesGenerator
   {
      public static void AddSchoolAndTrustInfoAndProjectDates(DocumentBuilder documentBuilder, AcademyConversionProject project)
      {
         var academyRouteInfo = AddAcademyRouteInfo(project);
         documentBuilder.ReplacePlaceholderWithContent("AcademyRouteInfo", body =>
         {
            body.AddHeading("Conversion details", HeadingLevel.One);
            body.AddTable(academyRouteInfo);
         });

         var advisoryBoardDetails = AddAdvisoryBoardDetails(project);
         documentBuilder.ReplacePlaceholderWithContent("AdvisoryBoardDetails", body => body.AddTable(advisoryBoardDetails));

         var localAuthorityAndSponsorDetails = AddLocalAuthorityAndSponsorDetails(project);
         documentBuilder.ReplacePlaceholderWithContent("LocalAuthorityAndSponsorDetails", body =>
         {
            body.AddTable(localAuthorityAndSponsorDetails);
         });
      }

      public static List<TextElement[]> AddLocalAuthorityAndSponsorDetails(AcademyConversionProject project)
      {
         List<TextElement[]> localAuthorityAndSponsorDetails = new()
            {
                DocumentGeneratorStringSanitiser.CreateTextElements("Local authority", project.LocalAuthority),
                DocumentGeneratorStringSanitiser.CreateTextElements("Sponsor name", project.SponsorName),
                DocumentGeneratorStringSanitiser.CreateTextElements("Sponsor reference number", project.SponsorReferenceNumber)
            };

         return localAuthorityAndSponsorDetails;
      }

      public static List<TextElement[]> AddAdvisoryBoardDetails(AcademyConversionProject project)
      {
         List<TextElement[]> advisoryBoardDetails = new()
         {
               DocumentGeneratorStringSanitiser.CreateTextElements("Proposed decision date", project.HeadTeacherBoardDate?.ToDateString()),
               DocumentGeneratorStringSanitiser.CreateTextElements("Proposed academy opening date", project.ProposedConversionDate.ToDateString()),
               DocumentGeneratorStringSanitiser.CreateTextElements("Previously considered date", project.PreviousHeadTeacherBoardDate.ToDateString())
         };

         return advisoryBoardDetails;
      }

      public static List<TextElement[]> AddAcademyRouteInfo(AcademyConversionProject project)
      {
         List<TextElement[]> academyRouteInfo = project.AcademyTypeAndRoute switch
         {
            AcademyTypeAndRoutes.Voluntary => VoluntaryRouteInfo(project),
            AcademyTypeAndRoutes.Sponsored => SponsoredRouteInfo(project),
            _ => new List<TextElement[]>()
         };

         return academyRouteInfo;
      }

      public static List<TextElement[]> VoluntaryRouteInfo(AcademyConversionProject project)
      {
         var voluntaryRouteInfo = new List<TextElement[]>
         {
               DocumentGeneratorStringSanitiser.CreateTextElements("Academy type and route", project.AcademyTypeAndRoute),
         };

         bool isPreDeadline = project.ApplicationReceivedDate.HasValue && DateTime.Compare(project.ApplicationReceivedDate.Value, new DateTime(2024, 12, 20, 23, 59, 59, DateTimeKind.Utc)) <= 0;
         bool isVoluntaryConverionPreDeadline = isPreDeadline && project.AcademyTypeAndRoute == "Converter";

         if (isVoluntaryConverionPreDeadline)
         {
            voluntaryRouteInfo.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Grant funding amount", project.ConversionSupportGrantAmount?.ToMoneyString(true)));
            voluntaryRouteInfo.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Grant funding reason", project.ConversionSupportGrantChangeReason));
         }

         voluntaryRouteInfo.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Recommendation", project.RecommendationForProject));

         if (project.SchoolType.ToLower().Contains("pupil referral unit"))
         {
            voluntaryRouteInfo.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Number of sites", project.ConversionSupportGrantNumberOfSites?.ToString()));
         }

         return voluntaryRouteInfo;
      }

      public static List<TextElement[]> SponsoredRouteInfo(AcademyConversionProject project)
      {
         var sponsoredRouteInfo = new List<TextElement[]>
            {
                DocumentGeneratorStringSanitiser.CreateTextElements("Academy type and route", project.AcademyTypeAndRoute),
                DocumentGeneratorStringSanitiser.CreateTextElements("Grant funding type", project.ConversionSupportGrantType),
                DocumentGeneratorStringSanitiser.CreateTextElements("Grant funding amount", project.ConversionSupportGrantAmount?.ToMoneyString(true)),
                DocumentGeneratorStringSanitiser.CreateTextElements("Grant funding reason", project.ConversionSupportGrantChangeReason),
                DocumentGeneratorStringSanitiser.CreateTextElements("Is the school applying for an EIG (Environmental Improvement Grant)?", project.ConversionSupportGrantEnvironmentalImprovementGrant),
                DocumentGeneratorStringSanitiser.CreateTextElements("Has the Schools Notification Mailbox (SNM) received a Form 7?", project.Form7Received),
                DocumentGeneratorStringSanitiser.CreateTextElements("Date SNM received Form 7", project.Form7ReceivedDate?.ToDateString()),
                DocumentGeneratorStringSanitiser.CreateTextElements("Date directive academy order (DAO) pack sent", project.DaoPackSentDate?.ToDateString())
            };

         if (project.SchoolType.ToLower().Contains("pupil referral unit"))
         {
            sponsoredRouteInfo.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Number of sites", project.ConversionSupportGrantNumberOfSites?.ToString()));
         }

         return sponsoredRouteInfo;
      }
   }
}
