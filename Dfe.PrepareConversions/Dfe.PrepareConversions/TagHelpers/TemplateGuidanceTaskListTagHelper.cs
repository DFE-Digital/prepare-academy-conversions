using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.PrepareConversions.TagHelpers
{
   [HtmlTargetElement("template-guidance")]
   public class TemplateGuidanceTaskListTagHelper : TagHelper
   {
      public string ProjectId { get; set; }
      public bool IsSponsored { get; set; }
      private readonly string _title = "Create a trust template";
      private readonly string _linkText = "Prepare your template";

      public override void Process(TagHelperContext context, TagHelperOutput output)
      {
         output.Content.SetHtmlContent(GetHtmlContent(_title, $"{ProjectId}/trust-guidance", _linkText));
      }

      private static string GetHtmlContent(string title, string urlSlug, string linkText)
      {
         return $@"
                <h3 class='app-task-list__section govuk-!-margin-top-8'>{title}</h3>
                <div class='app-task-list'>
                    <ul class='app-task-list__items govuk-!-padding-left-0'>
                        <li class='app-task-list__item'>
                            <span class='app-task-list__task-name'>
                                <a class='govuk-link' href='{urlSlug}' aria-describedby='trust-template-status'>
                                    {linkText}
                                </a>
                            </span>                           
                        </li>
                    </ul>
                </div>";
      }
   }
}

