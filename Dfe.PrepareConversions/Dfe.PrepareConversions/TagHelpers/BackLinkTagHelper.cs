using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Net;

namespace Dfe.PrepareConversions.TagHelpers
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

      private LinkItem LinkItemLookup => (Links.ByPage(LinkItem.Page) ?? Links.TaskList.Index);
      private LinkItem BackLookup => (Links.ByPage(Back.Page) ?? Links.TaskList.Index);
      private LinkItem ReturnLookup => (Links.ByPage(Return.Page) ?? Links.TaskList.Index);

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			RouteValues.Add("id", ViewContext.RouteData.Values["id"].ToString());
         Page = LinkItemLookup.Page;
			output.TagName = "a";
			output.TagMode = TagMode.StartTagAndEndTag;
			output.Attributes.Add("class", "govuk-back-link");
			var backLinkText = LinkItemLookup.BackText;

			if (ViewContext.HttpContext.Request.Query.ContainsKey("back") && ViewContext.HttpContext.Request.Query["back"].Count == 1)
			{
				var returnPage = ViewContext.HttpContext.Request.Query["back"][0];
            Page = (Links.ByPage(WebUtility.UrlDecode(returnPage)) ?? Links.TaskList.Index).Page;
				if(Back != null) RouteValues.Add("back", BackLookup.Page);
				if(Return != null) RouteValues.Add("return", ReturnLookup.Page);
			}
			else if (ViewContext.HttpContext.Request.Query.ContainsKey("return") && ViewContext.HttpContext.Request.Query["return"].Count == 1)
			{
				var returnPage = ViewContext.HttpContext.Request.Query["return"][0];
            Page = (Links.ByPage(WebUtility.UrlDecode(returnPage)) ?? Links.TaskList.Index).Page;
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
