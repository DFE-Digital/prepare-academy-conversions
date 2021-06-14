using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ApplyToBecomeInternal.Extensions
{
	public class SubMenuLinkTagHelper : AnchorTagHelper
	{
		public SubMenuLinkTagHelper(IHtmlGenerator generator) : base(generator){}

		private readonly string _page = "page";
		public bool Highlighted { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if(Highlighted)
				output.Attributes.SetAttribute("aria-current", _page);

			output.TagName = "a";

			base.Process(context, output);
		}
	}
}
