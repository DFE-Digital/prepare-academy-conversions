using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.ReligiousBodyConsultation;
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

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.ReligiousBodyConsultation;

public class ReligiousBodyConsultationModelTests
{
   [Fact]
   public async Task OnGetAsync_Populates_all_religious_body_answers_and_evidence()
   {
      const int id = 601;
      SignificantChangeProjectResponse project = BuildProject(id);
      project.ReligiousBodyConsultation = new SignificantChangeReligiousBodyConsultationResponse
      {
         TrustConsultedReligiousBody = YesNoNotApplicableAnswer.No,
         TrustConsultedReligiousBodyNotConsultedReason = "Consultation is planned",
         ReligiousBodyRaisedObjection = YesNoNotApplicableAnswer.Yes,
         ReligiousBodyConsentGranted = YesNoNotApplicableAnswer.NotApplicable,
         SupportingEvidence = "https://example.test/evidence"
      };
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, project);
      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.TrustConsultedReligiousBody.Should().Be(YesNoNotApplicableAnswer.No);
      sut.TrustConsultedReligiousBodyNotConsultedReason.Should().Be("Consultation is planned");
      sut.ReligiousBodyRaisedObjection.Should().Be(YesNoNotApplicableAnswer.Yes);
      sut.ReligiousBodyConsentGranted.Should().Be(YesNoNotApplicableAnswer.NotApplicable);
      sut.SupportingEvidence.Should().Be("https://example.test/evidence");
   }

   [Fact]
   public async Task OnPostAsync_Saves_all_answers_in_one_request_and_redirects()
   {
      const int id = 602;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      repository
         .Setup(x => x.SetReligiousBodyConsultation(id, It.IsAny<SetSignificantChangeReligiousBodyConsultationCommand>()))
         .Returns(Task.CompletedTask);
      IndexModel sut = BuildModel(repository.Object);
      sut.TrustConsultedReligiousBody = YesNoNotApplicableAnswer.No;
      sut.TrustConsultedReligiousBodyNotConsultedReason = "Consultation will be scheduled";
      sut.ReligiousBodyRaisedObjection = YesNoNotApplicableAnswer.Yes;
      sut.ReligiousBodyRaisedObjectionReason = "Cleared";
      sut.ReligiousBodyConsentGranted = YesNoNotApplicableAnswer.NotApplicable;
      sut.ReligiousBodyConsentNotGrantedReason = "Discarded";
      sut.SupportingEvidence = "https://example.test/evidence";

      IActionResult result = await sut.OnPostAsync(id);

      RedirectToPageResult redirect = Assert.IsType<RedirectToPageResult>(result);
      redirect.PageName.Should().Be(Links.SignificantChange.SignificantChangeTaskList.Page);
      repository.Verify(x => x.SetReligiousBodyConsultation(
         id,
         It.Is<SetSignificantChangeReligiousBodyConsultationCommand>(command =>
            command.TrustConsultedReligiousBody == YesNoNotApplicableAnswer.No
            && command.TrustConsultedReligiousBodyNotConsultedReason == "Consultation will be scheduled"
            && command.ReligiousBodyRaisedObjection == YesNoNotApplicableAnswer.Yes
            && command.ReligiousBodyRaisedObjectionReason == null
            && command.ReligiousBodyConsentGranted == YesNoNotApplicableAnswer.NotApplicable
            && command.ReligiousBodyConsentNotGrantedReason == null
            && command.SupportingEvidence == "https://example.test/evidence")), Times.Once);
   }

   [Fact]
   public async Task OnPostAsync_Requires_reason_when_no_is_selected()
   {
      const int id = 603;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      IndexModel sut = BuildModel(repository.Object);
      sut.TrustConsultedReligiousBody = YesNoNotApplicableAnswer.No;
      sut.TrustConsultedReligiousBodyNotConsultedReason = " ";
      sut.ReligiousBodyRaisedObjection = YesNoNotApplicableAnswer.Yes;
      sut.ReligiousBodyConsentGranted = YesNoNotApplicableAnswer.Yes;

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ContainsKey(nameof(IndexModel.TrustConsultedReligiousBodyNotConsultedReason)).Should().BeTrue();
      repository.Verify(x => x.SetReligiousBodyConsultation(id, It.IsAny<SetSignificantChangeReligiousBodyConsultationCommand>()), Times.Never);
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
         Urn = 10000000 + id,
         SchoolName = "Test school",
         Tier = 1,
         TrustName = "Example trust",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         Status = "pre decision",
         LocalAuthorityName = "Test local authority"
      };
}
