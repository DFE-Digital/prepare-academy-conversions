﻿using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

// ReSharper disable ClassNeverInstantiated.Global

namespace Dfe.PrepareConversions.TagHelpers;

[HtmlTargetElement("govuk-summary-list-row", TagStructure = TagStructure.WithoutEndTag)]
public class SummaryListRowTagHelper(IHtmlHelper htmlHelper) : InputTagHelperBase(htmlHelper)
{
   [HtmlAttributeName("value")]
   public string Value { get; set; }

   [HtmlAttributeName("value-link")]
   public string ValueLink { get; set; }

   [HtmlAttributeName("additional-text")]
   public string AdditionalText { get; set; } // allows 2 items to be displayed in the same table row

   [HtmlAttributeName("asp-page")]
   public string Page { get; set; }

   [HtmlAttributeName("asp-fragment")]
   public string Fragment { get; set; }

   [HtmlAttributeName("asp-route-id")]
   public string RouteId { get; set; }

   [HtmlAttributeName("asp-route-urn")]
   public string RouteUrn { get; set; }

   [HtmlAttributeName("asp-route-return")]
   public string RouteReturn { get; set; }

   [HtmlAttributeName("hidden-text")]
   public string HiddenText { get; set; }

   [HtmlAttributeName("key-width")]
   public string KeyWidth { get; set; }

   [HtmlAttributeName("value-width")]
   public string ValueWidth { get; set; }

   [HtmlAttributeName("highlight-negative-value")]
   public bool HighlightNegativeValue { get; set; }

   [HtmlAttributeName("asp-read-only")]
   public bool IsReadOnly { get; set; }

   [HtmlAttributeName("asp-route-returnToPreview")]
   public bool ReturnToPreview { get; set; }

   protected override async Task<IHtmlContent> RenderContentAsync()
   {
      string value1 = For == null ? Value : For.Model?.ToString();

      SummaryListRowViewModel model = new()
      {
         Id = Id,
         Key = Label,
         Value = value1,
         ValueLink = ValueLink,
         AdditionalText = AdditionalText,
         Page = Page,
         Fragment = Fragment,
         RouteId = RouteId,
         RouteUrn = RouteUrn,
         Return = RouteReturn ?? ViewContext.ViewData["Return"]?.ToString(),
         HiddenText = HiddenText,
         KeyWidth = KeyWidth,
         ValueWidth = ValueWidth,
         Name = Name,
         HighlightNegativeValue = HighlightNegativeValue,
         IsReadOnly = IsReadOnly,
         ReturnToPreview = ReturnToPreview
      };

      return await _htmlHelper.PartialAsync("_SummaryListRow", model);
   }
}
