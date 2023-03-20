using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.TagHelpers;

[HtmlTargetElement("govuk-radios-input", TagStructure = TagStructure.WithoutEndTag)]
public class RadiosInputTagHelper : InputTagHelperBase
{
   public RadiosInputTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

   [HtmlAttributeName("leading-paragraph")]
   public string LeadingParagraph { get; set; }

   [HtmlAttributeName("heading-label")]
   public bool HeadingLabel { get; set; }

   [HtmlAttributeName("values")]
   public string[] Values { get; set; }

   [HtmlAttributeName("labels")]
   public string[] Labels { get; set; }

   protected override async Task<IHtmlContent> RenderContentAsync()
   {
      RadiosInputViewModel model = new()
      {
         Id = Id,
         Name = Name,
         Label = Label,
         Value = For.Model?.ToString(),
         Values = Values,
         Labels = Labels,
         HeadingLabel = HeadingLabel,
         LeadingParagraph = LeadingParagraph
      };

      return await _htmlHelper.PartialAsync("_RadiosInput", model);
   }
}
