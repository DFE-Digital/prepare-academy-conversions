using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models.ApplicationForm.Sections
{
	public class PreOpeningSupportGrantSectionTests
	{
		[Fact]
		public static void Constructor_WithApplication_SetFields()
		{
			var application = new Application
			{
				FundsPaidToSchoolOrTrust = "To the trust the school is joining"
			};

			var formSection = new PreOpeningSupportGrantSection(application);

			var expectedFields = new[]
			{
				new FormField("Do you want these funds paid to the school or the trust?", "To the trust the school is joining")
			};

			formSection.Heading.Should().Be("Pre-opening support grant");
			formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFields);
		}
	}
}
