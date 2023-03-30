using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Dfe.PrepareConversions.TagHelpers;

[HtmlTargetElement("inline-error", TagStructure = TagStructure.WithoutEndTag)]
public class InlineErrorMessageTagHelper : TagHelper
{
   [ViewContext]
   [HtmlAttributeNotBound]
   public ViewContext ViewContext { get; set; }

   [HtmlAttributeName("for")]
   public string For { get; set; }

   public override void Process(TagHelperContext context, TagHelperOutput output)
   {
      if (ViewContext.ModelState.ContainsKey(For) is false)
      {
         output.SuppressOutput();
         return;
      }

      output.TagName = "span";
      output.TagMode = TagMode.StartTagAndEndTag;
      output.Attributes.Add("class", "govuk-error-message");
      output.Attributes.Add("id", $"{For}-error");

      string message = ' ' + ViewContext.ModelState[For]?.Errors.First().ErrorMessage;

      output.Content.SetHtmlContent($"<span class=\"govuk-visually-hidden\">Error:</span> {message}");
   }
}
