using Dfe.PrepareConversions.Data.Tests.AutoFixture;
using Dfe.PrepareConversions.Models;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using System.Globalization;
using System;

namespace Dfe.PrepareConversions.Tests.Models
{
	public class DateInputModelBinderUnitTests
	{
		[Theory, AutoMoqData]
		public async Task Should_throw_when_bindingcontext_is_null(DateInputModelBinder sut)
		{
			var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.BindModelAsync(null));

			Assert.Equal("bindingContext", exception.ParamName);
		}

		[Theory, AutoMoqData]
		public async Task Should_throw_when_invalid_model_type(DateInputModelBinder sut)
		{
			var queryCollection = new QueryCollection(new Dictionary<string, StringValues>());
			var vp = new QueryStringValueProvider(BindingSource.Query, queryCollection, CultureInfo.CurrentCulture);
			var modelType = typeof(string);

			var metadataProvider = new EmptyModelMetadataProvider();

			var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => sut.BindModelAsync(new DefaultModelBindingContext
			{
				ModelMetadata = metadataProvider.GetMetadataForType(modelType),
				ModelName = "test",
				ModelState = new ModelStateDictionary(),
				ValueProvider = vp,
			}));

			Assert.Equal($"Cannot bind {modelType.Name}.", exception.Message);
		}

		[Theory, AutoMoqData]
		public async Task Should_bind_to_valid_model(DateInputModelBinder sut)
		{
			var id = "test";
			var queryCollection = new QueryCollection(
				new Dictionary<string, StringValues>()
				{
					{ $"{id}-day", new StringValues("01") },
					{ $"{id}-month", new StringValues("01") },
					{ $"{id}-year", new StringValues("2020") },
				});
			var vp = new QueryStringValueProvider(BindingSource.Query, queryCollection, CultureInfo.CurrentCulture);
			var modelType = typeof(DateTime?);

			var metadataProvider = new EmptyModelMetadataProvider();
			var bindingContext = new DefaultModelBindingContext
			{
				ModelMetadata = metadataProvider.GetMetadataForType(modelType),
				ModelName = id,
				ModelState = new ModelStateDictionary(),
				ValueProvider = vp,
			};

			await sut.BindModelAsync(bindingContext);

			Assert.Equal("Success '01/01/2020 00:00:00'", bindingContext.Result.ToString());
		}
	}
}
