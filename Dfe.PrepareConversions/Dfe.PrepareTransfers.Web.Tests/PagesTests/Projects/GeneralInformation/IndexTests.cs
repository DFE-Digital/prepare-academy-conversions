using AutoFixture;
using Dfe.PrepareConversions.Data.Services.Person;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.Pages.Projects.GeneralInformation;
using GovUK.Dfe.CoreLibs.Contracts.ExternalApplications.Models.Response;
using GovUK.Dfe.PersonsApi.Client.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareTransfers.Web.Tests.PagesTests.Projects.GeneralInformation
{
   public class IndexTests : BaseTests
   {
      private readonly Index _subject;
      private readonly Mock<IPersonApiEstablishmentsService> _personApiEstablishmentsService = new();
      private readonly Mock<ErrorService> _errorService = new();
      private readonly Academy _outgoingAcademy;

      public IndexTests()
      {
         var expectedMp = new MemberOfParliament
         {
            DisplayName = "MP Name",
            ConstituencyPartyName = "MP Party"
         };
         var mpResult = new Result<MemberOfParliament>(expectedMp, true, null, null);

         _personApiEstablishmentsService
            .Setup(x => x.GetMemberOfParliamentBySchoolUrnAsync(It.IsAny<int>()))
            .ReturnsAsync(mpResult);

         _subject = new Index(GetInformationForProject.Object, _personApiEstablishmentsService.Object, ProjectRepository.Object, _errorService.Object)
         {
            AcademyUkprn = AcademyUkprn
         };

         var ukprn = "7689";
         var fixture = new Fixture();
         var outgoingAcademy = fixture.Create<Academy>();
         outgoingAcademy.Ukprn = ukprn;
         outgoingAcademy.Urn = "123456";
         outgoingAcademy.LastChangedDate = "22-10-2021";
         FoundInformationForProject.OutgoingAcademies = [outgoingAcademy];
         FoundInformationForProject.Project.TransferringAcademies.First().OutgoingAcademyUkprn = ukprn;
         _subject.AcademyUkprn = ukprn;
         _outgoingAcademy = outgoingAcademy;
      }

      [Fact]
      public async void GivenUrn_FetchesProjectFromTheRepository()
      {
         await _subject.OnGetAsync(ProjectUrn0001);

         GetInformationForProject.Verify(r => r.Execute(ProjectUrn0001), Times.Once);
      }

      [Fact]
      public async Task GivenExistingAcademy_AssignsTheAcademyToTheViewModel()
      {
         var response = await _subject.OnGetAsync(ProjectUrn0001);

         var expectedGeneralInformation = FoundInformationForProject.OutgoingAcademies.First().GeneralInformation;
         Assert.IsType<PageResult>(response);
         Assert.Equal(_outgoingAcademy.Name, _subject.AcademyName);
         Assert.Equal(expectedGeneralInformation.SchoolPhase, _subject.SchoolPhase);
      }
   }
}