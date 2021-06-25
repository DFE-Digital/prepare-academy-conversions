using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Models
{
	public class CheckboxInputModelBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext == null)
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}

			var modelType = bindingContext.ModelType;
			if (modelType != typeof(bool) && modelType != typeof(bool?))
			{
				throw new InvalidOperationException($"Cannot bind {modelType.Name}.");
			}

			var checkboxInputModelName = $"{bindingContext.ModelName}";
			var hiddenInputModelName = $"{bindingContext.ModelName}-hidden";

			var checkboxInputValueProviderResult = bindingContext.ValueProvider.GetValue(checkboxInputModelName);
			var hiddenInputValueProviderResult = bindingContext.ValueProvider.GetValue(hiddenInputModelName);

			if (checkboxInputValueProviderResult == ValueProviderResult.None
				&& hiddenInputValueProviderResult == ValueProviderResult.None)
			{
				if (modelType == typeof(bool?))
				{
					bindingContext.Result = ModelBindingResult.Success(null);
				}
				else
				{
					bindingContext.Result = ModelBindingResult.Failed();
				}

				return Task.CompletedTask;
			}


			if (checkboxInputValueProviderResult != ValueProviderResult.None && bool.TryParse(checkboxInputValueProviderResult.FirstValue, out var checkboxInputValue))
			{
				bindingContext.Result = ModelBindingResult.Success(checkboxInputValue);
			}
			else if (hiddenInputValueProviderResult != ValueProviderResult.None && bool.TryParse(hiddenInputValueProviderResult.FirstValue, out var hiddenInputValue))
			{
				bindingContext.Result = ModelBindingResult.Success(hiddenInputValue);
			}
			else
			{
				bindingContext.ModelState.SetModelValue(checkboxInputModelName, checkboxInputValueProviderResult);
				bindingContext.ModelState.SetModelValue(hiddenInputModelName, hiddenInputValueProviderResult);

				bindingContext.ModelState.TryAddModelError(
					bindingContext.ModelName,
					new ArgumentException("Not a valid boolean checkbox"),
					bindingContext.ModelMetadata);

				bindingContext.Result = ModelBindingResult.Failed();
			}

			return Task.CompletedTask;
		}
	}
}
