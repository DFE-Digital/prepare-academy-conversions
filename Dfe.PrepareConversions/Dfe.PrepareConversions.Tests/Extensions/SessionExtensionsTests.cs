using Dfe.PrepareConversions.Data.Tests.AutoFixture;
using Dfe.PrepareConversions.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Extensions
{
	public class SessionExtensionsTests
	{
		[Theory, AutoMoqData]
		public void Should_set_object_to_session(Mock<ISession> session, Test toSet, string key)
		{			
			var json = JsonSerializer.Serialize(toSet);
			var expectedObj = Encoding.UTF8.GetBytes(json);

			session.Object.Set(key, toSet);
			
			session.Verify(m => m.Set(key, It.Is<byte[]>(actualObj => Enumerable.SequenceEqual(actualObj, expectedObj))), Times.Once);
		}

		[Theory, AutoMoqData]
		public void Should_get_object_from_session(Mock<ISession> session, Test expectedResult, string key)
		{
			var json = JsonSerializer.Serialize(expectedResult);
			var expectedObj = Encoding.UTF8.GetBytes(json);

			session.Setup(m => m.TryGetValue(It.IsAny<string>(), out expectedObj));

			var actualResult = session.Object.Get<Test>(key);

			actualResult.Should().BeEquivalentTo(expectedResult);
		}

		public class Test
		{
			public string TestProperty { get; set; }
		}
	}
}
