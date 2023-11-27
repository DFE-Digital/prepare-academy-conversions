using Dfe.PrepareConversions.Utils;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.PrepareConversions.TagHelpers
{
   [HtmlTargetElement("key-stage-data-row", TagStructure = TagStructure.NormalOrSelfClosing)]
   public class KeyStageDataRowTagHelper : TagHelper
   {
      [HtmlAttributeName("key-stage")]
      public KeyStageDataStatusHelper.KeyStages KeyStage { get; set; }
      [HtmlAttributeName("latest-year")]
      public string LatestYear { get; set; }

      public override void Process(TagHelperContext context, TagHelperOutput output)
      {
         output.TagName = null;
         string rowContent = KeyStageDataStatusHelper.KeyStage4DataRow(LatestYear);
         output.Content.SetHtmlContent(rowContent);
      }
   }
}
