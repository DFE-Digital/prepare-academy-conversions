using Dfe.Academisation.CorrelationIdMiddleware;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Tests.TestDoubles;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Data.Tests.Services;
public class DfeHttpClientFactoryTests
{
   [Fact]
   public void CreatingTramsClient_Should_Invoke_HttpClientFactory_With_CorrectClientName()
   {
      var expectedApiClient = "TramsClient";
      var unexpectedApiClient = "AcademisationClient";
      var mockHttpClientFactory = Mock.Of<IHttpClientFactory>(x => x.CreateClient(expectedApiClient) == new MockHttpClientFactory(new MockHttpMessageHandler(BackendDefinitionBehavior.Always)).CreateClient(expectedApiClient));

      var mockCorrelationId = Guid.NewGuid();
      var mockCorrelationContext = Mock.Of<ICorrelationContext>(x => x.CorrelationId == mockCorrelationId);

      var sut = new DfeHttpClientFactory(mockHttpClientFactory, mockCorrelationContext);

      _ = sut.CreateTramsClient();

      Mock.Get(mockHttpClientFactory).Verify(x => x.CreateClient(expectedApiClient), Times.Once);
      Mock.Get(mockHttpClientFactory).Verify(x => x.CreateClient(unexpectedApiClient), Times.Never);
   }

   [Fact]
   public void CreatingAcademisationClient_Should_Invoke_HttpClientFactory_With_CorrectClientName()
   {
      var expectedApiClient = "AcademisationClient";
      var unexpectedApiClient = "TramsClient";
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
      var mockHttpClientFactory = Mock.Of<IHttpClientFactory>(x => x.CreateClient("TramsClient") == new MockHttpClientFactory(new MockHttpMessageHandler(BackendDefinitionBehavior.Always)).CreateClient("TramsClient"));

      var mockCorrelationId = Guid.NewGuid();
      var mockCorrelationContext = Mock.Of<ICorrelationContext>(x => x.CorrelationId == mockCorrelationId);

      var sut = new DfeHttpClientFactory(mockHttpClientFactory, mockCorrelationContext);

      _ = sut.CreateTramsClient();

      Mock.Get(mockHttpClientFactory).Verify(x => x.CreateClient("TramsClient"), Times.Once);
   }
   
   [Fact]
   public void CreatingAcademisationClient_Should_Add_CorrelationId_Header()
   {
      var mockHttpClientFactory = Mock.Of<IHttpClientFactory>(x => x.CreateClient("TramsClient") == new MockHttpClientFactory(new MockHttpMessageHandler(BackendDefinitionBehavior.Always)).CreateClient("TramsClient"));

      var mockCorrelationId = Guid.NewGuid();
      var mockCorrelationContext = Mock.Of<ICorrelationContext>(x => x.CorrelationId == mockCorrelationId);

      var sut = new DfeHttpClientFactory(mockHttpClientFactory, mockCorrelationContext);

      _ = sut.CreateTramsClient();

      Mock.Get(mockHttpClientFactory).Verify(x => x.CreateClient("TramsClient"), Times.Once);
   }
}
