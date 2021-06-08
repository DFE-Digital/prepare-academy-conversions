using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.GenerateHTBTemplate;
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
				Phase = ProjectPhase.PreHTB,
				ProjectDocuments = new[]
				{
					new DocumentDetails
					{
						Name = "Wilfreds-Dynamics-HTB-temp-13-March-2021.docx",
						Type = "Word document",
						Size = "267kb"
					},
					new DocumentDetails
					{
						Name = "Dynamics-trust-temp-13March2021.docx",
						Type = "Word document",
						Size = "112kb"
					},
					new DocumentDetails
					{
						Name = "Dynamics-trust-update-May-2021.docx",
						Type = "Word document",
						Size = "854kb"
					}
				},
				Rationale = new Rationale
				{
					ProjectRationale = "Rationale for the project",
					TrustRationale = "Rationale for the trust"
				}
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
			viewModel.ProjectDocuments.Should().HaveCount(3);
			viewModel.ProjectRationale.Should().Be("Rationale for the project");
			viewModel.TrustRationale.Should().Be("Rationale for the trust");
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
				Phase = ProjectPhase.PostHTB,
				Rationale = new Rationale {ProjectRationale = "Rationale for the project", TrustRationale = "Rationale for the trust"}
			};

			var viewModel = new ProjectViewModel(project);
			viewModel.Phase.Should().Be("Post HTB");
		}
	}
}
