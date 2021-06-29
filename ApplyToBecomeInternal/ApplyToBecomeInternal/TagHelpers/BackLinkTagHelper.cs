using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Net;

namespace ApplyToBecomeInternal.TagHelpers
{
	[HtmlTargetElement("govuk-back-link", TagStructure = TagStructure.WithoutEndTag)]
	public class BackLinkTagHelper : AnchorTagHelper
	{
		[HtmlAttributeName("link-item")]
		public LinkItem LinkItem { get; set; }

		public BackLinkTagHelper(IHtmlGenerator generator) : base(generator) { }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			RouteValues.Add("id", ViewContext.RouteData.Values["id"].ToString());
			Page = LinkItem.Page;
			output.TagName = "a";
			output.TagMode = TagMode.StartTagAndEndTag;
			output.Attributes.Add("class", "govuk-back-link");
			if (ViewContext.HttpContext.Request.Query.ContainsKey("return") && ViewContext.HttpContext.Request.Query["return"].Count == 1)
			{
				var returnPage = ViewContext.HttpContext.Request.Query["return"][0];
				Page = WebUtility.UrlDecode(returnPage);
			}
			output.Content.SetHtmlContent(LinkItem.BackText);
			base.Process(context, output);
		}
	}
}
