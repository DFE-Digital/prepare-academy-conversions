using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ApplyToBecomeInternal.TagHelpers
{
	[HtmlTargetElement("a", Attributes = "back-link-for")]
	public class BackLinkTagHelper : AnchorTagHelper
	{
		[HtmlAttributeName("back-link-for")]
		public LinkItem BackLinkFor { get; set; }

		public BackLinkTagHelper(IHtmlGenerator generator) : base(generator) { }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			RouteValues.Add("id", ViewContext.RouteData.Values["id"].ToString());
			Page = BackLinkFor.Page;
			output.Attributes.Add("class", "govuk-back-link");
			output.Content.SetHtmlContent(BackLinkFor.BackText);
			base.Process(context, output);
		}
	}
}
