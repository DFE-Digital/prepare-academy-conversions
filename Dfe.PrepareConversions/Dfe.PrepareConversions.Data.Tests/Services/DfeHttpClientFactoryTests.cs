using Dfe.Academisation.CorrelationIdMiddleware;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Tests.TestDoubles;
using FluentAssertions;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Net.Http;
using Xunit;
using Dfe.Prepare.Data;

namespace Dfe.PrepareConversions.Data.Tests.Services;
public class DfeHttpClientFactoryTests
{
   [Fact]
   public void TramsClientName_IsCorrect()
   {
      DfeHttpClientFactory.TramsClientName.Should().Be("TramsClient");
   }

   [Fact]
   public void AcademisationClientName_IsCorrect()
   {
      DfeHttpClientFactory.AcademisationClientName.Should().Be("AcademisationClient");
   }

   [Fact]
   public void CreatingTramsClient_Should_Invoke_HttpClientFactory_With_CorrectClientName()
   {
      var unexpectedApiClient = DfeHttpClientFactory.AcademisationClientName;
      var mockHttpClientFactory = Mock.Of<IHttpClientFactory>(x => x.CreateClient(DfeHttpClientFactory.TramsClientName) == new MockHttpClientFactory(new MockHttpMessageHandler(BackendDefinitionBehavior.Always))
         .CreateClient(DfeHttpClientFactory.TramsClientName));

      var mockCorrelationId = Guid.NewGuid();
      var mockCorrelationContext = Mock.Of<ICorrelationContext>(x => x.CorrelationId == mockCorrelationId);

      var sut = new DfeHttpClientFactory(mockHttpClientFactory, mockCorrelationContext);

      _ = sut.CreateTramsClient();

      Mock.Get(mockHttpClientFactory).Verify(x => x.CreateClient(DfeHttpClientFactory.TramsClientName), Times.Once);
      Mock.Get(mockHttpClientFactory).Verify(x => x.CreateClient(unexpectedApiClient), Times.Never);
   }

   [Fact]
   public void CreatingAcademisationClient_Should_Invoke_HttpClientFactory_With_CorrectClientName()
   {
      var expectedApiClient = DfeHttpClientFactory.AcademisationClientName;
      var unexpectedApiClient = DfeHttpClientFactory.TramsClientName;
      var mockHttpClientFactory = Mock.Of<IHttpClientFactory>(x => x.CreateClient(expectedApiClient) == new MockHttpClientFactory(new MockHttpMessageHandler(BackendDefinitionBehavior.Always)).CreateClient(expectedApiClient));

      var mockCorrelationId = Guid.NewGuid();
      var mockCorrelationContext = Mock.Of<ICorrelationContext>(x => x.CorrelationId == mockCorrelationId);

      var sut = new DfeHttpClientFactory(mockHttpClientFactory, mockCorrelationContext);

      _ = sut.CreateAcademisationClient();

      Mock.Get(mockHttpClientFactory).Verify(x => x.CreateClient(expectedApiClient), Times.Once);
      Mock.Get(mockHttpClientFactory).Verify(x => x.CreateClient(unexpectedApiClient), Times.Never);
   }
   
   [Fact]
   public void CreatingTramsClient_Should_Add_CorrelationId_Header()
   {
      var mockHttpClientFactory = Mock.Of<IHttpClientFactory>(x => x.CreateClient(DfeHttpClientFactory.TramsClientName) == new MockHttpClientFactory(new MockHttpMessageHandler(BackendDefinitionBehavior.Always))
         .CreateClient(DfeHttpClientFactory.TramsClientName));

      var mockCorrelationId = Guid.NewGuid();
      var mockCorrelationContext = Mock.Of<ICorrelationContext>(x => x.CorrelationId == mockCorrelationId);

      var sut = new DfeHttpClientFactory(mockHttpClientFactory, mockCorrelationContext);

      _ = sut.CreateTramsClient();

      Mock.Get(mockHttpClientFactory).Verify(x => x.CreateClient(DfeHttpClientFactory.TramsClientName), Times.Once);
   }
   
   [Fact]
   public void CreatingAcademisationClient_Should_Add_CorrelationId_Header()
   {
      var mockHttpClientFactory = Mock.Of<IHttpClientFactory>(x => x.CreateClient(DfeHttpClientFactory.TramsClientName) == new MockHttpClientFactory(new MockHttpMessageHandler(BackendDefinitionBehavior.Always)).CreateClient(DfeHttpClientFactory.TramsClientName));

      var mockCorrelationId = Guid.NewGuid();
      var mockCorrelationContext = Mock.Of<ICorrelationContext>(x => x.CorrelationId == mockCorrelationId);

      var sut = new DfeHttpClientFactory(mockHttpClientFactory, mockCorrelationContext);

      _ = sut.CreateTramsClient();

      Mock.Get(mockHttpClientFactory).Verify(x => x.CreateClient(DfeHttpClientFactory.TramsClientName), Times.Once);
   }
}
