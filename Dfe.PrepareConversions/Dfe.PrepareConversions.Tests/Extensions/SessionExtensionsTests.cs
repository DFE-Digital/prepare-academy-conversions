using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Tests.Customisations;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Extensions;

public class SessionExtensionsTests
{
   [Theory]
   [AutoMoqData]
   public void Should_set_object_to_session(Mock<ISession> session, Test toSet, string key)
   {
      string json = JsonSerializer.Serialize(toSet);
      byte[] expectedObj = Encoding.UTF8.GetBytes(json);

      session.Object.Set(key, toSet);

      session.Verify(m => m.Set(key, It.Is<byte[]>(actualObj => actualObj.SequenceEqual(expectedObj))), Times.Once);
   }

   [Theory]
   [AutoMoqData]
   public void Should_get_object_from_session(Mock<ISession> session, Test expectedResult, string key)
   {
      string json = JsonSerializer.Serialize(expectedResult);
      byte[] expectedObj = Encoding.UTF8.GetBytes(json);

      session.Setup(m => m.TryGetValue(It.IsAny<string>(), out expectedObj));

      Test actualResult = session.Object.Get<Test>(key);

      actualResult.Should().BeEquivalentTo(expectedResult);
   }

   public class Test
   {
      public string TestProperty { get; set; }
   }
}
