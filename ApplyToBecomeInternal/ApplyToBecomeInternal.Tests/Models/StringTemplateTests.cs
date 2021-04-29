using ApplyToBecomeInternal.Models;
using FluentAssertions;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models
{
	public class StringTemplateTests
	{
        public static TheoryData<string, (string, string)[], string> TestData = new TheoryData<string, (string, string)[], string>
        {
			{"template {s}", new[]{("s", "string")}, "template string"},
			{"{t} {s}", new[]{("t", "template"), ("s", "string")}, "template string"},
			{"template string", new (string, string)[0], "template string"},
		};

		[Theory]
		[MemberData(nameof(TestData))]
		public void ToString_WithTemplateAndData_ReturnsPopulatedString(string template, (string key, string value)[] data, string expected)
		{
			var stringTemplate = new StringTemplate(template);
			foreach ((string key, string value) in data)
				stringTemplate.Set(key, value);
			stringTemplate.ToString().Should().Be(expected);
		}
	}
}