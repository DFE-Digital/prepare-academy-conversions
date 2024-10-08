using System.Text.Encodings.Web;
using Dfe.PrepareTransfers.Web.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace Dfe.PrepareTransfers.Web.Helpers.TagHelpers
{
   [HtmlTargetElement("backtopreview")]
   public class BackToPreviewPageTagHelper : TagHelper
   {
      private readonly LinkGenerator _linkGenerator;
      public bool ReturnToPreview { get; set; }
      public string Urn { get; set; }

      public BackToPreviewPageTagHelper(LinkGenerator linkGenerator)
      {
         _linkGenerator = linkGenerator;
      }

      public override async void Process(TagHelperContext context, TagHelperOutput output)
      {
         if (ReturnToPreview)
         {
            output.TagName = "a";
            output.AddClass("govuk-back-link", HtmlEncoder.Default);
            output.Attributes.Add("href",
                _linkGenerator.GetPathByPage(Links.HeadteacherBoard.Preview.PageName, null, new { Area = "Transfers", Urn }));
            output.Content.SetContent(Links.HeadteacherBoard.Preview.BackText);
         }
         else
         {
            var childContent = await output.GetChildContentAsync();
            var content = childContent.GetContent();
            output.TagName = null;
            output.Content.SetHtmlContent(content);
         }
      }
   }
}