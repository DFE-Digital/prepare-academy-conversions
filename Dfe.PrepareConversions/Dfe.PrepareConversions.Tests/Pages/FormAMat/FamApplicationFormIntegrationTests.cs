using AngleSharp.Dom;
using Dfe.PrepareConversions.Data.Features;
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

namespace Dfe.PrepareConversions.Tests.Pages.FormAMat;

public class FamApplicationFormIntegrationTests : BaseIntegrationTests
{
   private AcademyConversionProject _project;
   private static readonly string Path = "/form-a-mat/{0}";

   public FamApplicationFormIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   private void AddProjectWithFullApplicationForm()
   {
      _project = AddGetProject();

      AddGetApplication(app =>
      {
         app.ApplicationId = _project.Id;
         app.ApplicationReference = _project.ApplicationReferenceNumber;
         app.Schools.FirstOrDefault()!.SchoolName = _project.SchoolName;
         app.ApplicationType = GlobalStrings.FormAMat;
      });
   }

   [Fact]
   public async Task The_Fam_Application_Form_Link_Is_Not_Present()
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync($"/form-a-mat/{_project.Id}");

      IElement pageItem = Document.QuerySelector("#application-form-link");
      pageItem.Should().BeNull();
   }

   [Fact]
   public async Task Should_Display_School_Application_Form_Section()
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(Path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "School application form").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
   }

   [Fact]
   public async Task Should_Display_About_The_Conversion_Section()
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(Path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "About the conversion").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "The school joining the trust").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Contact details").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Date for conversion").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Reasons for joining").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Name changes").Should().NotBeEmpty();
   }

   [Fact]
   public async Task Should_Display_Further_Information_Section()
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(Path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Further information").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Additional details").Should().NotBeEmpty();
   }

   [Fact]
   public async Task Should_Display_Finances_Section_With_Leases_And_Loans()
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(Path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Finances").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Previous financial year").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Current financial year").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Next financial year").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Loan 1").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Lease 1").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Financial investigations").Should().NotBeEmpty();

   }

   [Fact]
   public async Task Should_Display_Finances_Section_With_No_Leases_Or_Loans()
   {
      _project = AddGetProject();
      AddGetApplication(app =>
      {
         app.ApplicationId = _project.Id;
         app.ApplicationReference = _project.ApplicationReferenceNumber;
         app.ApplicationType = GlobalStrings.FormAMat;
         app.Schools.First().SchoolName = _project.SchoolName;
         app.Schools.First().Leases = new List<Lease>();
         app.Schools.First().Loans = new List<Loan>();
      });

      await OpenAndConfirmPathAsync(string.Format(Path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Finances").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Previous financial year").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Current financial year").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Next financial year").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Loans").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Leases").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Financial investigations").Should().NotBeEmpty();

      IElement anyLeases = Document.QuerySelector("[test-id='Finances_Leases1_value']");
      anyLeases.InnerHtml.Should().Contain("No");
      IElement anyLoans = Document.QuerySelector("[test-id='Finances_Loans1_value']");
      anyLoans.InnerHtml.Should().Contain("No");
   }

   [Fact]
   public async Task Should_Display_Future_Pupil_Numbers_Section()
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(Path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Future pupil numbers").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
   }

   [Fact]
   public async Task Should_Display_Land_And_Buildings_Section()
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(Path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Land and buildings").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
   }

   [Fact]
   public async Task Should_Display_Pre_Opening_Support_Grant_Section()
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(Path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Pre-opening support grant").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
   }
   [Fact]
   public async Task Should_Display_Trust_Information_Section()
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(Path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Trust information").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Opening date").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Reasons for forming the trust").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Plans for growth").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "School improvement strategy").Should().NotBeEmpty();
   }

   [Fact]
   public async Task Should_Display_Consultation_Section()
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(Path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Consultation").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
   }

   [Fact]
   public async Task Should_Display_Declaration_Section()
   {
      AddProjectWithFullApplicationForm();

      await OpenAndConfirmPathAsync(string.Format(Path, _project.Id));

      Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Declaration").Should().NotBeEmpty();
      Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
   }

   [Fact]
   public async Task Should_Show_404Error_When_Application_Not_Found()
   {
      _project = AddGetProject();

      await OpenAndConfirmPathAsync(string.Format(Path, _project.Id));

      IElement errorHeading = Document.QuerySelector("#error-heading");
      errorHeading.Should().NotBeNull();

      errorHeading?.TextContent.Should().Contain("not found");
   }

   [Fact]
   public async Task Should_Show_Page_Not_Found_When_Application_Is_Not_Valid()
   {
      _project = AddGetProject();
      AddGetApplication(app =>
      {
         app.ApplicationId = 999;
         app.ApplicationType = GlobalStrings.FormAMat;
         app.Schools = new List<School>();
      });

      await OpenAndConfirmPathAsync(string.Format(Path, _project.Id));

      IElement errorHeading = Document.QuerySelector("#error-heading");
      errorHeading.Should().NotBeNull();

      errorHeading?.TextContent.Should().Contain("Page not found");
   }

   [Fact]
   public async Task Should_Contain_Contents_With_Links_To_Correct_Sections()
   {
      // Arrange
      Dictionary<string, string> headings = new()
      {
         { "Overview", "#Overview" },
         { "Trust information", "#Trust_information" },
         { "Key people within the trust", "#Key_people_within_the_trust"},
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
      await OpenAndConfirmPathAsync(string.Format(Path, _project.Id));

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
