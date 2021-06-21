using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.TagHelpers
{
	[HtmlTargetElement("govuk-checkbox-input", TagStructure = TagStructure.WithoutEndTag)]
	public class CheckboxInputTagHelper : InputTagHelperBase
	{
		public CheckboxInputTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

		protected override async Task<IHtmlContent> RenderContentAsync()
		{
			var model = new CheckboxInputViewModel
			{
				Id = Id,
				Name = Name,
				Label = Label,
				Value = For.Model?.ToString()
			};

			return await _htmlHelper.PartialAsync("_CheckboxInput", model);
		}
	}
}
