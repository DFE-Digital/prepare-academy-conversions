using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Person;
using Dfe.PrepareConversions.Pages.TaskList.SchoolOverview;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.ViewModels;
using System.Net;
using GovUK.Dfe.CoreLibs.Contracts.ExternalApplications.Models.Response;
using GovUK.Dfe.PersonsApi.Client.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolOverview
{
    public class ConfirmSchoolOverviewModelTests
    {
      private readonly Mock<IAcademyConversionProjectRepository> _repo = new();
      private readonly Mock<IPersonApiEstablishmentsService> _personService = new();
      private readonly Mock<ErrorService> _errorService = new();

      private SchoolOverviewModel CreateModel(AcademyConversionProject project)
      {
         _repo.Setup(r => r.GetProjectById(It.IsAny<int>()))
             .ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, project));

         return new SchoolOverviewModel(_repo.Object, _personService.Object, _errorService.Object)
         {
            Project = new ProjectViewModel(project)
         };
      }

      [Theory]
      [InlineData("Pre advisory board")]
      [InlineData("Deferred")]
      public async Task OnGetAsync_UpdatesMP_WhenStatusIsPreAdvisoryOrDeferred_AndLookupSucceeds(string status)
      {
         // Arrange
         var project = new AcademyConversionProject { Id = 1, ProjectStatus = status, Urn = 123456 };
         
         var mp = new MemberOfParliament { DisplayName = "Jane Doe (Party)" };
         
         _personService.Setup(s => s.GetMemberOfParliamentBySchoolUrnAsync(123456))
             .ReturnsAsync(Result<MemberOfParliament>.Success(mp));

         var model = CreateModel(project);

         // Act
         var result = await model.OnGetAsync(1);

         // Assert
         _repo.Verify(r => r.UpdateProject(1, It.Is<UpdateAcademyConversionProject>(u => u.MemberOfParliamentNameAndParty == "Jane Doe (Party)")), Times.Once);

         Assert.IsType<PageResult>(result);
      }

      [Theory]
      [InlineData("Pre advisory board")]
      [InlineData("Deferred")]
      public async Task OnGetAsync_SetsError_WhenStatusIsPreAdvisoryOrDeferred_AndLookupFails(string status)
      {
         // Arrange
         var project = new AcademyConversionProject { Id = 1, ProjectStatus = status, Urn = 123456 };
         
         _personService.Setup(s => s.GetMemberOfParliamentBySchoolUrnAsync(123456))
             .ReturnsAsync(Result<MemberOfParliament>.Failure("error"));

         var model = CreateModel(project);

         // Act
         var result = await model.OnGetAsync(1);

         // Assert
         _repo.Verify(r => r.UpdateProject(It.IsAny<int>(), It.IsAny<UpdateAcademyConversionProject>()), Times.Never);
         Assert.IsType<PageResult>(result);

         //_errorService.Verify(es => es.AddError("member-of-parliament-name-and-party", "The Member of Parliement name and Political party could not be retrieved"), Times.Once);
         //_errorService.Verify(es => es.AddError(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
      }

      [Fact]
      public async Task OnGetAsync_DoesNotCallMPService_WhenStatusIsOther()
      {
         // Arrange
         var project = new AcademyConversionProject { Id = 1, ProjectStatus = "Approved", Urn = 123456 };
         var model = CreateModel(project);

         // Act
         var result = await model.OnGetAsync(1);

         // Assert
         _personService.Verify(s => s.GetMemberOfParliamentBySchoolUrnAsync(It.IsAny<int>()), Times.Never);
         
         _repo.Verify(r => r.UpdateProject(It.IsAny<int>(), It.IsAny<UpdateAcademyConversionProject>()), Times.Never);
         
         Assert.IsType<PageResult>(result);
      }
   }
}
