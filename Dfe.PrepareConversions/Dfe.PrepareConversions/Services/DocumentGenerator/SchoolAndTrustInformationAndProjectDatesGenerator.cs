using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.DocumentGeneration;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using Dfe.PrepareConversions.Extensions;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
{
   public static class SchoolAndTrustInformationAndProjectDatesGenerator
   {
      public static void AddSchoolAndTrustInfoAndProjectDates(DocumentBuilder documentBuilder,
         AcademyConversionProject project)
      {
         AddAcademyRouteInfo(documentBuilder, project);
         AddAdvisoryBoardDetails(documentBuilder, project);
         AddLocalAuthorityAndSponsorDetails(documentBuilder, project);
      }
      private static void AddLocalAuthorityAndSponsorDetails(IDocumentBuilder builder, AcademyConversionProject project)
      {
         List<TextElement[]> localAuthorityAndSponsorDetails = new()
      {
         new[]
         {
            new TextElement { Value = "Local authority", Bold = true },
            new TextElement { Value = project.LocalAuthority }
         },
         new[]
         {
            new TextElement { Value = "Sponsor name", Bold = true },
            new TextElement { Value = project.SponsorName }
         },
         new[]
         {
            new TextElement { Value = "Sponsor reference number", Bold = true },
            new TextElement { Value = project.SponsorReferenceNumber }
         }
      };

         builder.ReplacePlaceholderWithContent("LocalAuthorityAndSponsorDetails", body =>
         {
            body.AddHeading("Conversion details", HeadingLevel.One);
            body.AddTable(localAuthorityAndSponsorDetails);
         });
      }
      private static void AddAdvisoryBoardDetails(IDocumentBuilder builder, AcademyConversionProject project)
      {
         List<TextElement[]> advisoryBoardDetails = new()
      {
         new[]
         {
            new TextElement { Value = "Date of Advisory Board", Bold = true },
            new TextElement { Value = project.HeadTeacherBoardDate.ToDateString()}
         },
         new[]
         {
            new TextElement { Value = "Proposed academy opening date", Bold = true },
            new TextElement { Value = project.ProposedAcademyOpeningDate.ToDateString() }
         },
         new[]
         {
            new TextElement { Value = "Previous Advisory Board", Bold = true },
            new TextElement { Value = project.PreviousHeadTeacherBoardDate.ToDateString() }
         }
      };

         builder.ReplacePlaceholderWithContent("AdvisoryBoardDetails", body => body.AddTable(advisoryBoardDetails));
         
      }
      private static void AddAcademyRouteInfo(IDocumentBuilder builder, AcademyConversionProject project)
      {
         List<TextElement[]> academyRouteInfo = new();
         switch (project.AcademyTypeAndRoute)
         {
            case AcademyTypeAndRoutes.Voluntary:
               academyRouteInfo = VoluntaryRouteInfo(project);
               break;
            case AcademyTypeAndRoutes.Sponsored:
               academyRouteInfo = SponsoredRouteInfo(project);
               break;
         }

         builder.ReplacePlaceholderWithContent("AcademyRouteInfo", body => body.AddTable(academyRouteInfo));
         
      }

      private static List<TextElement[]> VoluntaryRouteInfo(AcademyConversionProject project)
      {
         List<TextElement[]> voluntaryRouteInfo = new()
      {
         new[]
         {
            new TextElement { Value = "Academy type and route", Bold = true },
            new TextElement { Value = $"{project.AcademyTypeAndRoute} {project.ConversionSupportGrantChangeReason} {project.ConversionSupportGrantAmount.ToMoneyString(true)}" }
         },
         new[]
         {
            new TextElement { Value = "Recommendation", Bold = true },
            new TextElement { Value = project.RecommendationForProject }
         },
         new[]
         {
            new TextElement { Value = "Is an academy order (AO) required?", Bold = true },
            new TextElement { Value = project.AcademyOrderRequired }
         },
      };
         return voluntaryRouteInfo;

      }
      private static List<TextElement[]> SponsoredRouteInfo(AcademyConversionProject project)
      {
         List<TextElement[]> sponsoredRouteInfo = new()
      {
         new[]
         {
            new TextElement { Value = "Academy type and route", Bold = true },
            new TextElement { Value = $"{project.AcademyTypeAndRoute} {project.ConversionSupportGrantChangeReason} {project.ConversionSupportGrantAmount.ToMoneyString(true)}" }
         },
         new[]
         {
            new TextElement { Value = "Has the Schools Notification Mailbox (SNM) received a Form 7?", Bold = true },
            new TextElement { Value = project.Form7Received }
         },
         new[]
         {
            new TextElement { Value = "Date SNM received Form 7", Bold = true },
            new TextElement { Value = project.Form7ReceivedDate.ToDateString() }
         },
         new[]
         {
            new TextElement { Value = "Date directive academy order (DAO) pack sent", Bold = true },
            new TextElement { Value = project.DaoPackSentDate.ToDateString() }
         },
      };
         return sponsoredRouteInfo;
      }
   }
}
