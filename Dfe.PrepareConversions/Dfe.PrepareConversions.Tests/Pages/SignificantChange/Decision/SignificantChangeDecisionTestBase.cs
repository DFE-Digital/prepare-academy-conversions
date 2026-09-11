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

   protected SignificantChangeProjectResponse AddProject(int id = ProjectId)
   {
      SignificantChangeProjectResponse project = new()
      {
         Id = id,
         Urn = 10000000 + id,
         SchoolName = $"Significant change school {id}",
         Tier = 1,
         TrustName = "Example Trust",
         TrustUkprn = "12345678",
         AssignedUser = new User("user-id", "assigned.user@test.local", "Assigned User"),
         TypeOfSignificantChange = "Route A",
         Status = "pre decision"
      };

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, id), project);

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