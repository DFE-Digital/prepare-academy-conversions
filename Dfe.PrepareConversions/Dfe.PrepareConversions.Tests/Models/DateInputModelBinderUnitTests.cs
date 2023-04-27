using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Tests.Customisations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Models;

public class DateInputModelBinderUnitTests
{
   [Theory]
   [AutoMoqData]
   public async Task Should_throw_when_bindingcontext_is_null(DateInputModelBinder sut)
   {
      ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.BindModelAsync(null));

      Assert.Equal("bindingContext", exception.ParamName);
   }

   [Theory]
   [AutoMoqData]
   public async Task Should_throw_when_invalid_model_type(DateInputModelBinder sut)
   {
      QueryCollection queryCollection = new(new Dictionary<string, StringValues>());
      QueryStringValueProvider vp = new(BindingSource.Query, queryCollection, CultureInfo.CurrentCulture);
      Type modelType = typeof(string);

      EmptyModelMetadataProvider metadataProvider = new();

      InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(() => sut.BindModelAsync(new DefaultModelBindingContext
      {
         ModelMetadata = metadataProvider.GetMetadataForType(modelType), ModelName = "test", ModelState = new ModelStateDictionary(), ValueProvider = vp
      }));

      Assert.Equal($"Cannot bind {modelType.Name}.", exception.Message);
   }

   [Theory]
   [AutoMoqData]
   public async Task Should_bind_to_valid_model(DateInputModelBinder sut)
   {
      string id = "test";
      QueryCollection queryCollection = new(
         new Dictionary<string, StringValues> { { $"{id}-day", new StringValues("01") }, { $"{id}-month", new StringValues("01") }, { $"{id}-year", new StringValues("2020") } });
      QueryStringValueProvider vp = new(BindingSource.Query, queryCollection, CultureInfo.CurrentCulture);
      Type modelType = typeof(DateTime?);

      EmptyModelMetadataProvider metadataProvider = new();
      DefaultModelBindingContext bindingContext = new()
      {
         ModelMetadata = metadataProvider.GetMetadataForType(modelType), ModelName = id, ModelState = new ModelStateDictionary(), ValueProvider = vp
      };

      await sut.BindModelAsync(bindingContext);

      Assert.Equal("Success '01/01/2020 00:00:00'", bindingContext.Result.ToString());
   }
}
