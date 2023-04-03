using AngleSharp.Dom;
using Dfe.PrepareConversions.Data.Models;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.AnnexB;

public class WhenViewingTheAnnexBTab : BaseIntegrationTests
{
   public WhenViewingTheAnnexBTab(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   private IElement FormReturned => Document.QuerySelector(CypressSelectorFor("select-annex-b-form-returned"));
   private IElement FormReturnedAnswer => Document.QuerySelector(CypressSelectorFor("select-annex-b-form-returned-answer"));
   private IElement FormReturnedChange => Document.QuerySelector(CypressSelectorFor("select-annex-b-form-returned-change"));

   private IElement SharepointLink => Document.QuerySelector(CypressSelectorFor("select-annex-b-sharepoint-link"));
   private IElement SharepointUrl => Document.QuerySelector(CypressSelectorFor("select-annex-b-sharepoint-url"));
   private IElement SharepointChange => Document.QuerySelector(CypressSelectorFor("select-annex-b-sharepoint-change"));

   private static string AnnexBFormPathFor(AcademyConversionProject project)
   {
      return $"/task-list/{project.Id}/annex-b-form-saved";
   }

   [Fact]
   public async Task Should_display_the_form_received_prompt()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync(AnnexBFormPathFor(project));

      FormReturned.Text().Should().Contain("received a completed Annex B form");
   }

   [Fact]
   public async Task Should_display_the_empty_placeholder()
   {
      AcademyConversionProject project = AddGetProject(x =>
      {
         x.AnnexBFormReceived = default;
         x.AnnexBFormUrl = default;
      });

      await OpenAndConfirmPathAsync(AnnexBFormPathFor(project));

      FormReturnedAnswer.Text().Trim().Should().Be("Empty");
   }

   [Fact]
   public async Task Should_not_display_the_form_url_field_when_not_supplied()
   {
      AcademyConversionProject project = AddGetProject(x =>
      {
         x.AnnexBFormReceived = false;
         x.AnnexBFormUrl = default;
      });

      await OpenAndConfirmPathAsync(AnnexBFormPathFor(project));

      SharepointLink.Should().BeNull();
   }

   [Fact]
   public async Task Should_display_no_if_the_form_has_not_been_provided()
   {
      AcademyConversionProject project = AddGetProject(x =>
      {
         x.AnnexBFormReceived = false;
         x.AnnexBFormUrl = default;
      });

      await OpenAndConfirmPathAsync(AnnexBFormPathFor(project));

      FormReturnedAnswer.Text().Trim().Should().Be("No");
   }

   [Fact]
   public async Task Should_display_yes_and_the_url_if_the_form_url_has_been_provided()
   {
      AcademyConversionProject project = AddGetProject(x =>
      {
         x.AnnexBFormReceived = true;
         x.AnnexBFormUrl = "https://example.org";
      });

      await OpenAndConfirmPathAsync(AnnexBFormPathFor(project));

      FormReturnedAnswer.Text().Trim().Should().Be("Yes");
      SharepointUrl.Text().Trim().Should().Be(project.AnnexBFormUrl);
   }

   [Fact]
   public async Task Should_provide_a_change_link_that_takes_the_user_to_the_edit_page()
   {
      AcademyConversionProject project = AddGetProject(x =>
      {
         x.AnnexBFormReceived = true;
         x.AnnexBFormUrl = "https://example.org";
      });

      await OpenAndConfirmPathAsync(AnnexBFormPathFor(project));

      FormReturnedChange.Text().Trim().Should().Contain("Change");
      SharepointChange.Text().Trim().Should().Contain("Change");
   }
}
