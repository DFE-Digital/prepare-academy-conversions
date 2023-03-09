using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Models.ApplicationForm;
using Dfe.PrepareConversions.Models.ApplicationForm.Sections;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Models.ApplicationForm.Sections
{
	public class PreOpeningSupportGrantSectionTests
	{
		[Fact]
		public static void Constructor_WithApplication_SetFields()
		{
			var application = new ApplyingSchool
			{
				SchoolSupportGrantFundsPaidTo = "School"
			};

			var formSection = new PreOpeningSupportGrantSection(application);

			var expectedFields = new[]
			{
				new FormField("Do you want these funds paid to the school or the trust?", application.SchoolSupportGrantFundsPaidTo)
			};

			formSection.Heading.Should().Be("Pre-opening support grant");
			formSection.SubSections.First().Fields.Should().BeEquivalentTo(expectedFields);
		}
	}
}
