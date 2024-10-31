using Dfe.PrepareTransfers.Web.Pages.Projects.AcademyAndTrustInformation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Session;
using Moq;
using Xunit;

namespace Dfe.PrepareTransfers.Web.Tests.PagesTests.Projects.AcademyAndTrustInformation
{
   public class ProjectTests : BaseTests
   {
      private readonly PageContext _pageContext;
      private readonly Mock<ISession> _session;
      private readonly IncomingTrustNameModel _subject;

      protected ProjectTests()
      {
        _session = new Mock<ISession>(); 
         var sessionFeature = new SessionFeature { Session = _session.Object };
         var httpContext = new DefaultHttpContext();
         httpContext.Features.Set<ISessionFeature>(sessionFeature);

         _pageContext = new PageContext()
         {
            HttpContext = httpContext
         };

         
         _subject = new IncomingTrustNameModel(ProjectRepository.Object, _session.Object) { Urn = ProjectUrn0001, PageContext = _pageContext };
      }
      public class OnPostAsync : ProjectTests
      {
         public OnPostAsync()
         {
            _subject.Urn = ProjectUrn0001;
         }

         [Fact]
         public async void GivenErrorInModelState_ReturnsCorrectPage()
         {
            _subject.ModelState.AddModelError(nameof(_subject.IncomingTrustName), "error");
             var result = await _subject.OnPostAsync();

            ProjectRepository.Verify(r => r.UpdateIncomingTrust(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), string.Empty), Times.Never);

            Assert.IsType<PageResult>(result);
         }

         [Fact]
         public async void GivenUrnAndProject_UpdatesTheProject()
         {
            _subject.IncomingTrustName = "New Project Name";
            await _subject.OnPostAsync();

            ProjectRepository.Verify(r =>
                    r.UpdateIncomingTrust(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), string.Empty),
                Times.Once);
         }
      }

   }

}