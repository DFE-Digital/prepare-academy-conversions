using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.Public;

public class CookiePreferencesIntegrationTests(IntegrationTestingWebApplicationFactory factory) : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_navigate_to_the_cookie_preferences_from_the_link()
   {
      var project = AddGetProject();
      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateDataTestAsync("cookie-preferences");

      Document.Url.Should().Contain("/public/cookie-preferences");
   }

   [Fact]
   public async Task Should_navigate_to_the_cookie_preferences_from_the_first_link_in_the_banner()
   {
      var project = AddGetProject();
      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateDataTestAsync("cookie-banner-link-1");

      Document.Url.Should().Contain("/public/cookie-preferences");
   }

   [Fact]
   public async Task Should_navigate_to_the_cookie_preferences_from_the_second_link_in_the_banner()
   {
      var project = AddGetProject();
      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateDataTestAsync("cookie-banner-link-2");

      Document.Url.Should().Contain("/public/cookie-preferences");
   }

   [Fact]
   public async Task Should_redirect_to_current_page_when_accepting_cookies_from_banner()
   {
      var project = AddGetProject();
      string url = $"/task-list/{project.Id}";
      await OpenAndConfirmPathAsync(url);

      var item = GetDataTestButtonElement("cookie-banner-accept");
      item!.InnerHtml.Should().Contain("Accept analytics cookies");

      NavigateDataTestButton("hide-cookie-banner-accept");
      
      Document.Url.Should().Contain(url);
   }

   [Fact]
   public async Task Should_stay_on_the_cookie_page_when_accepting_cookies_on_the_page()
   {
      await OpenAndConfirmPathAsync("/public/cookie-preferences");

      Document.QuerySelector<IHtmlInputElement>("#cookie-consent-accept")!.IsChecked = true;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().Contain("/public/cookie-preferences");
   }

   [Fact]
   public async Task Should_stay_on_cookie_page_to_page_accepting_cookies_on_banner()
   {
      await OpenAndConfirmPathAsync("/public/cookie-preferences");

      var item = GetDataTestButtonElement("cookie-banner-accept");
      item!.InnerHtml.Should().Contain("Accept analytics cookies");

      NavigateDataTestButton("hide-cookie-banner-accept");

      Document.Url.Should().Contain("/public/cookie-preferences");
   }

   [Fact]
   public async Task Should_stay_on_the_cookie_page_when_rejecting_cookies_on_the_page()
   {
      await OpenAndConfirmPathAsync("/public/cookie-preferences");

      Document.QuerySelector<IHtmlInputElement>("#cookie-consent-deny")!.IsChecked = true;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().Contain("/public/cookie-preferences");
   }

   [Fact]
   public async Task Should_show_success_message_when_preferences_set()
   {
      var project = AddGetProject();
      string url = $"/task-list/{project.Id}";
      await OpenAndConfirmPathAsync(url);

      await NavigateDataTestAsync("cookie-banner-link-2");

      Document.QuerySelector<IHtmlInputElement>("#cookie-consent-deny")!.IsChecked = true;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      IElement item = Document.QuerySelector("#govuk-notification-banner-title");
      item!.InnerHtml.Should().Contain("Success");

      IHtmlAnchorElement backLink = Document.QuerySelector<IHtmlAnchorElement>("[data-test='success-banner-return-link']");
      backLink!.Href.Should().Contain($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_navigate_back_to_correct_page_using_success_link()
   {
      var project = AddGetProject();
      string url = $"/task-list/{project.Id}";
      await OpenAndConfirmPathAsync(url);

      await NavigateDataTestAsync("cookie-banner-link-2");

      Document.QuerySelector<IHtmlInputElement>("#cookie-consent-deny")!.IsChecked = true;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      await NavigateDataTestAsync("success-banner-return-link");
      Document.Url.Should().Contain($"/task-list/{project.Id}");
   }
}
