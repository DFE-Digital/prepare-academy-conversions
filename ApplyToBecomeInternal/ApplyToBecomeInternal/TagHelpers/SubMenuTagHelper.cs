using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Extensions
{
	public class SubMenuTagHelper : TagHelper
	{
		private readonly string _page = "page";
		public SubMenuPage CurrentPage { get; set; }
		public string Url { get; set; }
		public string Title { get; set; }

		public int Order => throw new NotImplementedException();

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{

			output.TagName = "currentpagelabel";

			output.PreElement.SetHtmlContent("<a class='moj-sub-navigation__link'");
			output.PostElement.SetHtmlContent("</a>");
			
			output.Attributes.SetAttribute("href", Url);
			output.Content.SetContent(Title);

			if(CurrentPage == SubMenuPage.ProjectNotes)
			{
				HighlightPageInSubMenu(output);
			}
			else if(CurrentPage == SubMenuPage.SchoolApplicationForm)
			{
				HighlightPageInSubMenu(output);
			}
			else if(CurrentPage == SubMenuPage.TaskList)
			{
				HighlightPageInSubMenu(output);
			}
		}

		private void HighlightPageInSubMenu(TagHelperOutput output)
		{
			output.Attributes.SetAttribute("aria-current", _page);
		}

		public void Init(TagHelperContext context)
		{
			throw new NotImplementedException();
		}
	}
}
