using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Models
{
	public class MonetaryInputModelBinder : IModelBinder
	{
		private readonly ILoggerFactory _loggerFactory;

		public MonetaryInputModelBinder(ILoggerFactory loggerFactory)
		{
			_loggerFactory = loggerFactory;
		}

		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext == null)
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}

			var modelType = bindingContext.ModelType;
			if (modelType != typeof(Decimal) && modelType != typeof(Decimal?))
			{
				throw new InvalidOperationException($"Cannot bind {modelType.Name}.");
			}

			var decimalResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

			if (decimalResult.FirstValue == string.Empty)
			{
				bindingContext.Result = ModelBindingResult.Success(0.0m);
				return Task.CompletedTask;
			}

			(new DecimalModelBinder(NumberStyles.Any, _loggerFactory)).BindModelAsync(bindingContext);

			if (bindingContext.ModelState.TryGetValue(bindingContext.ModelName, out var entry) && entry.Errors.Count > 0)
			{
				var displayName = bindingContext.ModelMetadata.DisplayName ?? bindingContext.ModelName;
				entry.Errors.Add($"{displayName} must be written in the correct format, like 5,000.00");
			}

			return Task.CompletedTask;
		}
	}
}
