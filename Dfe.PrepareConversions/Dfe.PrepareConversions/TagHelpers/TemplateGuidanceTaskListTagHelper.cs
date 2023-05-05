using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.PrepareConversions.TagHelpers
{
   [HtmlTargetElement("template-guidance")]
   public class TemplateGuidanceTaskListTagHelper : TagHelper
   {
      public string ProjectId { get; set; }
      public bool IsSponsored { get; set; }

      public override void Process(TagHelperContext context, TagHelperOutput output)
      {
         string urlSlug = $"{ProjectId}/template-guidance";
         if (IsSponsored)
         {
            output.Content.SetHtmlContent($@"
                <h3 class='app-task-list__section govuk-!-margin-top-8'>Sponsor template guidance</h3>
                <div class='app-task-list'>
                    <ul class='app-task-list__items govuk-!-padding-left-0'>
                        <li class='app-task-list__item'>
                            <span class='app-task-list__task-name'>
                                <a class='govuk-link' href='{urlSlug}' aria-describedby='trust-template-status'>
                                    Prepare your sponsor template
                                </a>
                            </span>
                            <strong class='govuk-tag govuk-tag--grey app-task-list__tag' id='trust-template-status'>Reference only</strong>
                        </li>
                    </ul>
                </div>");
         }
         else
         {
            output.Content.SetHtmlContent($@"
                <h3 class='app-task-list__section govuk-!-margin-top-8'>Trust template guidance</h3>
                <div class='app-task-list'>
                    <ul class='app-task-list__items govuk-!-padding-left-0'>
                        <li class='app-task-list__item'>
                            <span class='app-task-list__task-name'>
                                <a class='govuk-link' href='{urlSlug}' aria-describedby='trust-template-status'>
                                    Prepare your trust template
                                </a>
                            </span>
                            <strong class='govuk-tag govuk-tag--grey app-task-list__tag' id='trust-template-status'>Reference only</strong>
                        </li>
                    </ul>
                </div>");
         }
      }
   }
}