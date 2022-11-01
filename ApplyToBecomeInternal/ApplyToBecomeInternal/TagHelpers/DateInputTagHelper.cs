using ApplyToBecomeInternal.Services;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.TagHelpers
{
	[HtmlTargetElement("govuk-date-input", TagStructure = TagStructure.WithoutEndTag)]
	public class DateInputTagHelper : InputTagHelperBase
	{
		public bool HeadingLabel { get; set; }
		
		private readonly ErrorService _errorService;

		public DateInputTagHelper(IHtmlHelper htmlHelper, ErrorService errorService) : base(htmlHelper)
		{
			_errorService = errorService;
		}

		protected override async Task<IHtmlContent> RenderContentAsync()
		{
			if (For.ModelExplorer.ModelType != typeof(DateTime?))
			{
				throw new ArgumentException();
			}

			var date = For.Model as DateTime?;
			var model = new DateInputViewModel
			{
				Id = Id,
				Name = Name,
				Label = Label,
				HeadingLabel = HeadingLabel,
				Hint = Hint
			};

			if (date != null && date.HasValue)
			{
				model.Day = date.Value.Day.ToString();
				model.Month = date.Value.Month.ToString();
				model.Year = date.Value.Year.ToString();
			}

			var error = _errorService.GetError(Name);
			if (error != null)
			{
				model.ErrorMessage = error.Message;
				model.DayInvalid = error.InvalidInputs.Contains($"{Name}-day");
				if (ViewContext.HttpContext.Request.Form.TryGetValue($"{Name}-day", out var dayValue))
				{
					model.Day = dayValue;
				}
				model.MonthInvalid = error.InvalidInputs.Contains($"{Name}-month");
				if (ViewContext.HttpContext.Request.Form.TryGetValue($"{Name}-month", out var monthValue))
				{
					model.Month = monthValue;
				}
				model.YearInvalid = error.InvalidInputs.Contains($"{Name}-year");
				if (ViewContext.HttpContext.Request.Form.TryGetValue($"{Name}-year", out var yearValue))
				{
					model.Year = yearValue;
				}
				if (model.DayInvalid == false && model.MonthInvalid == false && model.YearInvalid == false)
				{
					model.DayInvalid = model.MonthInvalid = model.YearInvalid = true;
				}
			}

			return await _htmlHelper.PartialAsync("_DateInput", model);
		}

	}
}
