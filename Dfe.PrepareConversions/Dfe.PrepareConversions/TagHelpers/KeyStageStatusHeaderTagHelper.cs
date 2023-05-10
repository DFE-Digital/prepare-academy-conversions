using Dfe.PrepareConversions.Utils;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.PrepareConversions.TagHelpers
{
   [HtmlTargetElement("key-stage-header", TagStructure = TagStructure.NormalOrSelfClosing)]
   public class KeyStageHeaderTagHelper : TagHelper
   {
      [HtmlAttributeName("year-index")]
      public int YearIndex { get; set; }

      [HtmlAttributeName("key-stage")]
      public KeyStageDataStatusHelper.KeyStages KeyStage { get; set; }

      public override void Process(TagHelperContext context, TagHelperOutput output)
      {
         output.TagName = null; 
         string headerContent = KeyStageDataStatusHelper.KeyStageHeader(YearIndex, KeyStage);
         output.Content.AppendHtml(headerContent);
      }
   }
}
