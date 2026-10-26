using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.StakeholderObjections;
using Dfe.PrepareConversions.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.StakeholderObjections;

public class StakeholderObjectionsModelTests
{
   [Fact]
   public async Task OnGetAsync_Populates_all_stakeholder_answers_and_evidence()
   {
      const int id = 401;
      SignificantChangeProjectResponse project = BuildProject(id);
      project.OtherStakeholderEngagement = new SignificantChangeOtherStakeholderEngagementResponse
      {
         TrustConsultedStakeholders = false,
         TrustConsultedStakeholdersNotConsultedReason = "Consultation planned",
         WereThereAnyObjectionsRaised = true,
         StakeholderObjectionsDetails = "Objections were raised",
         RelevantStakeholdersNotifiedOfAdmissionsChanges = YesNoNotApplicableAnswer.No,
         RelevantStakeholdersNotifiedOfAdmissionsChangesReason = "Notifications are pending",
         SupportingEvidence = "https://example.test/evidence"
      };
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, project);
      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.TrustConsultedStakeholders.Should().BeFalse();
      sut.TrustConsultedStakeholdersNotConsultedReason.Should().Be("Consultation planned");
      sut.WereThereAnyObjectionsRaised.Should().BeTrue();
      sut.StakeholderObjectionsDetails.Should().Be("Objections were raised");
      sut.RelevantStakeholdersNotifiedOfAdmissionsChanges.Should().Be(YesNoNotApplicableAnswer.No);
      sut.RelevantStakeholdersNotifiedOfAdmissionsChangesReason.Should().Be("Notifications are pending");
      sut.SupportingEvidence.Should().Be("https://example.test/evidence");
   }

   [Fact]
   public async Task OnPostAsync_Saves_all_answers_in_one_request_and_redirects()
   {
      const int id = 402;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      repository
         .Setup(x => x.SetOtherStakeholderEngagement(id, It.IsAny<SetSignificantChangeOtherStakeholderEngagementCommand>()))
         .Returns(Task.CompletedTask);
      IndexModel sut = BuildModel(repository.Object);
      sut.TrustConsultedStakeholders = false;
      sut.TrustConsultedStakeholdersNotConsultedReason = "Trust will consult later";
      sut.WereThereAnyObjectionsRaised = true;
      sut.StakeholderObjectionsDetails = "Objections addressed";
      sut.RelevantStakeholdersNotifiedOfAdmissionsChanges = YesNoNotApplicableAnswer.NotApplicable;
      sut.RelevantStakeholdersNotifiedOfAdmissionsChangesReason = "Discarded";
      sut.SupportingEvidence = "https://example.test/evidence";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      repository.Verify(x => x.SetOtherStakeholderEngagement(
         id,
         It.Is<SetSignificantChangeOtherStakeholderEngagementCommand>(command =>
            command.TrustConsultedStakeholders == false
            && command.TrustConsultedStakeholdersNotConsultedReason == "Trust will consult later"
            && command.WereThereAnyObjectionsRaised == true
            && command.StakeholderObjectionsDetails == "Objections addressed"
            && command.RelevantStakeholdersNotifiedOfAdmissionsChanges == YesNoNotApplicableAnswer.NotApplicable
            && command.RelevantStakeholdersNotifiedOfAdmissionsChangesReason == null
            && command.SupportingEvidence == "https://example.test/evidence")), Times.Once);
   }

   [Theory]
   [InlineData("consultation")]
   [InlineData("objections")]
   [InlineData("notifications")]
   public async Task OnPostAsync_Requires_conditional_details(string missingDetails)
   {
      const int id = 403;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      IndexModel sut = BuildModel(repository.Object);
      sut.TrustConsultedStakeholders = true;
      sut.WereThereAnyObjectionsRaised = false;
      sut.RelevantStakeholdersNotifiedOfAdmissionsChanges = YesNoNotApplicableAnswer.Yes;

      if (missingDetails == "consultation")
      {
         sut.TrustConsultedStakeholders = false;
      }
      else if (missingDetails == "objections")
      {
         sut.WereThereAnyObjectionsRaised = true;
      }
      else
      {
         sut.RelevantStakeholdersNotifiedOfAdmissionsChanges = YesNoNotApplicableAnswer.No;
      }

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ErrorCount.Should().BeGreaterThan(0);
      repository.Verify(x => x.SetOtherStakeholderEngagement(id, It.IsAny<SetSignificantChangeOtherStakeholderEngagementCommand>()), Times.Never);
   }

   private static Mock<ISignificantChangeProjectRepository> BuildRepository(int id, SignificantChangeProjectResponse project)
   {
      Mock<ISignificantChangeProjectRepository> repository = new();
      repository.Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.OK, project));
      return repository;
   }

   private static IndexModel BuildModel(ISignificantChangeProjectRepository repository)
   {
      DefaultHttpContext httpContext = new();
      ModelStateDictionary modelState = new();
      ActionContext actionContext = new(httpContext, new RouteData(), new ActionDescriptor(), modelState);
      PageContext pageContext = new(actionContext)
      {
         ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState)
      };
      return new IndexModel(repository, new ErrorService()) { PageContext = pageContext };
   }

   private static SignificantChangeProjectResponse BuildProject(int id) =>
      new()
      {
         Id = id,
         ApplicationId = "1234",
         ApplicationReference = "56789",
         Urn = 10000000 + id,
         SchoolName = "Test school",
         Tier = 1,
         TrustName = "Example trust",
         TrustUkprn = "12345678",
         LocalAuthorityName = "Test local authority",
         TypeOfSignificantChange = "Route A",
         Status = "pre decision"
      };
}
