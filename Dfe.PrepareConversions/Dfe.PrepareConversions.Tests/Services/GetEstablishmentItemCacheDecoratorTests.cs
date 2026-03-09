using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Services;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Services;

public class GetEstablishmentItemCacheDecoratorTests
{
   [Fact]
   public async Task GetEstablishmentByUkprn_CachesResultInHttpContextItems()
   {
      // Arrange
      var ukprn = "12345";
      var expected = new EstablishmentDto();

      var getEstablishmentMock = new Mock<IGetEstablishment>();
      getEstablishmentMock.Setup(x => x.GetEstablishmentByUkprn(ukprn))
          .ReturnsAsync(expected);

      var httpContext = new DefaultHttpContext();
      var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
      httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

      var decorator = new GetEstablishmentItemCacheDecorator(getEstablishmentMock.Object, httpContextAccessorMock.Object);

      // Act
      var result1 = await decorator.GetEstablishmentByUkprn(ukprn);
      var result2 = await decorator.GetEstablishmentByUkprn(ukprn);

      // Assert
      Assert.Same(expected, result1);
      Assert.Same(expected, result2);
      getEstablishmentMock.Verify(x => x.GetEstablishmentByUkprn(ukprn), Times.Once); // Only called once due to caching
   }
}
