using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.AdmissionVariationConsultation;

public class AdmissionVariationConsultationIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_admission_variation_consultation_page_with_existing_values()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 521);
      project.AdmissionVariationConsultation.ConsultationIncludeAdmissionVariation = false;
      project.AdmissionVariationConsultation.ConsultationNoAdmissionVariationReason = "The consultation focused on governance and implementation approach";

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/admission-variation-consultation");

      Document.QuerySelector<IHtmlHeadingElement>("h1")!.TextContent.Trim().Should().Be("Admission variation consultation");
      Document.QuerySelector<IHtmlInputElement>("#consultation-include-admision-variation-no")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='consultation-no-admission-variation-reason']")!.Value
         .Should().Be("The consultation focused on governance and implementation approach");
   }

   [Fact]
   public async Task Should_save_yes_answer_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 522);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetAdmissionVariationConsultation, project.Id),
         new SetSignificantChangeAdmissionVariationConsultationCommand(true, null),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/admission-variation-consultation");

      Document.QuerySelector<IHtmlInputElement>("#consultation-include-admision-variation-yes")!.IsChecked = true;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_save_no_answer_with_reason_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 523);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      const string reason = "The trust intends to run admissions variation consultation after this phase";
      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetAdmissionVariationConsultation, project.Id),
         new SetSignificantChangeAdmissionVariationConsultationCommand(false, reason),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/admission-variation-consultation");

      Document.QuerySelector<IHtmlInputElement>("#consultation-include-admision-variation-no")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='consultation-no-admission-variation-reason']")!.Value = reason;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_show_validation_error_when_no_is_selected_without_reason()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 524);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/admission-variation-consultation");

      Document.QuerySelector<IHtmlInputElement>("#consultation-include-admision-variation-no")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='consultation-no-admission-variation-reason']")!.Value = "";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}/admission-variation-consultation");
      Document.QuerySelector<IHtmlElement>("#ConsultationNoAdmissionVariationReason-error")!
         .TextContent.Should().Contain("Add a reason");
   }

   [Fact]
   public async Task Should_show_validation_error_when_no_option_is_selected()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 525);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/admission-variation-consultation");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}/admission-variation-consultation");
      Document.QuerySelector<IHtmlElement>("#ConsultationIncludeAdmissionVariation-error")!
         .TextContent.Should().Contain("Select an option");
   }

   private static SignificantChangeProjectResponse BuildProject(int id)
   {
      return new SignificantChangeProjectResponse
      {
         Id = id,
         Urn = 10000000 + id,
         SchoolName = "Significant change school",
         Tier = 1,
         TrustName = "Example Trust",
         TrustUkprn = "12345678",
         AssignedUser = new User("user-id", "assigned.user@test.local", "Assigned User"),
         TypeOfSignificantChange = "Route A",
         Status = "pre decision",
         AdmissionVariationConsultation = new SignificantChangeAdmissionVariationConsultationResponse()
      };
   }
}
