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
			var project = new Project{
				Id = 1,
				School = new School{Name = "School Name", LocalAuthority = "Local Authority", URN = "URN"},
				Trust = new Trust{Name = "Trust Name"},
				ApplicationReceivedDate = new DateTime(2020, 12, 12),
				AssignedDate = new DateTime(2021, 04, 02),
				Phase = ProjectPhase.PreHTB
			};
			
			var viewModel = new ProjectViewModel(project);
			viewModel.Id.Should().Be("1");
			viewModel.TrustName.Should().Be("Trust Name");
			viewModel.SchoolName.Should().Be("School Name");
			viewModel.SchoolURN.Should().Be("URN");
			viewModel.LocalAuthority.Should().Be("Local Authority"); 
			viewModel.ApplicationReceivedDate.Should().Be("12 December 2020");
			viewModel.AssignedDate.Should().Be("02 April 2021");
			viewModel.Phase.Should().Be("Pre HTB");
		}

		[Fact]
		public void Constructor_WithPostHTBProject_SetsPhaseToPostHTB()
		{
			var project = new Project
			{
				Id = 1,
				School = new School {Name = "School Name", LocalAuthority = "Local Authority", URN = "URN"},
				Trust = new Trust {Name = "Trust Name"},
				ApplicationReceivedDate = new DateTime(2020, 12, 12),
				AssignedDate = new DateTime(2021, 04, 02),
				Phase = ProjectPhase.PostHTB
			};

			var viewModel = new ProjectViewModel(project);
			viewModel.Phase.Should().Be("Post HTB");
		}
	}
}