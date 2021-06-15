using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.ViewModels;
using FluentAssertions;
using System;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models
{
	public class ProjectViewModelTests
	{
		[Fact]
		public void Constructor_WithProject_SetsProperties()
		{
			var project = new AcademyConversionProject {
				Id = 1,
				SchoolName = "School Name", 
				LocalAuthority = "Local Authority", 
				Urn = 12345,
				ApplicationReceivedDate = new DateTime(2020, 12, 12),
				AssignedDate = new DateTime(2021, 04, 02),
				ProjectStatus = "Pre HTB",
				RationaleForProject = "Rationale for the project",
				RationaleForTrust = "Rationale for the trust"
			};

			var viewModel = new ProjectViewModel(project);
			viewModel.Id.Should().Be("1");
			viewModel.SchoolName.Should().Be("School Name");
			viewModel.SchoolURN.Should().Be("12345");
			viewModel.LocalAuthority.Should().Be("Local Authority");
			viewModel.ApplicationReceivedDate.Should().Be("12 December 2020");
			viewModel.AssignedDate.Should().Be("02 April 2021");
			viewModel.Phase.Should().Be("Pre HTB");
			viewModel.RationaleForProject.Should().Be("Rationale for the project");
			viewModel.RationaleForTrust.Should().Be("Rationale for the trust");
		}
	}
}
