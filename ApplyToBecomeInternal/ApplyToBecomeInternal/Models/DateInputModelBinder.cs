using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Linq;
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

			IDateValidationMessageProvider page =
				(bindingContext.ActionContext as Microsoft.AspNetCore.Mvc.RazorPages.PageContext)?.ViewData.Model as IDateValidationMessageProvider;

			var validator = new DateValidationService(page);
			(bool dateValid, string validationMessage) =
				validator.Validate(dayValueProviderResult.FirstValue, monthValueProviderResult.FirstValue, yearValueProviderResult.FirstValue, displayName);

			if (dateValid)
			{
				int day = int.Parse(dayValueProviderResult.FirstValue);
				int month = int.Parse(monthValueProviderResult.FirstValue);
				int year = int.Parse(yearValueProviderResult.FirstValue);

				var date = new DateTime(year, month, day);
				(bool validDateRange, string message) = IsInValidDateRange(date);

				if (validDateRange)
				{
					bindingContext.Result = ModelBindingResult.Success(date);
				}
				else
				{
					bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, message);
					bindingContext.Result = ModelBindingResult.Failed();
				}
			}
			else
			{
				bindingContext.ModelState.SetModelValue(dayModelName, dayValueProviderResult);
				bindingContext.ModelState.SetModelValue(monthModelName, monthValueProviderResult);
				bindingContext.ModelState.SetModelValue(yearModelName, yearValueProviderResult);
				bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, validationMessage);
				bindingContext.Result = ModelBindingResult.Failed();
			}

			return Task.CompletedTask;

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

			(bool, string) IsInValidDateRange(DateTime date)
			{
				if (bindingContext.ModelMetadata is DefaultModelMetadata defaultModelMetadata
					&& defaultModelMetadata.Attributes.Attributes.FirstOrDefault(a => a.GetType() == typeof(DateValidationAttribute)) is DateValidationAttribute dateValidation)
				{
					var rangeValidator = new DateRangeValidationService();
					return rangeValidator.Validate(date, dateValidation.DateValidationEnum, displayName);
				}

				return (true, string.Empty);
			}

			bool IsEmptyDate()
			{
				return dayValueProviderResult.FirstValue == string.Empty
					   && monthValueProviderResult.FirstValue == string.Empty
					   && yearValueProviderResult.FirstValue == string.Empty;
			}
		}
	}

	public class DateValidationAttribute : Attribute
	{
		public DateValidationAttribute(DateRangeValidationService.DateRange dateValidationEnum)
		{
			DateValidationEnum = dateValidationEnum;
		}

		public DateRangeValidationService.DateRange DateValidationEnum { get; }
	}
}