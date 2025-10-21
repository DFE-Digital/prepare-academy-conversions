﻿using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolPerformance;

public class ConfirmSchoolPerformanceIntegrationTests : BaseIntegrationTests
{
   public ConfirmSchoolPerformanceIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Should_be_reference_only_and_display_school_performance()
   {
      AcademyConversionProject project = AddGetProject();
      EstablishmentDto schoolPerformance = AddGetEstablishmentDto(project.Urn.ToString());

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#school-performance-status")?.TextContent.Trim().Should().Be("Reference only");
      Document.QuerySelector("#school-performance-status")?.ClassName.Should().Contain("grey");

      await NavigateAsync("Ofsted information");

      Document.QuerySelector("#additional-information")?.TextContent.Should().Be(project.SchoolPerformanceAdditionalInformation);

      Document.QuerySelector("#overall-effectiveness")?.TextContent.Should()
         .Be($"Last full inspection: {schoolPerformance.MISEstablishment.OverallEffectiveness.DisplayOfstedRating()}");
      Document.QuerySelector("#quality-of-education")?.TextContent.Should().Be(schoolPerformance.MISEstablishment.QualityOfEducation.DisplayOfstedRating());
      Document.QuerySelector("#leadership-and-management")?.TextContent.Should()
         .Be(schoolPerformance.MISEstablishment.EffectivenessOfLeadershipAndManagement.DisplayOfstedRating());
      Document.QuerySelector("#behaviour-and-attitudes")?.TextContent.Should().Be(schoolPerformance.MISEstablishment.BehaviourAndAttitudes.DisplayOfstedRating());
      Document.QuerySelector("#personal-development")?.TextContent.Should().Be(schoolPerformance.MISEstablishment.PersonalDevelopment.DisplayOfstedRating());
      Document.QuerySelector("#early-years-provision")?.TextContent.Should().Be(schoolPerformance.MISEstablishment.EarlyYearsProvision.DisplayOfstedRating());
      Document.QuerySelector("#sixth-form-provision")?.TextContent.Should().Be(schoolPerformance.MISEstablishment.SixthFormProvision.DisplayOfstedRating());
   }

   [Fact]
   public async Task Should_navigate_between_task_list_and_confirm_school_performance()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Ofsted information");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-performance-ofsted-information");

      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_display_null_values_appropriately()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("Ofsted information");

      Document.QuerySelector("#ofsted-full-inspection-date")?.TextContent.Should().Be("No data");
      Document.QuerySelector("#overall-effectiveness")?.TextContent.Should().Be("Last full inspection: No data");
      Document.QuerySelector("#quality-of-education")?.TextContent.Should().Be("No data");
      Document.QuerySelector("#leadership-and-management")?.TextContent.Should().Be("No data");
      Document.QuerySelector("#behaviour-and-attitudes")?.TextContent.Should().Be("No data");
      Document.QuerySelector("#personal-development")?.TextContent.Should().Be("No data");
   }
}