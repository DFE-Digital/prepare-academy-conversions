﻿using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AutoFixture;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolAndTrustInformation;

public class AuthorIntegrationTests : BaseIntegrationTests
{
   public AuthorIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Should_navigate_to_and_update_author()
   {
      AcademyConversionProject project = AddGetProject(x =>
      {
         x.AcademyTypeAndRoute = AcademyTypeAndRoutes.Voluntary;
         x.ApplicationReceivedDate = new DateTime(2024, 12, 16, 15, 0, 0, DateTimeKind.Utc);
      });

      UpdateAcademyConversionProject request = AddPatchConfiguredProject(project, x =>
      {
         x.Author = _fixture.Create<string>();
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/conversion-details");
      await NavigateAsync("Change", 3);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/author");
      Document.QuerySelector<IHtmlInputElement>("#author")!.Value.Should().Be(project.Author);

      Document.QuerySelector<IHtmlInputElement>("#author")!.Value = request.Author;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/conversion-details");
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      AcademyConversionProject project = AddGetProject();
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/author");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_navigate_back_to_author()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/author");
      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/conversion-details");
   }
}
