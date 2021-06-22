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
				model.DayInvalid = error.AttemptedValues.TryGetValue($"{Name}-day", out var day) && day.IsInvalid;
				model.Day = day.Value;
				model.MonthInvalid = error.AttemptedValues.TryGetValue($"{Name}-month", out var month) && month.IsInvalid;
				model.Month = month.Value;
				model.YearInvalid = error.AttemptedValues.TryGetValue($"{Name}-year", out var year) && year.IsInvalid;
				model.Year = year.Value;
			}

			return await _htmlHelper.PartialAsync("_DateInput", model);
		}
	}
}
