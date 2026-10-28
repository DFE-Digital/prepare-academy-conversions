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
   [InlineData("consultation", nameof(IndexModel.TrustConsultedStakeholdersNotConsultedReason))]
   [InlineData("objections", nameof(IndexModel.StakeholderObjectionsDetails))]
   [InlineData("notifications", nameof(IndexModel.RelevantStakeholdersNotifiedOfAdmissionsChangesReason))]
   public async Task OnPostAsync_Requires_conditional_details(string missingDetails, string field)
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
      sut.ModelState.ErrorCount.Should().Be(1);
      sut.ModelState[field]!.Errors.Should().ContainSingle();
      repository.Verify(x => x.SetOtherStakeholderEngagement(id, It.IsAny<SetSignificantChangeOtherStakeholderEngagementCommand>()), Times.Never);
   }

   [Theory]
   [InlineData(YesNoNotApplicableAnswer.Yes)]
   [InlineData(YesNoNotApplicableAnswer.NotApplicable)]
   public async Task OnPostAsync_Does_not_require_or_send_hidden_details(YesNoNotApplicableAnswer notified)
   {
      const int id = 404;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      repository.Setup(x => x.SetOtherStakeholderEngagement(id, It.IsAny<SetSignificantChangeOtherStakeholderEngagementCommand>()))
         .Returns(Task.CompletedTask);
      IndexModel sut = BuildModel(repository.Object);
      sut.TrustConsultedStakeholders = true;
      sut.TrustConsultedStakeholdersNotConsultedReason = "Discarded";
      sut.WereThereAnyObjectionsRaised = false;
      sut.StakeholderObjectionsDetails = " ";
      sut.RelevantStakeholdersNotifiedOfAdmissionsChanges = notified;
      sut.RelevantStakeholdersNotifiedOfAdmissionsChangesReason = null;

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<RedirectToPageResult>();
      sut.ModelState.IsValid.Should().BeTrue();
      repository.Verify(x => x.SetOtherStakeholderEngagement(id,
         It.Is<SetSignificantChangeOtherStakeholderEngagementCommand>(command =>
            command.TrustConsultedStakeholders == true
            && command.TrustConsultedStakeholdersNotConsultedReason == null
            && command.WereThereAnyObjectionsRaised == false
            && command.StakeholderObjectionsDetails == null
            && command.RelevantStakeholdersNotifiedOfAdmissionsChanges == notified
            && command.RelevantStakeholdersNotifiedOfAdmissionsChangesReason == null)), Times.Once);
   }

   [Theory]
   [InlineData(nameof(IndexModel.TrustConsultedStakeholders))]
   [InlineData(nameof(IndexModel.WereThereAnyObjectionsRaised))]
   [InlineData(nameof(IndexModel.RelevantStakeholdersNotifiedOfAdmissionsChanges))]
   public async Task OnPostAsync_Requires_each_answer(string field)
   {
      const int id = 405;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      IndexModel sut = BuildModel(repository.Object);
      sut.TrustConsultedStakeholders = field == nameof(IndexModel.TrustConsultedStakeholders) ? null : true;
      sut.WereThereAnyObjectionsRaised = field == nameof(IndexModel.WereThereAnyObjectionsRaised) ? null : false;
      sut.RelevantStakeholdersNotifiedOfAdmissionsChanges =
         field == nameof(IndexModel.RelevantStakeholdersNotifiedOfAdmissionsChanges) ? null : YesNoNotApplicableAnswer.Yes;

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ErrorCount.Should().Be(1);
      sut.ModelState[field]!.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be("Select an option");
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
