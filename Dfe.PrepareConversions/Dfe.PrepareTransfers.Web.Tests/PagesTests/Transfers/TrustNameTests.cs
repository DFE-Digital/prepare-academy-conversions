using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.Pages.NewTransfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareTransfers.Web.Tests.PagesTests.Transfers
{
    public class TrustNameTests
    {
        private readonly PageContext _pageContext;
        private readonly TempDataDictionary _tempData;
        private readonly TrustNameModel _subject;
         private readonly Mock<ITrusts> _trustsRepository;

         public TrustNameTests()
         {
            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            _tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _pageContext = new PageContext()
            {
                ViewData = viewData
            };

            _trustsRepository = new Mock<ITrusts>();
            _subject = new TrustNameModel(_trustsRepository.Object)
            {
                PageContext = _pageContext,
                TempData = _tempData
            };
         }

         [Fact]
         public async Task OnContinue_GivenNoTrustName_DisplaysError()
         {
            var searchQuery = "";

            var trusts = new List<Trust>
            {
               new Trust { Ukprn = "test-ukprn" },
               new Trust { Ukprn = "another-test-ukprn" }
            };

            _subject.SearchQuery = searchQuery;

            _trustsRepository.Setup(r => r.SearchTrusts(searchQuery, "")).ReturnsAsync(trusts);

            await _subject.OnPostAsync();

            _trustsRepository.Verify(r => r.SearchTrusts(searchQuery, ""));

            Assert.Equal("Enter the outgoing trust name", _subject.ModelState["SearchQuery"].Errors.First().ErrorMessage);
         }

         [Fact]
         public async Task OnContinue_GivenSearchReturnsNoTrusts_DisplaysError()
         {
            var searchQuery = "Test Trust";
            var trusts = new List<Trust>();

            _subject.SearchQuery = searchQuery;

            _trustsRepository.Setup(r => r.SearchTrusts(searchQuery, "")).ReturnsAsync(trusts);

            await _subject.OnPostAsync();

            _trustsRepository.Verify(r => r.SearchTrusts(searchQuery, ""));

            Assert.Equal("We could not find any trusts matching your search criteria", _subject.ModelState["SearchQuery"].Errors.First().ErrorMessage);
         }

         [Fact]
         public async Task OnContinue_RedirectsToTrustSearchPage()
         {
            const string searchQuery = "Trust name";

            var trusts = new List<Trust>
            {
               new Trust { Ukprn = "test-ukprn" },
               new Trust { Ukprn = "another-test-ukprn" }
            };

            _subject.SearchQuery = searchQuery;

            _trustsRepository.Setup(r => r.SearchTrusts(searchQuery, "")).ReturnsAsync(trusts);

            var response = await _subject.OnPostAsync();

            _trustsRepository.Verify(r => r.SearchTrusts(searchQuery, ""));

            var redirectResponse = Assert.IsType<RedirectToPageResult>(response);
            Assert.Equal("/NewTransfer/TrustSearch", redirectResponse.PageName);
         }

        [Fact]
        // Ensure query string gets bound to model when in the format ?query=search-term
        public void BindsPropertyIsPresentWithCorrectOptions()
        {
            var trustNameModel = new TrustNameModel(_trustsRepository.Object);
            var attribute = (BindPropertyAttribute)trustNameModel.GetType()
                .GetProperty("SearchQuery").GetCustomAttributes(typeof(BindPropertyAttribute), false).First();

            Assert.NotNull(attribute);
            Assert.Equal("query", attribute.Name);
            Assert.True(attribute.SupportsGet);
        }

        [Fact]
        public void GivenChangeLink_SetChangeLinkinViewData()
        {
            _subject.OnGet(change: true);

            Assert.Equal(true, _subject.ViewData["ChangeLink"]);
        }
    }
}
