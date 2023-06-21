using AngleSharp.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AcademisationApplication;
using Dfe.PrepareConversions.Data.Models.Application;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Lease = Dfe.PrepareConversions.Data.Models.AcademisationApplication.Lease;
using Loan = Dfe.PrepareConversions.Data.Models.AcademisationApplication.Loan;

namespace Dfe.PrepareConversions.Tests.Pages.ApplicationForm;

public class ApplicationFormIntegrationTests : BaseIntegrationTests
{
   private AcademyConversionProject _project;

   public ApplicationFormIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   private void AddProjectWithFullApplicationForm()
   {
      const int applicationId = 420;
      const int projectId = 421;
      _project = AddGetProject(project =>
      {
         project.Id = projectId;
         project.ApplicationReferenceNumber = $"A2B_{applicationId}";
      });

      var application = AddGetApplication(app =>
      {
         app.ApplicationId = applicationId;
         app.ApplicationReference = _project.ApplicationReferenceNumber;
         app.ApplicationType = "JoinMat";
      });
   }

   [Fact]
   public async Task The_Application_Form_Link_Is_Present()
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync($"/school-application-form/{_project.Id}");

      IElement pageItem = Document.QuerySelector("#application-form-link");
      pageItem.Should().NotBeNull();

      pageItem?.TextContent.Should().Be("Open school application form in a new tab");
      pageItem?.BaseUri.Should().Contain($"school-application-form/{_project.Id}");
   }

   [Theory]
   [InlineData("/school-application-form/{0}")]
   [InlineData("/school-application-form/school-application-tab/{0}")]
   public async Task Should_Display_School_Application_Form_Section(string path)
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "School application form").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
   }

   [Theory]
   [InlineData("/school-application-form/{0}")]
   [InlineData("/school-application-form/school-application-tab/{0}")]
   public async Task Should_Display_About_The_Conversion_Section(string path)
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "About the conversion").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "The school joining the trust").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Contact details").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Date for conversion").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Reasons for joining").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Name changes").Should().NotBeEmpty();
   }

   [Theory]
   [InlineData("/school-application-form/{0}")]
   [InlineData("/school-application-form/school-application-tab/{0}")]
   public async Task Should_Display_Further_Information_Section(string path)
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Further information").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Additional details").Should().NotBeEmpty();
   }

   [Theory]
   [InlineData("/school-application-form/{0}")]
   [InlineData("/school-application-form/school-application-tab/{0}")]
   public async Task Should_Display_Finances_Section_With_Leases_And_Loans(string path)
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Finances").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Previous financial year").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Current financial year").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Next financial year").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Loan 1").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Lease 1").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Financial investigations").Should().NotBeEmpty();
   }

   [Theory]
   [InlineData("/school-application-form/{0}")]
   [InlineData("/school-application-form/school-application-tab/{0}")]
   public async Task Should_Display_Finances_Section_With_No_Leases_Or_Loans(string path)
   {
      _project = AddGetProject();
      AddGetApplication(app =>
      {
         app.ApplicationId = _project.Id;
         app.ApplicationReference = _project.ApplicationReferenceNumber;
         app.ApplicationType = "JoinMat";
         app.Schools.First().Leases = new List<Lease>();
         app.Schools.First().Loans = new List<Loan>();
      });

      await OpenAndConfirmPathAsync(string.Format(path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Finances").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Previous financial year").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Current financial year").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Next financial year").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Loans").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Leases").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Financial investigations").Should().NotBeEmpty();

      IEnumerable<IElement> anyLeases = Document.QuerySelectorAll(".govuk-summary-list__row").Where(contents => contents.InnerHtml.Contains("Are there any existing leases?"));
      anyLeases.First().InnerHtml.Should().Contain("No");
      IEnumerable<IElement> anyLoans = Document.QuerySelectorAll(".govuk-summary-list__row").Where(contents => contents.InnerHtml.Contains("Are there any existing loans?"));
      anyLoans.First().InnerHtml.Should().Contain("No");
   }

   [Theory]
   [InlineData("/school-application-form/{0}")]
   [InlineData("/school-application-form/school-application-tab/{0}")]
   public async Task Should_Display_Future_Pupil_Numbers_Section(string path)
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Future pupil numbers").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
   }

   [Theory]
   [InlineData("/school-application-form/{0}")]
   [InlineData("/school-application-form/school-application-tab/{0}")]
   public async Task Should_Display_Land_And_Buildings_Section(string path)
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Land and buildings").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
   }

   [Theory]
   [InlineData("/school-application-form/{0}")]
   [InlineData("/school-application-form/school-application-tab/{0}")]
   public async Task Should_Display_Pre_Opening_Support_Grant_Section(string path)
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Pre-opening support grant").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
   }

   [Theory]
   [InlineData("/school-application-form/{0}")]
   [InlineData("/school-application-form/school-application-tab/{0}")]
   public async Task Should_Display_Consultation_Section(string path)
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Consultation").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
   }

   [Theory]
   [InlineData("/school-application-form/{0}")]
   [InlineData("/school-application-form/school-application-tab/{0}")]
   public async Task Should_Display_Declaration_Section(string path)
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Declaration").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
   }

   [Theory]
   [InlineData("/school-application-form/{0}")]
   [InlineData("/school-application-form/school-application-tab/{0}")]
   public async Task Should_Show_404Error_When_Application_Not_Found(string path)
   {
      _project = AddGetProject();

      await OpenAndConfirmPathAsync(string.Format(path, _project.Id));

      IElement errorHeading = Document.QuerySelector("#error-heading");
      errorHeading.Should().NotBeNull();

      errorHeading?.TextContent.Should().Contain("not found");
   }

   [Theory]
   [InlineData("/school-application-form/{0}")]
   [InlineData("/school-application-form/school-application-tab/{0}")]
   public async Task Should_Show_501Error_When_Application_Is_Not_Join_Mat(string path)
   {
      _project = AddGetProject();
      AddGetApplication(app =>
      {
         app.ApplicationId = _project.Id;
         app.ApplicationReference = _project.ApplicationReferenceNumber;
         app.ApplicationType = "FormMat";
      });

      await OpenAndConfirmPathAsync(string.Format(path, _project.Id));

      IElement errorHeading = Document.QuerySelector("#error-heading");
      errorHeading.Should().NotBeNull();

      errorHeading?.TextContent.Should().Contain("Not implemented");
   }

   [Theory]
   [InlineData("/school-application-form/{0}")]
   [InlineData("/school-application-form/school-application-tab/{0}")]
   public async Task Should_Show_404Error_When_Application_Is_Not_Found(string path)
   {
      _project = AddGetProject();
      AddGetApplication(app =>
      {
         app.ApplicationId = _project.Id;
         app.ApplicationReference = "Example";
         app.ApplicationType = "JoinMat";
         app.Schools = new List<School>();
      });

      await OpenAndConfirmPathAsync(string.Format(path, _project.Id));

      IElement errorHeading = Document.QuerySelector("#error-heading");
      errorHeading.Should().NotBeNull();

      errorHeading?.TextContent.Should().Contain("Page not found");
   }

   [Theory]
   [InlineData("/school-application-form/{0}")]
   [InlineData("/school-application-form/school-application-tab/{0}")]
   public async Task Should_Contain_Contents_With_Links_To_Correct_Sections(string path)
   {
      // Arrange
      Dictionary<string, string> headings = new()
      {
         { "Overview", "#Overview" },
         { "About the conversion", "#About_the_conversion" },
         { "Further information", "#Further_information" },
         { "Finances", "#Finances" },
         { "Future pupil numbers", "#Future_pupil_numbers" },
         { "Land and buildings", "#Land_and_buildings" },
         { "Pre-opening support grant", "#Pre-opening_support_grant" },
         { "Consultation", "#Consultation" },
         { "Declaration", "#Declaration" }
      };

      AddProjectWithFullApplicationForm();

      // Act
      await OpenAndConfirmPathAsync(string.Format(path, _project.Id));

      foreach ((string sectionHeading, string id) in headings)
      {
         // Assert
         // check the link text and href
         IElement sectionLink = Document.QuerySelector($"{id}_link");
         sectionLink?.TextContent.Should().Be(sectionHeading);

         string linkAttributeValue = sectionLink?.Attributes.First(a => a.Name == "href").Value;

         linkAttributeValue.Should().Be(id).And.NotBeNullOrWhiteSpace();

         string expectedLinkName = linkAttributeValue?.Replace("#", string.Empty);
         expectedLinkName.Should().NotBeNullOrWhiteSpace();

         // check the link is associated with the correct section header
         Document
            .QuerySelectorAll("h2")
            .First(contents => contents.InnerHtml == sectionHeading).Attributes
            .First(a => a.Name == "id").Value
            .Should().Be(expectedLinkName);
      }
   }
}
