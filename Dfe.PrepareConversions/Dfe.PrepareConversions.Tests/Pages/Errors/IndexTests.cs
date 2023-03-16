using Dfe.PrepareConversions.Pages.Errors;
using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.Errors;

public class IndexTests
{
   private readonly Mock<HttpContext> _httpContextMock;
   private readonly IndexModel _model;

   public IndexTests()
   {
      _httpContextMock = new Mock<HttpContext>();

      ActionContext actionContext = new(_httpContextMock.Object, new RouteData(), new PageActionDescriptor(), new ModelStateDictionary());
      PageContext pageContext = new(actionContext);
      _model = new IndexModel { PageContext = pageContext };
   }

   [Theory]
   [InlineData(404, "Page not found")]
   [InlineData(500, "Internal server error")]
   [InlineData(501, "Not implemented")]
   [InlineData(99999, "Error 99999")]
   public void OnGet_WhenResponseHasAStatusCode_SetsMessageCorrectly(int statusCode, string expectedMessage)
   {
      _model.OnGet(statusCode);

      _model.ErrorMessage.Should().Be(expectedMessage);
   }

   [Fact]
   public void OnGet_WhenUnhandledError_HasDefaultMessage()
   {
      Mock<IFeatureCollection> mockIFeatureCollection = new();
      mockIFeatureCollection.Setup(collection => collection.Get<IExceptionHandlerPathFeature>())
         .Returns(new ExceptionHandlerFeature { Error = new Exception() });
      _httpContextMock.Setup(context => context.Features).Returns(mockIFeatureCollection.Object);

      _model.OnGet();

      _model.ErrorMessage.Should().Be("Sorry, there is a problem with the service");
   }

   [Fact]
   public void OnGet_WhenUnhandledNoPageNamedError_SetsPageNotFoundMessageAndStatus()
   {
      Mock<IFeatureCollection> mockIFeatureCollection = new();
      mockIFeatureCollection.Setup(collection => collection.Get<IExceptionHandlerPathFeature>())
         .Returns(new ExceptionHandlerFeature { Error = new InvalidOperationException("No page named") });
      _httpContextMock.Setup(context => context.Features).Returns(mockIFeatureCollection.Object);
      _httpContextMock.SetupSet(context => context.Response.StatusCode = 404).Verifiable();

      _model.OnGet();

      _model.ErrorMessage.Should().Be("Page not found");
      _httpContextMock.Verify();
   }

   [Theory]
   [InlineData(404, "Page not found")]
   [InlineData(500, "Internal server error")]
   [InlineData(501, "Not implemented")]
   [InlineData(99999, "Error 99999")]
   public void OnPost_WhenResponseHasAStatusCode_SetsMessageCorrectly(int statusCode, string expectedMessage)
   {
      _model.OnPost(statusCode);

      _model.ErrorMessage.Should().Be(expectedMessage);
   }

   [Fact]
   public void OnPost_WhenUnhandledError_HasDefaultMessage()
   {
      Mock<IFeatureCollection> mockIFeatureCollection = new();
      mockIFeatureCollection.Setup(collection => collection.Get<IExceptionHandlerPathFeature>())
         .Returns(new ExceptionHandlerFeature { Error = new Exception() });
      _httpContextMock.Setup(context => context.Features).Returns(mockIFeatureCollection.Object);

      _model.OnPost();

      _model.ErrorMessage.Should().Be("Sorry, there is a problem with the service");
   }

   [Fact]
   public void OnPost_WhenUnhandledNoPageNamedError_SetsPageNotFoundMessageAndStatus()
   {
      Mock<IFeatureCollection> mockIFeatureCollection = new();
      mockIFeatureCollection.Setup(collection => collection.Get<IExceptionHandlerPathFeature>())
         .Returns(new ExceptionHandlerFeature { Error = new InvalidOperationException("No page named") });
      _httpContextMock.Setup(context => context.Features).Returns(mockIFeatureCollection.Object);
      _httpContextMock.SetupSet(context => context.Response.StatusCode = 404).Verifiable();

      _model.OnPost();

      _model.ErrorMessage.Should().Be("Page not found");
      _httpContextMock.Verify();
   }
}
