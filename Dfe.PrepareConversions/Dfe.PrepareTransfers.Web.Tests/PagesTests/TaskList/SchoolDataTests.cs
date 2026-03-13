using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Data.Models.KeyStagePerformance;
using Dfe.PrepareTransfers.Web.Pages.TaskList;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareTransfers.Web.Tests.PagesTests.TaskList
{
   public class SchoolDataTests : BaseTests
   {
      private readonly SchoolData _subject;
      private Mock<IAcademies> Academies { get; } = new();
      private string SchoolAcademyUrn = "academyurn";

      private Mock<IEducationPerformance> ProjectRepositoryEducationPerformance { get; } = new();

      public SchoolDataTests()
      {
         Academies.Setup(s => s.GetAcademyByUkprn(It.IsAny<string>()))
            .ReturnsAsync(new Academy { Urn = SchoolAcademyUrn });
         ProjectRepositoryEducationPerformance.Setup(S => S.GetByAcademyUrn(It.IsAny<string>()))
            .ReturnsAsync(new RepositoryResult<EducationPerformance> { Result = new EducationPerformance() });

         _subject = new SchoolData(Academies.Object, ProjectRepository.Object,
            ProjectRepositoryEducationPerformance.Object) { Urn = ProjectUrn0001, AcademyUkprn = "academyukprn" };
      }

      [Fact]
      public async Task GivenUrn_FetchesProjectFromTheRepository()
      {
         await _subject.OnGetAsync();
         ProjectRepository.Verify(r => r.GetByUrn(ProjectUrn0001), Times.Once);
      }

      [Fact]
      public async Task GivenUrn_AssignsModelToThePage()
      {
         var response = await _subject.OnGetAsync();
         response.Should().BeOfType<PageResult>();
         _subject.Urn.Should().Be(ProjectUrn0001);
      }

      [Fact]
      public async Task GivenAcademyUkprn_FetchesAcademyFromTheRepository()
      {
         await _subject.OnGetAsync();
         Academies.Verify(r => r.GetAcademyByUkprn(_subject.AcademyUkprn), Times.Once);
      }

      [Fact]
      public async Task OnGet_FetchesPerformanceDataFromTheRepository()
      {
         await _subject.OnGetAsync();
         ProjectRepositoryEducationPerformance.Verify(r => r.GetByAcademyUrn(AcademyUrn), Times.Once);
      }

      [Fact]
      public async Task OnGet_SetsAcademyUrn()
      {
         await _subject.OnGetAsync();
         _subject.AcademyUrn.Should().Be(SchoolAcademyUrn);
      }
   }
}