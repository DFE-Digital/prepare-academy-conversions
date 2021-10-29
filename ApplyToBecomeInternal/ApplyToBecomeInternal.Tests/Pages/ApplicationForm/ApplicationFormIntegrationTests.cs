using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages
{
	public class ApplicationFormIntegrationTests : BaseIntegrationTests
	{
		public ApplicationFormIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		[Fact]
		public async void The_project_template_link_is_present()
		{
			var projects = AddGetProjects().ToList();
			var firstProject = AddGetProject(p => p.Id = projects.First().Id);

			await OpenUrlAsync($"/school-application-form/{firstProject.Id}");			

			var pageItem = Document.QuerySelector($"#application-form-link");
			pageItem.TextContent.Should().Be("Open school application form in a new tab");
			pageItem.BaseUri.Should().Be($"http://localhost/school-application-form/{firstProject.Id}");
		}
	}
}
