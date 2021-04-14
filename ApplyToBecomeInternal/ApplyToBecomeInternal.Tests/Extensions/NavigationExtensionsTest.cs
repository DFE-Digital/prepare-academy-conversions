using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models.Shared;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Extensions
{
	public class NavigationExtensionsTest
	{

		[Theory]
		[InlineData(NavigationContent.ProjectsList, "Back to all conversion projects", "/projectlist")]
		public void CustomNavigationAttribute_NavigationEnum_ReturnAttributeValues(NavigationContent navigation, string expectedCopy, string expectedUrl)
		{
			var actual = NavigationExtensions.GetAttribute<NavigationAttribute>(navigation);
			actual.Content.Should().Be(expectedCopy);
			actual.Url.Should().Be(expectedUrl);
		}
	}
}
