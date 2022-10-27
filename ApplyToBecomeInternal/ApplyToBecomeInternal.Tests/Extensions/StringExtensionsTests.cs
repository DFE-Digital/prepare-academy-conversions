using ApplyToBecomeInternal.Extensions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Extensions
{
	public class StringExtensionsTests
	{
		private string _fullCaps = "THIS IS FULL CAPS";
		private string _lowerCase = "this is lower case";

		[Fact]
		public void ToSentenceCase_WithFullCaps_ReturnsCorrectly() => _fullCaps.SentenceCase().Should().Be("This is full caps");

		[Fact]
		public void ToSentenceCase_WithLowerCase_ReturnsCorrectly() => _lowerCase.SentenceCase().Should().Be("This is lower case");
	}
}
