using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using Moq;
using Xunit;
using FluentAssertions;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareConversions.Areas.Transfers.Pages.Projects.PublicSectorEqualityDuty;
using Dfe.PrepareTransfers.Web.Services.Responses;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Data.Models.Projects;

namespace Dfe.PrepareTransfers.Web.Tests.PagesTests.PublicEqualityDutyImpact.Transfer
{
    public class TaskModelTests
    {
      private readonly Mock<IGetInformationForProject> _getInformationForProject;
      private readonly Mock<IProjects> _projectsRepository;
      private readonly Mock<ErrorService> _errorService;
      private readonly TaskModel _subject;

      public TaskModelTests()
      {
         _getInformationForProject = new Mock<IGetInformationForProject>();
         _projectsRepository = new Mock<IProjects>();
         _errorService = new Mock<ErrorService>();
         _subject = new TaskModel(_getInformationForProject.Object, _projectsRepository.Object, _errorService.Object);
      }


      //[Fact(Skip = "Will look into this shortly")]
      //public async Task OnGet_Returns_Page()
      //{
      //   var projectResponse = new GetInformationForProjectResponse
      //   {
      //      Project = {
      //         Urn = "12345",
      //         TransferringAcademies = 
      //         [
      //            new TransferringAcademy { }
      //         ],
      //         PublicEqualityDutyImpact = "Likely",
      //         PublicEqualityDutyReduceImpactReason = "Some reason",
      //         PublicEqualityDutySectionComplete = false
      //      },
      //      OutgoingAcademies = []
      //   };

      //   _getInformationForProject.Setup(s => s.Execute(projectResponse.Project.Urn)).ReturnsAsync(projectResponse);

      //   var result = await _subject.OnGetAsync(projectResponse.Project.Urn);

      //   Assert.IsType<PageResult>(result);

      //   _getInformationForProject.Verify(r => r.Execute(projectResponse.Project.Urn), Times.Once);

      //   Assert.Equal(projectResponse.Project.Urn, _subject.Urn);
      //   Assert.Equal(projectResponse.Project.OutgoingTrustName, _subject.OutgoingTrustName);
      //   Assert.Equal(projectResponse.Project.PublicEqualityDutyImpact, _subject.Impact);
      //   Assert.Equal(projectResponse.Project.PublicEqualityDutyReduceImpactReason, _subject.ReduceImpactReason);
      //   Assert.Equal(projectResponse.Project.PublicEqualityDutySectionComplete, _subject.SectionComplete);
      //}
   }
}
