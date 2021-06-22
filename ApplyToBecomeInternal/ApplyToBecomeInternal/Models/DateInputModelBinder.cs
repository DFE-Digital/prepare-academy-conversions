using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Models
{
	public class DateInputModelBinder : IModelBinder
	{
		private readonly List<string> _missingParts = new List<string>();
		private readonly List<string> _invalidParts = new List<string>();

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

			var dayValueProviderResult = bindingContext.ValueProvider.GetValue(dayModelName);
			var monthValueProviderResult = bindingContext.ValueProvider.GetValue(monthModelName);
			var yearValueProviderResult = bindingContext.ValueProvider.GetValue(yearModelName);

			if (IsOptionalOrFieldTypeMismatch())
			{
				if (modelType == typeof(DateTime?))
				{
					if (IsEmptyDate())
					{
						var date = default(DateTime);
						bindingContext.Result = ModelBindingResult.Success(date);
					}
					else
					{
						bindingContext.Result = ModelBindingResult.Success(null);
					}
				}
				else
				{
					bindingContext.Result = ModelBindingResult.Failed();
				}

				return Task.CompletedTask;
			}

			var displayName = bindingContext.ModelMetadata.DisplayName;

			int day = -1;
			int month = -1;
			int year = -1;

			var yearValid = TryParseYear();
			var monthValid = TryParseMonth();
			var dayValid = TryParseDay();

			if (dayValid && monthValid && yearValid)
			{
				var date = new DateTime(year, month, day);
				bindingContext.Result = ModelBindingResult.Success(date);
			}
			else
			{
				bindingContext.ModelState.SetModelValue(dayModelName, dayValueProviderResult);
				bindingContext.ModelState.SetModelValue(monthModelName, monthValueProviderResult);
				bindingContext.ModelState.SetModelValue(yearModelName, yearValueProviderResult);

				if (_missingParts.Count == 3)
				{
					bindingContext.ModelState.TryAddModelError(
							bindingContext.ModelName,
							$"Enter {displayName.ToLower()}");
				}
				else if (_missingParts.Count > 0)
				{
					bindingContext.ModelState.TryAddModelError(
							bindingContext.ModelName,
							$"{displayName} must include a {string.Join(" and ", _missingParts)}");
				}
				else if (_invalidParts.Count > 0)
				{
					bindingContext.ModelState.TryAddModelError(
							bindingContext.ModelName,
							$"{displayName} must be a real date");
				}
				else
				{
					bindingContext.ModelState.TryAddModelError(
							bindingContext.ModelName,
							$"{displayName} is not a valid date");
				}

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
						(month != -1 && month < 13 && year != -1 && day > DateTime.DaysInMonth(year, month)))
					{
						bindingContext.ModelState.TryAddModelError(
							dayModelName,
							$"error");
						_invalidParts.Add("day");

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
						$"error");
					_missingParts.Add("day");

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
							$"error");
						_invalidParts.Add("month");

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
						$"error");
					_missingParts.Add("month");

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
							$"error");
						_invalidParts.Add("year");

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
						$"error");
					_missingParts.Add("year");

					return false;
				}
			}

			bool IsOptionalOrFieldTypeMismatch()
			{
				return string.IsNullOrWhiteSpace(dayValueProviderResult.FirstValue)
					&& string.IsNullOrWhiteSpace(monthValueProviderResult.FirstValue)
					&& string.IsNullOrWhiteSpace(yearValueProviderResult.FirstValue)
					&& !bindingContext.ModelMetadata.IsRequired
					|| dayValueProviderResult == ValueProviderResult.None
					&& monthValueProviderResult == ValueProviderResult.None
					&& yearValueProviderResult == ValueProviderResult.None;
			}

			bool IsEmptyDate()
			{
				return dayValueProviderResult.FirstValue == string.Empty
				       && monthValueProviderResult.FirstValue == string.Empty
				       && yearValueProviderResult.FirstValue == string.Empty;
			}
		}
	}
}
