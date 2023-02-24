using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SchoolAndTrustInformation;

	public class ConfirmSchoolAndTrustInformationIntegrationTests : BaseIntegrationTests
	{
		public ConfirmSchoolAndTrustInformationIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_be_in_progress_and_display_school_and_trust_information()
		{
      AcademyConversionProject project = AddGetProject(p => p.SchoolAndTrustInformationSectionComplete = false);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#school-and-trust-information-status").TextContent.Trim().Should().Be("In Progress");
			Document.QuerySelector("#school-and-trust-information-status").ClassName.Should().Contain("blue");

			await NavigateAsync("School and trust information and project dates");

			Document.QuerySelector("#project-recommendation").TextContent.Should().Be(project.RecommendationForProject);
			Document.QuerySelector("#author").TextContent.Should().Be(project.Author);
			Document.QuerySelector("#cleared-by").TextContent.Should().Be(project.ClearedBy);
			Document.QuerySelector("#academy-order-required").TextContent.Should().Be(project.AcademyOrderRequired);
			Document.QuerySelector("#advisory-board-date").TextContent.Should().Be(project.HeadTeacherBoardDate.ToDateString());
			Document.QuerySelector("#previous-advisory-board").TextContent.Should().Be(project.PreviousHeadTeacherBoardDate.ToDateString());
			Document.QuerySelector("#school-name").TextContent.Should().Be(project.SchoolName);
			Document.QuerySelector("#unique-reference-number").TextContent.Should().Be(project.Urn.ToString());
			Document.QuerySelector("#local-authority").TextContent.Should().Be(project.LocalAuthority);
			Document.QuerySelector("#trust-reference-number").TextContent.Should().Be(project.TrustReferenceNumber);
			Document.QuerySelector("#name-of-trust").TextContent.Should().Be(project.NameOfTrust);
			Document.QuerySelector("#sponsor-reference-number").TextContent.Should().Be(project.SponsorReferenceNumber);
			Document.QuerySelector("#sponsor-name").TextContent.Should().Be(project.SponsorName);
			Document.QuerySelector("#academy-type-and-route").TextContent.Should().Contain(project.AcademyTypeAndRoute);
			Document.QuerySelector("#academy-type-and-route").TextContent.Should().Contain(project.ConversionSupportGrantAmount.Value.ToMoneyString(true));
			Document.QuerySelector("#academy-type-and-route-additional-text").TextContent.Should().Contain(project.ConversionSupportGrantChangeReason);
			Document.QuerySelector("#proposed-academy-opening-date").TextContent.Should().Be(project.ProposedAcademyOpeningDate.ToDateString(true));
		}

		[Fact]
		public async Task Previous_head_teacher_board_date_should_be_no_when_question_field_set_to_no()
		{
      AcademyConversionProject project = AddGetProject(p => p.PreviousHeadTeacherBoardDateQuestion = "No");

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
			await NavigateAsync("School and trust information and project dates");

			Document.QuerySelector("#previous-advisory-board").TextContent.Should().Be("No");
		}

		[Fact]
		public async Task Should_display_an_error_when_school_and_trust_information_is_marked_as_complete_without_advisory_board_date_set()
		{
      AcademyConversionProject project = AddGetProject(project =>
			{
				project.SchoolAndTrustInformationSectionComplete = false;
				project.HeadTeacherBoardDate = null;
			});
			
			AddPatchProject(project, r => r.SchoolAndTrustInformationSectionComplete, true);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

			await NavigateAsync("School and trust information and project dates");

			Document.QuerySelector<IHtmlInputElement>("#school-and-trust-information-complete").DoClick();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
			Document.QuerySelector(".govuk-error-summary").Should().NotBe(null);
		}

		[Fact]
		public async Task Should_be_completed_and_checked_when_school_and_trust_information_complete()
		{
      AcademyConversionProject project = AddGetProject(project => project.SchoolAndTrustInformationSectionComplete = true);
         AddPatchConfiguredProject(project, x =>
         {
            x.SchoolAndTrustInformationSectionComplete = true;
            x.Urn = project.Urn;
         });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#school-and-trust-information-status").TextContent.Trim().Should().Be("Completed");

			await NavigateAsync("School and trust information and project dates");

			Document.QuerySelector<IHtmlInputElement>("#school-and-trust-information-complete").IsChecked.Should().BeTrue();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_be_not_started_and_display_empty_when_school_and_trust_information_not_prepopulated()
		{
      AcademyConversionProject project = AddGetProject(project =>
			{
				project.RecommendationForProject = null;
				project.Author = null;
				project.ClearedBy = null;
				project.AcademyOrderRequired = null;
				project.HeadTeacherBoardDate = null;
				project.PreviousHeadTeacherBoardDate = null;
				project.SchoolName = null;
				project.Urn = null;
				project.LocalAuthority = null;
				project.TrustReferenceNumber = null;
				project.NameOfTrust = null;
				project.SponsorReferenceNumber = null;
				project.SponsorName = null;
				project.AcademyTypeAndRoute = null;
            project.ConversionSupportGrantAmount = null;
            project.ConversionSupportGrantChangeReason = null;
				project.ProposedAcademyOpeningDate = null;
				project.SchoolAndTrustInformationSectionComplete = false;
			});
			AddPatchProject(project, r => r.SchoolAndTrustInformationSectionComplete, false);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#school-and-trust-information-status").TextContent.Trim().Should().Be("Not Started");
			Document.QuerySelector("#school-and-trust-information-status").ClassName.Should().Contain("grey");

			await NavigateAsync("School and trust information and project dates");

			Document.QuerySelector("#project-recommendation").TextContent.Should().Be("Empty");
			Document.QuerySelector("#author").TextContent.Should().Be("Empty");
			Document.QuerySelector("#cleared-by").TextContent.Should().Be("Empty");
			Document.QuerySelector("#academy-order-required").TextContent.Should().Be("Empty");
			Document.QuerySelector("#advisory-board-date").TextContent.Should().Be("Empty");
			Document.QuerySelector("#previous-advisory-board").TextContent.Should().Be("Empty");
			Document.QuerySelector("#school-name").TextContent.Should().Be("Empty");
			Document.QuerySelector("#unique-reference-number").TextContent.Should().Be("Empty");
			Document.QuerySelector("#local-authority").TextContent.Should().Be("Empty");
			Document.QuerySelector("#trust-reference-number").TextContent.Should().Be("Empty");
			Document.QuerySelector("#name-of-trust").TextContent.Should().Be("Empty");
			Document.QuerySelector("#sponsor-reference-number").TextContent.Should().Be("Not applicable");
			Document.QuerySelector("#sponsor-name").TextContent.Should().Be("Not applicable");
			Document.QuerySelector("#academy-type-and-route").TextContent.Should().Be("Empty");
			Document.QuerySelector("#proposed-academy-opening-date").TextContent.Should().Be("Empty");
			Document.QuerySelector<IHtmlInputElement>("#school-and-trust-information-complete").IsChecked.Should().BeFalse();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
      AcademyConversionProject project = AddGetProject();
			AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_show_error_summary_when_grant_amount_less_than_full_amount_and_no_reason_entered()
		{
      AcademyConversionProject project = AddGetProject(project =>
			{
				project.ConversionSupportGrantAmount = 2000m;
				project.ConversionSupportGrantChangeReason = null;
			});
			AddPatchProjectMany(project, composer =>
				composer
				.With(r => r.ConversionSupportGrantAmount, project.ConversionSupportGrantAmount)
				.With(r => r.ConversionSupportGrantChangeReason, String.Empty));

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

			await NavigateAsync("School and trust information and project dates");
			await NavigateAsync("Change", 2);

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/route-and-grant");
      IElement test = Document.QuerySelector(".govuk-error-summary");
			test.Should().NotBe(null);
		}

		[Fact]
		public async Task Should_navigate_between_task_list_and_school_and_trust_information()
		{
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
			await NavigateAsync("School and trust information and project dates");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");

			await NavigateAsync("Back to task list");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}
	}
}
