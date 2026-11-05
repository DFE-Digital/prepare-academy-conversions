using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.TagHelpers;

[HtmlTargetElement("supporting-evidence-input", TagStructure = TagStructure.WithoutEndTag)]
public class SupportingEvidenceInputTagHelper(IHtmlHelper htmlHelper) : InputTagHelperBase(htmlHelper)
{
   private string DataTest { get; set; }

   public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
   {
      if (context.AllAttributes.TryGetAttribute("data-test", out TagHelperAttribute dataTest))
      {
         DataTest = dataTest.Value?.ToString();
      }

      if (For is not null)
      {
         Name ??= For.Name;
         Id ??= Name;
      }

      return base.ProcessAsync(context, output);
   }

   protected override async Task<IHtmlContent> RenderContentAsync()
   {
      SupportingEvidenceInputViewModel model = new()
      {
         Id = Id,
         Name = Name,
         Value = For.Model?.ToString(),
         DataTest = DataTest
      };

      if (ViewContext.ModelState.TryGetValue(Name, out ModelStateEntry entry) && entry.Errors.Count > 0)
      {
         model.ErrorMessage = entry.Errors[0].ErrorMessage;
      }

      return await _htmlHelper.PartialAsync("_SupportingEvidenceInput", model);
   }
}