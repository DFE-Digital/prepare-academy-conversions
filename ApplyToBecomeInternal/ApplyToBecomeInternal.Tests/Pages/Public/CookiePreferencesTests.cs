using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ApplyToBecomeInternal.Tests.Pages.Public
{
	public class CookiePreferencesTests : BaseIntegrationTests
	{
		public CookiePreferencesTests(IntegrationTestingWebApplicationFactory factory, ITestOutputHelper outputHelper) : base(factory, outputHelper)
		{
		}

		[Fact]
		public async Task Should_navigate_to_the_cookie_preferences_from_the_link()
		{
			var project = AddGetProject();
			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateDataTestAsync("cookie-preferences");

			Document.Url.Should().Contain("/public/cookie-preferences");
		}

		[Fact]
		public async Task Should_navigate_to_the_cookie_preferences_from_the_first_link_in_the_banner()
		{
			var project = AddGetProject();
			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateDataTestAsync("cookie-banner-link-1");

			Document.Url.Should().Contain("/public/cookie-preferences");
		}

		[Fact]
		public async Task Should_navigate_to_the_cookie_preferences_from_the_second_link_in_the_banner()
		{
			var project = AddGetProject();
			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateDataTestAsync("cookie-banner-link-2");

			Document.Url.Should().Contain("/public/cookie-preferences");
		}

		[Fact]
		public async Task Should_redirect_to_current_page_when_accepting_cookies_from_banner()
		{
			var project = AddGetProject();
			string url = $"/task-list/{project.Id}";
			await OpenUrlAsync(url);

			await NavigateDataTestAsync("cookie-banner-accept");

			Document.Url.Should().Contain(url);
		}

		[Fact]
		public async Task Should_stay_on_the_cookie_page_when_accepting_cookies_on_the_page()
		{
			await OpenUrlAsync("/public/cookie-preferences");

			Document.QuerySelector<IHtmlInputElement>("#cookie-consent-accept").IsChecked = true;
			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().Contain("/public/cookie-preferences");
		}
		
		[Fact]
		public async Task Should_stay_on_the_cookie_page_when_rejecting_cookies_on_the_page()
		{
			await OpenUrlAsync("/public/cookie-preferences");

			Document.QuerySelector<IHtmlInputElement>("#cookie-consent-deny").IsChecked = true;
			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().Contain("/public/cookie-preferences");
		}

		[Fact]
		public async Task Should_show_success_message_when_preferences_set()
		{
			var project = AddGetProject();
			string url = $"/task-list/{project.Id}";
			await OpenUrlAsync(url);

			await NavigateDataTestAsync("cookie-banner-link-2");

			Document.QuerySelector<IHtmlInputElement>("#cookie-consent-deny").IsChecked = true;
			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			var item = Document.QuerySelector("#govuk-notification-banner-title");
			item.InnerHtml.Should().Contain("Success");
			
			var backLink = Document.QuerySelector<IHtmlAnchorElement> ("[data-test='success-banner-return-link']");
			backLink.Href.Should().Contain($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_navigate_back_to_correct_page_using_success_link()
		{
			var project = AddGetProject();
			string url = $"/task-list/{project.Id}";
			await OpenUrlAsync(url);

			await NavigateDataTestAsync("cookie-banner-link-2");

			Document.QuerySelector<IHtmlInputElement>("#cookie-consent-deny").IsChecked = true;
			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			await NavigateDataTestAsync("success-banner-return-link");
			Document.Url.Should().Contain($"/task-list/{project.Id}");
		}

	}
}