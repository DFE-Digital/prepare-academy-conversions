using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Models
{
	public class DecimalInputModelBinder : IModelBinder
	{
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
				bindingContext.Result = ModelBindingResult.Success(default(decimal));
				return Task.CompletedTask;
			}

			return (new DecimalModelBinder(NumberStyles.Any, new LoggerFactory())).BindModelAsync(bindingContext);
		}
	}
}
