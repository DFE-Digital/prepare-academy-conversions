using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.TagHelpers
{
	[HtmlTargetElement("govuk-summary-list-row", TagStructure = TagStructure.WithoutEndTag)]
	public class SummaryListRowTagHelper : InputTagHelperBase
	{
		[HtmlAttributeName("value")]
		public string Value { get; set; }

		[HtmlAttributeName("asp-page")]
		public string Page { get; set; }

		[HtmlAttributeName("asp-fragment")]
		public string Fragment { get; set; }

		[HtmlAttributeName("asp-route-id")]
		public string RouteId { get; set; }

		[HtmlAttributeName("hidden-text")]
		public string HiddenText { get; set; }

		[HtmlAttributeName("key-width")]
		public string KeyWidth { get; set; }

		[HtmlAttributeName("value-width")]
		public string ValueWidth { get; set; }

		public SummaryListRowTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

		protected override async Task<IHtmlContent> RenderContentAsync()
		{
			var value = For == null ? Value : For.Model?.ToString();

			var model = new SummaryListRowViewModel
			{
				Id = Id,
				Key = Label,
				Value = value,
				Page = Page,
				Fragment = Fragment,
				RouteId = RouteId,
				Return = ViewContext.ViewData["Return"]?.ToString(),
				HiddenText = HiddenText,
				KeyWidth = KeyWidth,
				ValueWidth = ValueWidth
			};

			return await _htmlHelper.PartialAsync("_SummaryListRow", model);
		}
	}
}
