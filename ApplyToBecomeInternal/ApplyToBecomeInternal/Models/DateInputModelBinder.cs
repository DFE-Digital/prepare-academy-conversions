using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Models
{
	public class DateInputModelBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext == null)
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}

			var modelType = bindingContext.ModelType;
			if (modelType != typeof(DateTime) && modelType != typeof(DateTime?))
			{
				throw new InvalidOperationException($"Cannot bind {modelType.Name}.");
			}

			var dayModelName = $"{bindingContext.ModelName}-day";
			var monthModelName = $"{bindingContext.ModelName}-month";
			var yearModelName = $"{bindingContext.ModelName}-year";

			var dayValueProviderResult  = bindingContext.ValueProvider.GetValue(dayModelName);
			var monthValueProviderResult  = bindingContext.ValueProvider.GetValue(monthModelName);
			var yearValueProviderResult  = bindingContext.ValueProvider.GetValue(yearModelName);

			if (dayValueProviderResult  == ValueProviderResult.None
				&& monthValueProviderResult  == ValueProviderResult.None
				&& yearValueProviderResult  == ValueProviderResult.None)
			{
				if (modelType == typeof(DateTime?))
				{
					bindingContext.Result = ModelBindingResult.Success(null);
				}
				else
				{
					bindingContext.Result = ModelBindingResult.Failed();
				}

				return Task.CompletedTask;
			}

			int day = -1;
			int month = -1;
			int year = -1;

			if (TryParseYear() && TryParseMonth() && TryParseDay())
			{
				var date = new DateTime(year, month, day);
				bindingContext.Result = ModelBindingResult.Success(date);
			}
			else if (dayValueProviderResult.FirstValue == string.Empty
			         && monthValueProviderResult.FirstValue == string.Empty
			         && yearValueProviderResult.FirstValue == string.Empty)
			{
				var date = default(DateTime);
				bindingContext.Result = ModelBindingResult.Success(date);
			}
			else
			{
				bindingContext.ModelState.SetModelValue(dayModelName, dayValueProviderResult);
				bindingContext.ModelState.SetModelValue(monthModelName, monthValueProviderResult);
				bindingContext.ModelState.SetModelValue(yearModelName, yearValueProviderResult);

				bindingContext.ModelState.TryAddModelError(
					bindingContext.ModelName,
					new FormatException("Invalid date specified."),
					bindingContext.ModelMetadata);

				bindingContext.Result = ModelBindingResult.Failed();
			}

			return Task.CompletedTask;

			bool TryParseDay()
			{
				if (dayValueProviderResult != ValueProviderResult.None &&
					dayValueProviderResult.FirstValue != string.Empty)
				{
					if (!int.TryParse(dayValueProviderResult.FirstValue, out day) ||
						day < 1 ||
						(month != -1 && year != -1 && day > DateTime.DaysInMonth(year, month)))
					{
						bindingContext.ModelState.TryAddModelError(
							dayModelName,
							"Day is not valid.");

						bindingContext.ModelState.TryGetValue(bindingContext.ModelName, out var x);

						return false;
					}
					else
					{
						return true;
					}
				}
				else
				{
					bindingContext.ModelState.TryAddModelError(
						dayModelName,
						"Day is missing.");

					return false;
				}
			}

			bool TryParseMonth()
			{
				if (monthValueProviderResult != ValueProviderResult.None &&
					monthValueProviderResult.FirstValue != string.Empty)
				{
					if (!int.TryParse(monthValueProviderResult.FirstValue, out month) ||
						month < 1 ||
						month > 12)
					{
						bindingContext.ModelState.TryAddModelError(
							monthModelName,
							"Month is not valid.");

						return false;
					}
					else
					{
						return true;
					}
				}
				else
				{
					bindingContext.ModelState.TryAddModelError(
						monthModelName,
						"Month is missing.");

					return false;
				}
			}

			bool TryParseYear()
			{
				if (yearValueProviderResult != ValueProviderResult.None &&
					yearValueProviderResult.FirstValue != string.Empty)
				{
					if (!int.TryParse(yearValueProviderResult.FirstValue, out year) ||
						year < 1 ||
						year > 9999)
					{
						bindingContext.ModelState.TryAddModelError(
							yearModelName,
							"Year is not valid.");

						return false;
					}
					else
					{
						return true;
					}
				}
				else
				{
					bindingContext.ModelState.TryAddModelError(
						yearModelName,
						"Year is missing.");

					return false;
				}
			}
		}
	}
}
