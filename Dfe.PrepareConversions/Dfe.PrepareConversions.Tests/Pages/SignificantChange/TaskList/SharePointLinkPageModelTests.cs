using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using TaskPages = Dfe.PrepareConversions.Pages.SignificantChange.TaskList;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList;

public class SharePointLinkPageModelTests
{
   public static IEnumerable<object[]> EvidenceValidationCases()
   {
      (Type ModelType, string PropertyName)[] properties =
      [
         (typeof(TaskPages.AdmissionsVariationDetails.IndexModel), nameof(TaskPages.AdmissionsVariationDetails.IndexModel.SupportingEvidence)),
         (typeof(TaskPages.FhaRisksOrIssuesRaised.IndexModel), nameof(TaskPages.FhaRisksOrIssuesRaised.IndexModel.SupportingEvidence)),
         (typeof(TaskPages.Funding.IndexModel), nameof(TaskPages.Funding.IndexModel.SupportingEvidence)),
         (typeof(TaskPages.HqtfConsideration.IndexModel), nameof(TaskPages.HqtfConsideration.IndexModel.SupportingEvidence)),
         (typeof(TaskPages.LandTransaction.IndexModel), nameof(TaskPages.LandTransaction.IndexModel.LandTransactionSupportingEvidence)),
         (typeof(TaskPages.LocalAuthorityObjections.IndexModel), nameof(TaskPages.LocalAuthorityObjections.IndexModel.LocalAuthorityObjectionsSupportingEvidenceLink)),
         (typeof(TaskPages.Ofsted.InspectionQuestionModel), nameof(TaskPages.Ofsted.InspectionQuestionModel.SupportingEvidence)),
         (typeof(TaskPages.PlanningPermission.IndexModel), nameof(TaskPages.PlanningPermission.IndexModel.PlanningPermissionSupportingEvidence)),
         (typeof(TaskPages.PublicSectorEqualityDuty.IndexModel), nameof(TaskPages.PublicSectorEqualityDuty.IndexModel.SupportingEvidence)),
         (typeof(TaskPages.ReligiousBodyConsultation.IndexModel), nameof(TaskPages.ReligiousBodyConsultation.IndexModel.SupportingEvidence)),
         (typeof(TaskPages.StakeholderObjections.IndexModel), nameof(TaskPages.StakeholderObjections.IndexModel.SupportingEvidence))
      ];
      const string schemeError = "The Supporting Evidence must have the https scheme";
      const string domainError = "Enter a Supporting Evidence in the correct format. SharePoint links start with 'https://educationgovuk.sharepoint.com' or 'https://educationgovuk-my.sharepoint.com/'";
      (string Link, string Error)[] links =
      [
         (null, null),
         (string.Empty, null),
         ("https://educationgovuk.sharepoint.com/sites/evidence", null),
         ("https://educationgovuk-my.sharepoint.com/personal/user/evidence", null),
         ("http://educationgovuk.sharepoint.com/sites/evidence", schemeError),
         ("educationgovuk.sharepoint.com/sites/evidence", schemeError),
         (" ", schemeError),
         ("https://example.com/evidence", domainError)
      ];

      foreach (var property in properties)
      {
         foreach (var link in links)
         {
            yield return [property.ModelType, property.PropertyName, link.Link, link.Error];
         }
      }
   }

   [Theory]
   [MemberData(nameof(EvidenceValidationCases))]
   public void Supporting_evidence_uses_sharepoint_validation(Type modelType, string propertyName, string link, string expectedError)
   {
      var repository = Mock.Of<ISignificantChangeProjectRepository>();
      object model = modelType == typeof(TaskPages.Ofsted.InspectionQuestionModel)
         ? new TaskPages.Ofsted.InspectionQuestionModel(repository, Mock.Of<ISession>(), new ErrorService())
         : Activator.CreateInstance(modelType, repository, new ErrorService());
      var context = new ValidationContext(model) { MemberName = propertyName };
      var results = new List<ValidationResult>();

      bool valid = Validator.TryValidateProperty(link, context, results);

      valid.Should().Be(expectedError == null);
      if (expectedError == null)
      {
         results.Should().BeEmpty();
      }
      else
      {
         results.Should().ContainSingle().Which.ErrorMessage.Should().Be(expectedError);
      }
   }
}