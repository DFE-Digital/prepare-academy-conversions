using System.Linq;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Data.Models.KeyStagePerformance;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Models.Forms;
using Dfe.PrepareTransfers.Web.Pages.TaskList.KeyStage4Performance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using Xunit;

namespace Dfe.PrepareTransfers.Web.Tests.PagesTests.TaskList.HtbDocument
{
    public class KeyStage4PerformanceTests : BaseTests
    {
        private readonly KeyStage4Performance _subject;

        public KeyStage4PerformanceTests()
        {
            FoundInformationForProject.OutgoingAcademies.First().EducationPerformance = new EducationPerformance
            {
                KeyStage4Performance =
                {
                    new KeyStage4
                    {
                        Year = "2019-2020",
                        SipNumberofpupilsprogress8 = new DisadvantagedPupilsResult
                        {
                            NotDisadvantaged = "20.5",
                            Disadvantaged = "10.5"
                        }
                    },
                    new KeyStage4
                    {
                        Year = "2018-2019",
                        SipNumberofpupilsprogress8 = new DisadvantagedPupilsResult
                        {
                            NotDisadvantaged = "40.8",
                            Disadvantaged = "30.4"
                        }
                    }
                },
                KeyStage4AdditionalInformation = "some additional info"
            };

            _subject = new KeyStage4Performance(GetInformationForProject.Object, ProjectRepository.Object)
            {
                Urn = ProjectUrn0001,
                AcademyUkprn = AcademyUkprn,
                AdditionalInformationViewModel = new AdditionalInformationViewModel()
            };
        }

        public class OnGetAsyncTests : KeyStage4PerformanceTests
        {
            [Fact]
            public async Task GivenUrn_FetchesProjectFromTheRepository()
            {
                await _subject.OnGetAsync();

                GetInformationForProject.Verify(r => r.Execute(ProjectUrn0001), Times.Once);
            }

            [Fact]
            public async Task GivenExistingProject_AssignsItToThePageModel()
            {
                var response = await _subject.OnGetAsync();

                Assert.IsType<PageResult>(response);
                Assert.Equal(ProjectUrn0001, _subject.Urn);
                Assert.Equal(AcademyUrn, _subject.OutgoingAcademyUrn);
                Assert.Equal(2, _subject.EducationPerformance.KeyStage4Performance.Count);
                Assert.Equal("2019-2020", _subject.EducationPerformance.KeyStage4Performance[0].Year);
                Assert.Equal("2018-2019", _subject.EducationPerformance.KeyStage4Performance[1].Year);
            }

            [Fact]
            public async Task GivenReturnToPreview_UpdatesTheViewModel()
            {
                _subject.ReturnToPreview = true;
                await _subject.OnGetAsync();

                Assert.True(_subject.ReturnToPreview);
            }
        }

        public class OnPostAsyncTests : KeyStage4PerformanceTests
        {
            [Fact]
            public async Task GivenUrn_FetchesProjectFromTheRepository()
            {
                await _subject.OnPostAsync();

                ProjectRepository.Verify(r => r.GetByUrn(ProjectUrn0001), Times.Once);
            }

            [Fact]
            public async Task GivenReturnToPreview_RedirectsToThePreviewPage()
            {
                _subject.ReturnToPreview = true;
                var response = await _subject.OnPostAsync();

                var redirectResponse = Assert.IsType<RedirectToPageResult>(response);
                Assert.Equal(Links.HeadteacherBoard.Preview.PageName, redirectResponse.PageName);
                Assert.Equal(ProjectUrn0001, redirectResponse.RouteValues["urn"]);
            }
        }
    }
}