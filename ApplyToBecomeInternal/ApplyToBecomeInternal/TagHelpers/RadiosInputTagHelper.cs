using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.TagHelpers
{
	[HtmlTargetElement("govuk-radios-input", TagStructure = TagStructure.WithoutEndTag)]
	public class RadiosInputTagHelper : InputTagHelperBase
	{
		[HtmlAttributeName("values")]
		public string[] Values { get; set; }

		[HtmlAttributeName("labels")]
		public string[] Labels { get; set; }

		public RadiosInputTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

		protected override async Task<IHtmlContent> RenderContentAsync()
		{
			var model = new RadiosInputViewModel
			{
				Id = Id,
				Name = Name,
				Label = Label,
				Value = For.Model?.ToString(),
				Values = Values,
				Labels = Labels
			};

			return await _htmlHelper.PartialAsync("_RadiosInput", model);
		}
	}
}
