using System.Linq;
using System.Threading.Tasks; 
using Dfe.PrepareTransfers.Data.Models.KeyStagePerformance;
using Dfe.PrepareTransfers.Data.Models.Projects;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Models.Forms;
using Dfe.PrepareTransfers.Web.Pages.TaskList.KeyStage5Performance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using Xunit;

namespace Dfe.PrepareTransfers.Web.Tests.PagesTests.TaskList.HtbDocument
{
   public class KeyStage5PerformanceTests : BaseTests
   {
      private readonly KeyStage5Performance _subject;

      public KeyStage5PerformanceTests()
      {
         FoundInformationForProject.OutgoingAcademies.First().EducationPerformance = new EducationPerformance
         {
            KeyStage5Performance = [new KeyStage5 { Year = "2019" }],
            KeyStage5AdditionalInformation = "some additional info"
         };

         _subject = new KeyStage5Performance(GetInformationForProject.Object, ProjectRepository.Object)
         {
            Urn = ProjectUrn0001,
            AcademyUkprn = AcademyUkprn,
            AdditionalInformationViewModel = new AdditionalInformationViewModel()
         };
      }

      public class OnGetAsyncTests : KeyStage5PerformanceTests
      {
         [Theory]
         [InlineData("1234")]
         [InlineData("4321")]
         public async Task OnGet_GetInformationForProjectId(string id)
         {
            _subject.Urn = id;
            await _subject.OnGetAsync();

            GetInformationForProject.Verify(s => s.Execute(id));
         }

         [Fact]
         public async Task OnGet_AssignsCorrectValuesToViewModel()
         {
            var result = await _subject.OnGetAsync();

            Assert.IsType<PageResult>(result);
            Assert.Equal(FoundInformationForProject.Project.Urn, _subject.Urn);
            Assert.Equal(FoundInformationForProject.OutgoingAcademies.First(a => a.Ukprn == AcademyUkprn).EducationPerformance, _subject.EducationPerformance);
            Assert.Equal(FoundInformationForProject.OutgoingAcademies.First(a => a.Ukprn == AcademyUkprn).Ukprn, _subject.AcademyUkprn);

         }

         [Fact]
         public async Task GivenReturnToPreview_UpdatesTheViewModel()
         {
            _subject.ReturnToPreview = true;
            await _subject.OnGetAsync();

            Assert.True(_subject.ReturnToPreview);
         }
      }

      public class OnPostAsyncTests : KeyStage5PerformanceTests
      {
         [Fact]
         public async Task GivenUrn_FetchesProjectFromTheRepository()
         {
            await _subject.OnPostAsync();
            ProjectRepository.Verify(r => r.GetByUrn(ProjectUrn0001), Times.Once);
         }

         [Fact]
         public async Task GivenAdditionalInformation_UpdatesTheProjectModel()
         {
            const string additionalInfo = "some additional info";
            _subject.AdditionalInformationViewModel.AdditionalInformation = additionalInfo;
            var response = await _subject.OnPostAsync();

            var redirectToPageResponse = Assert.IsType<RedirectToPageResult>(response);
            Assert.Equal("KeyStage5Performance", redirectToPageResponse.PageName);
            Assert.Null(redirectToPageResponse.PageHandler);
            Assert.Equal(additionalInfo,
                FoundProjectFromRepo.TransferringAcademies.First(a => a.OutgoingAcademyUkprn == AcademyUkprn)
                    .KeyStage5PerformanceAdditionalInformation);
         }

         [Fact]
         public async Task GivenAdditionalInformation_UpdatesTheProjectCorrectly()
         {
            const string additionalInfo = "some additional info";
            _subject.AdditionalInformationViewModel.AdditionalInformation = additionalInfo;
            await _subject.OnPostAsync();
            ProjectRepository.Verify(r => r.UpdateAcademy(It.Is<string>(x => x == _subject.Urn), It.Is<TransferringAcademy>(academy => academy.OutgoingAcademyUkprn == _subject.AcademyUkprn && academy.KeyStage5PerformanceAdditionalInformation == additionalInfo)
            ));
         }

         [Fact]
         public async Task GivenReturnToPreview_RedirectsToThePreviewPage()
         {
            _subject.ReturnToPreview = true;
            var response = await _subject.OnPostAsync();

            var redirectResponse = Assert.IsType<RedirectToPageResult>(response);
            Assert.Equal(Links.HeadteacherBoard.Preview.PageName, redirectResponse.PageName);
            Assert.Equal(_subject.Urn, redirectResponse.RouteValues["Urn"]);
         }
      }
   }
}