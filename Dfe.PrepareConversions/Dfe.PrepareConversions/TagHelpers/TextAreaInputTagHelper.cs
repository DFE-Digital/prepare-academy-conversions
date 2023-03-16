using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.TagHelpers;

[HtmlTargetElement("govuk-textarea-input", TagStructure = TagStructure.WithoutEndTag)]
public class TextAreaInputTagHelper : InputTagHelperBase
{
   public TextAreaInputTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

   [HtmlAttributeName("heading-label")]
   public bool HeadingLabel { get; set; }

   [HtmlAttributeName("rows")]
   public int Rows { get; set; }

   [HtmlAttributeName("rich-text")]
   public bool RichText { get; set; }

   protected override async Task<IHtmlContent> RenderContentAsync()
   {
      TextAreaInputViewModel model = new()
      {
         Id = Id,
         Name = Name,
         Label = Label,
         Value = For.Model?.ToString(),
         Rows = Rows,
         Hint = Hint,
         RichText = RichText,
         HeadingLabel = HeadingLabel
      };

      if (ViewContext.ModelState.TryGetValue(Name, out ModelStateEntry entry) && entry.Errors.Count > 0)
      {
         model.ErrorMessage = entry.Errors[0].ErrorMessage;
      }

      return await _htmlHelper.PartialAsync("_TextAreaInput", model);
   }
}
