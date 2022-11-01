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

		[HtmlAttributeName("back")]
		public LinkItem Back { get; set; }

		[HtmlAttributeName("return")]
		public LinkItem Return { get; set; }

		public BackLinkTagHelper(IHtmlGenerator generator) : base(generator) { }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			RouteValues.Add("id", ViewContext.RouteData.Values["id"].ToString());
			Page = LinkItem.Page;
			output.TagName = "a";
			output.TagMode = TagMode.StartTagAndEndTag;
			output.Attributes.Add("class", "govuk-back-link");
			var backLinkText = LinkItem.BackText;
			if (ViewContext.HttpContext.Request.Query.ContainsKey("back") && ViewContext.HttpContext.Request.Query["back"].Count == 1)
			{
				var returnPage = ViewContext.HttpContext.Request.Query["back"][0];
				Page = WebUtility.UrlDecode(returnPage);
				if(Back != null) RouteValues.Add("back", Back.Page);
				if(Return != null) RouteValues.Add("return", Return.Page);
			}
			else if (ViewContext.HttpContext.Request.Query.ContainsKey("return") && ViewContext.HttpContext.Request.Query["return"].Count == 1)
			{
				var returnPage = ViewContext.HttpContext.Request.Query["return"][0];
				Page = WebUtility.UrlDecode(returnPage);
				if (ViewContext.HttpContext.Request.Query.ContainsKey("backText") && ViewContext.HttpContext.Request.Query["backText"].Count == 1)
				{
					backLinkText = ViewContext.HttpContext.Request.Query["backText"][0];
				}
			}
			output.Content.SetHtmlContent(backLinkText);
			base.Process(context, output);
		}
	}
}
