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
            body.AddTable(localAuthorityAndSponsorDetails);
         });
      }
      private static void AddAdvisoryBoardDetails(IDocumentBuilder builder, AcademyConversionProject project)
      {
         List<TextElement[]> advisoryBoardDetails = new()
      {
         new[]
         {
            new TextElement { Value = "Date of advisory board", Bold = true },
            new TextElement { Value = project.HeadTeacherBoardDate.ToDateString()}
         },
         new[]
         {
            new TextElement { Value = "Proposed academy opening date", Bold = true },
            new TextElement { Value = project.ProposedAcademyOpeningDate.ToDateString() }
         },
         new[]
         {
            new TextElement { Value = "Previous advisory board", Bold = true },
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

         builder.ReplacePlaceholderWithContent("AcademyRouteInfo", body =>
         {
            body.AddHeading("Conversion details", HeadingLevel.One);
            body.AddTable(academyRouteInfo);
         });

      }

      private static List<TextElement[]> VoluntaryRouteInfo(AcademyConversionProject project)
      {
         List<TextElement[]> voluntaryRouteInfo = new()
      {
         new[]
         {
            new TextElement { Value = "Academy type and route", Bold = true },
            new TextElement { Value = project.AcademyTypeAndRoute }
         },
         new[]
         {
            new TextElement { Value = "Grant funding amount", Bold = true },
            new TextElement { Value = project.ConversionSupportGrantAmount.ToMoneyString(true) }
         },
         new[]
         {
            new TextElement { Value = "Grant funding reason", Bold = true },
            new TextElement { Value = project.ConversionSupportGrantChangeReason }
         },
         new[]
         {
            new TextElement { Value = "Recommendation", Bold = true },
            new TextElement { Value = project.RecommendationForProject }
         },
      };

         if (project.SchoolType.ToLower().Contains("pupil referral unit"))
         {
            voluntaryRouteInfo.Add(new[]
            {
               new TextElement { Value = "Number of sites", Bold = true },
               new TextElement { Value = project.ConversionSupportGrantNumberOfSites}
            }
         );
         }

         return voluntaryRouteInfo;

      }
      private static List<TextElement[]> SponsoredRouteInfo(AcademyConversionProject project)
      {
         List<TextElement[]> sponsoredRouteInfo = new()
      {
         new[]
         {
            new TextElement { Value = "Academy type and route", Bold = true },
            new TextElement { Value = project.AcademyTypeAndRoute}
         },
         new[]
         {
            new TextElement { Value = "Grant funding type", Bold = true },
            new TextElement { Value = project.ConversionSupportGrantType }
         },
         new[]
         {
            new TextElement { Value = "Grant funding amount", Bold = true },
            new TextElement { Value = project.ConversionSupportGrantAmount.ToMoneyString(true) }
         },
         new[]
         {
            new TextElement { Value = "Grant funding reason", Bold = true },
            new TextElement { Value = project.ConversionSupportGrantChangeReason }
         },
         new[]
         {
            new TextElement { Value = "Is the school applying for an EIG (Environmental Improvement Grant)?", Bold = true },
            new TextElement { Value = project.ConversionSupportGrantEnvironmentalImprovementGrant }
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

         if (project.SchoolType.ToLower().Contains("pupil referral unit"))
         {
            sponsoredRouteInfo.Add(new[]
            {
               new TextElement { Value = "Number of sites", Bold = true },
               new TextElement { Value = project.ConversionSupportGrantNumberOfSites}
            }
         );
         }

         return sponsoredRouteInfo;
      }
   }
}
