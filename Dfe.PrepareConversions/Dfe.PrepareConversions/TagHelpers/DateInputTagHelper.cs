using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.TagHelpers;

[HtmlTargetElement("govuk-date-input", TagStructure = TagStructure.WithoutEndTag)]
public class DateInputTagHelper(IHtmlHelper htmlHelper, ErrorService errorService) : InputTagHelperBase(htmlHelper)
{
   public bool HeadingLabel { get; set; }

   protected override async Task<IHtmlContent> RenderContentAsync()
   {
      DateInputViewModel model = ValidateRequest();

      return await _htmlHelper.PartialAsync("_DateInput", model);
   }

   private DateInputViewModel ValidateRequest()
   {
      if (For.ModelExplorer.ModelType != typeof(DateTime?))
      {
         throw new ArgumentException("ModelType is not a DateTime?");
      }

      DateTime? date = For.Model as DateTime?;
      DateInputViewModel model = new()
      {
         Id = Id,
         Name = Name,
         Label = Label,
         SubLabel = SubLabel,
         HeadingLabel = HeadingLabel,
         Hint = Hint,
         PreviousInformation = PreviousInformation,
         AdditionalInformation = AdditionalInformation,
         DateString = date.ToDateString(),
         DetailsHeading = DetailsHeading,
         DetailsBody = DetailsBody
      };

      if (date.HasValue)
      {
         model.Day = date.Value.Day.ToString();
         model.Month = date.Value.Month.ToString();
         model.Year = date.Value.Year.ToString();
      }

      Error error = errorService.GetError(Name);
      if (error is not null)
      {
         model.ErrorMessage = error.Message;
         model.DayInvalid = error.InvalidInputs.Contains($"{Name}-day");
         if (ViewContext.HttpContext.Request.Form.TryGetValue($"{Name}-day", out StringValues dayValue))
         {
            model.Day = dayValue;
         }

         model.MonthInvalid = error.InvalidInputs.Contains($"{Name}-month");
         if (ViewContext.HttpContext.Request.Form.TryGetValue($"{Name}-month", out StringValues monthValue))
         {
            model.Month = monthValue;
         }

         model.YearInvalid = error.InvalidInputs.Contains($"{Name}-year");
         if (ViewContext.HttpContext.Request.Form.TryGetValue($"{Name}-year", out StringValues yearValue))
         {
            model.Year = yearValue;
         }

         if (!model.DayInvalid && !model.MonthInvalid && model.YearInvalid)
         {
            model.DayInvalid = model.MonthInvalid = model.YearInvalid = true;
         }
      }

      return model;
   }
}
