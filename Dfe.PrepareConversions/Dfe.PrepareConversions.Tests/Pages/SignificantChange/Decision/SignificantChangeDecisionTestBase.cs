using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Tests.PageObjects;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.Decision;

public abstract class SignificantChangeDecisionTestBase : BaseIntegrationTests
{
   protected const int ProjectId = 4001;

   protected readonly SignificantChangeDecisionWizard Wizard;

   protected SignificantChangeDecisionTestBase(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
      Wizard = new SignificantChangeDecisionWizard(Context);
   }

   protected SignificantChangeProjectResponse AddProject(int id = ProjectId, SignificantChangeDecisions? decision = null,
      SignificantChangeTaskStatus taskStatus = SignificantChangeTaskStatus.Completed, bool assigned = true)
   {
      SignificantChangeProjectResponse project = new()
      {
         Id = id,
         Urn = 10000000 + id,
         SchoolName = $"Significant change school {id}",
         Tier = 1,
         TrustName = "Example Trust",
         TrustUkprn = "12345678",
         AssignedUser = assigned ? new User("user-id", "assigned.user@test.local", "Assigned User") : null,
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         Status = "pre decision",
         LocalAuthorityName = "Test local authority",
         Decision = decision,
         ProjectDates = new() { Status = taskStatus },
         AdmissionsVariation = new() { Status = taskStatus },
         OtherStakeholderEngagement = new() { Status = taskStatus },
         ConsultationDuration = new() { Status = taskStatus },
         ReligiousBodyConsultation = new() { Status = taskStatus },
         OfstedInspection = new() { Status = taskStatus },
         EqualitiesImpactAssessment = new() { Status = taskStatus },
         PlanningPermission = new() { Status = taskStatus },
         Funding = new() { Status = taskStatus },
         FHARisksOrIssuesRaised = new() { Status = taskStatus },
         HQTFConsideration = new() { Status = taskStatus },
         Recommendation = new() { Status = taskStatus },
         AdmissionsVariationRecommendation = new() { Status = taskStatus },
         LocalAuthorityObjections = new() { Status = taskStatus },
         LandTransaction = new() { Status = taskStatus }
      };

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, id), project);

      if (decision.HasValue)
      {
         _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeDecision, id), new SignificantChangeDecision { Decision = decision.Value });
      }

      return project;
   }

   protected void AddDecisionPostStub()
   {
      _factory.AddAnyPostWithJsonRequest(PathFor.RecordSignificantChangeDecision, new SignificantChangeDecision());
   }

   protected static string PathTo(string step, int id = ProjectId)
   {
      return $"/significant-change/{id}/decision/{step}";
   }
}