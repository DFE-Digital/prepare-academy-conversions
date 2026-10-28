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
         ReligiousBodyRaisedObjectionReason = "Objection details",
         ReligiousBodyConsentGranted = YesNoNotApplicableAnswer.No,
         ReligiousBodyConsentNotGrantedReason = "Consent is pending",
         SupportingEvidence = "https://example.test/evidence"
      };
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, project);
      IndexModel sut = BuildModel(repository.Object);

      IActionResult result = await sut.OnGetAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.TrustConsultedReligiousBody.Should().Be(YesNoNotApplicableAnswer.No);
      sut.TrustConsultedReligiousBodyNotConsultedReason.Should().Be("Consultation is planned");
      sut.ReligiousBodyRaisedObjection.Should().Be(YesNoNotApplicableAnswer.Yes);
      sut.ReligiousBodyRaisedObjectionReason.Should().Be("Objection details");
      sut.ReligiousBodyConsentGranted.Should().Be(YesNoNotApplicableAnswer.No);
      sut.ReligiousBodyConsentNotGrantedReason.Should().Be("Consent is pending");
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
            && command.ReligiousBodyRaisedObjectionReason == "Cleared"
            && command.ReligiousBodyConsentGranted == YesNoNotApplicableAnswer.NotApplicable
            && command.ReligiousBodyConsentNotGrantedReason == null
            && command.SupportingEvidence == "https://example.test/evidence")), Times.Once);
   }

   [Theory]
   [InlineData(nameof(IndexModel.TrustConsultedReligiousBodyNotConsultedReason), null)]
   [InlineData(nameof(IndexModel.TrustConsultedReligiousBodyNotConsultedReason), " ")]
   [InlineData(nameof(IndexModel.ReligiousBodyRaisedObjectionReason), null)]
   [InlineData(nameof(IndexModel.ReligiousBodyRaisedObjectionReason), " ")]
   [InlineData(nameof(IndexModel.ReligiousBodyConsentNotGrantedReason), null)]
   [InlineData(nameof(IndexModel.ReligiousBodyConsentNotGrantedReason), " ")]
   public async Task OnPostAsync_Requires_details_for_the_triggering_answer(string field, string details)
   {
      const int id = 603;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      IndexModel sut = BuildModel(repository.Object);
      sut.TrustConsultedReligiousBody = YesNoNotApplicableAnswer.No;
      sut.TrustConsultedReligiousBodyNotConsultedReason =
         field == nameof(IndexModel.TrustConsultedReligiousBodyNotConsultedReason) ? details : "Consultation planned";
      sut.ReligiousBodyRaisedObjection = YesNoNotApplicableAnswer.Yes;
      sut.ReligiousBodyRaisedObjectionReason =
         field == nameof(IndexModel.ReligiousBodyRaisedObjectionReason) ? details : "Objection details";
      sut.ReligiousBodyConsentGranted = YesNoNotApplicableAnswer.No;
      sut.ReligiousBodyConsentNotGrantedReason =
         field == nameof(IndexModel.ReligiousBodyConsentNotGrantedReason) ? details : "Consent pending";

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ErrorCount.Should().Be(1);
      sut.ModelState[field]!.Errors.Should().ContainSingle();
      repository.Verify(x => x.SetReligiousBodyConsultation(id, It.IsAny<SetSignificantChangeReligiousBodyConsultationCommand>()), Times.Never);
   }

   [Theory]
   [InlineData(YesNoNotApplicableAnswer.Yes, YesNoNotApplicableAnswer.No, YesNoNotApplicableAnswer.Yes)]
   [InlineData(YesNoNotApplicableAnswer.NotApplicable, YesNoNotApplicableAnswer.NotApplicable, YesNoNotApplicableAnswer.NotApplicable)]
   public async Task OnPostAsync_Does_not_require_or_send_hidden_details(
      YesNoNotApplicableAnswer consulted,
      YesNoNotApplicableAnswer objections,
      YesNoNotApplicableAnswer consent)
   {
      const int id = 604;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      repository.Setup(x => x.SetReligiousBodyConsultation(id, It.IsAny<SetSignificantChangeReligiousBodyConsultationCommand>()))
         .Returns(Task.CompletedTask);
      IndexModel sut = BuildModel(repository.Object);
      sut.TrustConsultedReligiousBody = consulted;
      sut.TrustConsultedReligiousBodyNotConsultedReason = "Discarded";
      sut.ReligiousBodyRaisedObjection = objections;
      sut.ReligiousBodyRaisedObjectionReason = " ";
      sut.ReligiousBodyConsentGranted = consent;
      sut.ReligiousBodyConsentNotGrantedReason = null;

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<RedirectToPageResult>();
      sut.ModelState.IsValid.Should().BeTrue();
      repository.Verify(x => x.SetReligiousBodyConsultation(id,
         It.Is<SetSignificantChangeReligiousBodyConsultationCommand>(command =>
            command.TrustConsultedReligiousBody == consulted
            && command.TrustConsultedReligiousBodyNotConsultedReason == null
            && command.ReligiousBodyRaisedObjection == objections
            && command.ReligiousBodyRaisedObjectionReason == null
            && command.ReligiousBodyConsentGranted == consent
            && command.ReligiousBodyConsentNotGrantedReason == null)), Times.Once);
   }

   [Theory]
   [InlineData(nameof(IndexModel.TrustConsultedReligiousBody))]
   [InlineData(nameof(IndexModel.ReligiousBodyRaisedObjection))]
   [InlineData(nameof(IndexModel.ReligiousBodyConsentGranted))]
   public async Task OnPostAsync_Requires_each_answer(string field)
   {
      const int id = 605;
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(id, BuildProject(id));
      IndexModel sut = BuildModel(repository.Object);
      sut.TrustConsultedReligiousBody = field == nameof(IndexModel.TrustConsultedReligiousBody) ? null : YesNoNotApplicableAnswer.Yes;
      sut.ReligiousBodyRaisedObjection = field == nameof(IndexModel.ReligiousBodyRaisedObjection) ? null : YesNoNotApplicableAnswer.No;
      sut.ReligiousBodyConsentGranted = field == nameof(IndexModel.ReligiousBodyConsentGranted) ? null : YesNoNotApplicableAnswer.Yes;

      IActionResult result = await sut.OnPostAsync(id);

      result.Should().BeOfType<PageResult>();
      sut.ModelState.ErrorCount.Should().Be(1);
      sut.ModelState[field]!.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be("Select an option");
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
