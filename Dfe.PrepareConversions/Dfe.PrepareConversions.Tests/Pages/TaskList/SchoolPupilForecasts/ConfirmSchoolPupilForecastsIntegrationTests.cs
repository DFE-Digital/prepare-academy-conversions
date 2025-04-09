using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AutoFixture;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using DfE.CoreLibs.Contracts.Academies.V4.Establishments;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using static Dfe.PrepareConversions.Extensions.IntegerExtensions;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolPupilForecasts;

public class ConfirmSchoolPupilForecastsIntegrationTests : BaseIntegrationTests
{
   public ConfirmSchoolPupilForecastsIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Should_be_reference_only_and_display_school_pupil_forecasts_table()
   {
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.YearOneProjectedCapacity = _fixture.Create<int>();
         p.YearOneProjectedPupilNumbers = _fixture.Create<int>();
         p.YearTwoProjectedCapacity = _fixture.Create<int>();
         p.YearTwoProjectedPupilNumbers = _fixture.Create<int>();
         p.YearThreeProjectedCapacity = _fixture.Create<int>();
         p.YearThreeProjectedPupilNumbers = _fixture.Create<int>();
      });
      EstablishmentDto establishment = AddGetEstablishmentDto(project.Urn.ToString());

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#school-pupil-forecasts-status")?.TextContent.Trim().Should().Be("Reference only");
      Document.QuerySelector("#school-pupil-forecasts-status")?.ClassName.Should().Contain("grey");

      await NavigateAsync("Pupil forecasts");

      Document.QuerySelector("#additional-information")?.TextContent.Should().Be(project.SchoolPupilForecastsAdditionalInformation);

      IHtmlCollection<IElement> rows = Document.QuerySelectorAll("tbody tr");
      rows[0].Children[1].TextContent.Should().Be(establishment.SchoolCapacity);
      rows[0].Children[2].TextContent.Should().Be(establishment.Census.NumberOfPupils);
      rows[0].Children[3].TextContent.Should().Be(ToInt(establishment.Census?.NumberOfPupils).AsPercentageOf(ToInt(establishment.SchoolCapacity)));
      rows[1].Children[1].TextContent.Should().Be(project.YearOneProjectedCapacity.ToString());
      rows[1].Children[2].TextContent.Should().Be(project.YearOneProjectedPupilNumbers.ToString());
      rows[1].Children[3].TextContent.Should().Be(project.YearOneProjectedPupilNumbers.AsPercentageOf(project.YearOneProjectedCapacity));
      rows[2].Children[1].TextContent.Should().Be(project.YearTwoProjectedCapacity.ToString());
      rows[2].Children[2].TextContent.Should().Be(project.YearTwoProjectedPupilNumbers.ToString());
      rows[2].Children[3].TextContent.Should().Be(project.YearTwoProjectedPupilNumbers.AsPercentageOf(project.YearTwoProjectedCapacity));
      rows[3].Children[1].TextContent.Should().Be(project.YearThreeProjectedCapacity.ToString());
      rows[3].Children[2].TextContent.Should().Be(project.YearThreeProjectedPupilNumbers.ToString());
      rows[3].Children[3].TextContent.Should().Be(project.YearThreeProjectedPupilNumbers.AsPercentageOf(project.YearThreeProjectedCapacity));
   }

   [Fact]
   public async Task Should_display_school_pupil_forecasts_table_when_values_are_null()
   {
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.YearOneProjectedCapacity = null;
         p.YearOneProjectedPupilNumbers = null;
         p.YearTwoProjectedCapacity = null;
         p.YearTwoProjectedPupilNumbers = null;
         p.YearThreeProjectedCapacity = null;
         p.YearThreeProjectedPupilNumbers = null;
      });
      AddGetEstablishmentDto(project.Urn.ToString(), true);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#school-pupil-forecasts-status")?.TextContent.Trim().Should().Be("Reference only");
      Document.QuerySelector("#school-pupil-forecasts-status")?.ClassName.Should().Contain("grey");

      await NavigateAsync("Pupil forecasts");

      Document.QuerySelector("#additional-information")?.TextContent.Should().Be(project.SchoolPupilForecastsAdditionalInformation);

      IHtmlCollection<IElement> rows = Document.QuerySelectorAll("tbody tr");
      rows[0].Children[1].TextContent.Should().Be("");
      rows[0].Children[2].TextContent.Should().Be("");
      rows[0].Children[3].TextContent.Should().Be("");
      rows[1].Children[1].TextContent.Should().Be(project.YearOneProjectedCapacity.ToString());
      rows[1].Children[2].TextContent.Should().Be(project.YearOneProjectedPupilNumbers.ToString());
      rows[1].Children[3].TextContent.Should().Be("");
      rows[2].Children[1].TextContent.Should().Be(project.YearTwoProjectedCapacity.ToString());
      rows[2].Children[2].TextContent.Should().Be(project.YearTwoProjectedPupilNumbers.ToString());
      rows[2].Children[3].TextContent.Should().Be("");
      rows[3].Children[1].TextContent.Should().Be(project.YearThreeProjectedCapacity.ToString());
      rows[3].Children[2].TextContent.Should().Be(project.YearThreeProjectedPupilNumbers.ToString());
      rows[3].Children[3].TextContent.Should().Be("");
   }

   [Fact]
   public async Task Should_navigate_between_task_list_and_confirm_school_pupil_forecasts()
   {
      AcademyConversionProject project = AddGetProject();
      AddGetEstablishmentDto(project.Urn.ToString());

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Pupil forecasts");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/pupil-forecasts");

      await Document.QuerySelector<IHtmlFormElement>("form")?.SubmitAsync()!;
      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Back_link_should_navigate_from_confirm_school_pupil_forecasts_to_task_list()
   {
      AcademyConversionProject project = AddGetProject();
      AddGetEstablishmentDto(project.Urn.ToString());

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Pupil forecasts");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/pupil-forecasts");

      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Theory]
   [InlineData("change-school-pupil-forecasts-additional-information")]
   public async Task Should_not_have_change_link_if_project_read_only(params string[] elements)
   {
      var project = AddGetProject(isReadOnly: true);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Pupil forecasts");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/pupil-forecasts");
      foreach (var element in elements)
      {
         VerifyElementDoesNotExist(element);
      }
      Document.QuerySelector("#confirm-and-continue-button").Should().BeNull();
   }
}
