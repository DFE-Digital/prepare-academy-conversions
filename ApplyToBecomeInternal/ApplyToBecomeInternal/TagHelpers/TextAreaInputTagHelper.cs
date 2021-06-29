using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.TagHelpers
{
	[HtmlTargetElement("govuk-textarea-input", TagStructure = TagStructure.WithoutEndTag)]
	public class TextAreaInputTagHelper : InputTagHelperBase
	{
		[HtmlAttributeName("rows")]
		public int Rows { get; set; }

		[HtmlAttributeName("rich-text")]
		public bool RichText { get; set; }

		public TextAreaInputTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

		protected override async Task<IHtmlContent> RenderContentAsync()
		{
			var model = new TextAreaInputViewModel
			{
				Id = Id,
				Name = Name,
				Label = Label,
				Value = For.Model?.ToString(),
				Rows = Rows,
				Hint = Hint,
				RichText = RichText
			};

			if (ViewContext.ModelState.TryGetValue(Name, out var entry) && entry.Errors.Count > 0)
			{
				model.ErrorMessage = entry.Errors[0].ErrorMessage;
			}

			return await _htmlHelper.PartialAsync("_TextAreaInput", model);
		}
	}
}
