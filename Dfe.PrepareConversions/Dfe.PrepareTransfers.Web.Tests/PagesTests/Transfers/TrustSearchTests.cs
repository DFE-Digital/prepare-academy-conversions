using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.Pages.Transfers;
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
    public class TrustSearchTests
    {
        private readonly TrustSearchModel _subject;
        private readonly Mock<ITrusts> _trustsRepository;

         public TrustSearchTests()
        {
            _trustsRepository = new Mock<ITrusts>();

            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var pageContext = new PageContext()
            {
                ViewData = viewData
            };

            _subject = new TrustSearchModel(_trustsRepository.Object)
            {
                PageContext = pageContext,
                TempData = tempData
            };
        }

         [Fact]
         public async Task OnContinue_GivenNoTrustSelected_DisplaysError()
         {
            var searchQuery = "test trust";
            var trustId = string.Empty;

            var trusts = new List<Trust>
               {
                  new Trust { Ukprn = trustId },
                  new Trust { Ukprn = "another-test-ukprn" }
               };

            _subject.SearchQuery = searchQuery;

            _trustsRepository.Setup(r => r.SearchTrusts(searchQuery, "")).ReturnsAsync(trusts);

            await _subject.OnPostAsync(searchQuery, trustId);

            _trustsRepository.Verify(r => r.SearchTrusts(searchQuery, ""));

            Assert.Equal("Select a trust", _subject.ModelState["TrustId"].Errors.First().ErrorMessage);
         }

         [Fact]
         public async Task OnContinue_RedirectsToOutgoingTrustDetailsPage()
         {
            var searchQuery = "test trust";
            var trustId = "10060613";

            var trusts = new List<Trust>
               {
                  new Trust { Ukprn = trustId },
                  new Trust { Ukprn = "another-test-ukprn" }
               };

            _subject.SearchQuery = searchQuery;

            _trustsRepository.Setup(r => r.SearchTrusts(searchQuery, "")).ReturnsAsync(trusts);

            var response = await _subject.OnPostAsync(searchQuery, trustId);

            _trustsRepository.Verify(r => r.SearchTrusts(searchQuery, ""));

            var redirectResponse = Assert.IsType<RedirectToPageResult>(response);
            Assert.Equal("/NewTransfer/OutgoingTrustDetails", redirectResponse.PageName);
         }

         [Fact]
         public async Task GivenSearchingByString_SearchesForTrustsAndAssignsToModel()
         {
            const string searchQuery = "Trust name";
            var trustId = "10067113";

            var trusts = new List<Trust>
            {
               new Trust { Ukprn = trustId },
               new Trust { Ukprn = "another-test-ukprn" }
            };

            _trustsRepository.Setup(r => r.SearchTrusts(searchQuery, "")).ReturnsAsync(trusts);

            _subject.SearchQuery = searchQuery;

            var result = await _subject.OnGetAsync(trustId);

            _trustsRepository.Verify(r => r.SearchTrusts(searchQuery, ""));

            Assert.Equal(trusts, _subject.Trusts);
            Assert.Equal(trustId, _subject.TrustId);
         }

        [Fact]
        // Ensure query string gets bound to model when in the format ?query=search-term
        public void BindsPropertyIsPresentWithCorrectOptions()
        {
            var trustSearchModel = new TrustSearchModel(_trustsRepository.Object);
            var attribute = (BindPropertyAttribute)trustSearchModel.GetType()
                .GetProperty("SearchQuery").GetCustomAttributes(typeof(BindPropertyAttribute), false).First();

            Assert.NotNull(attribute);
            Assert.Equal("query", attribute.Name);
            Assert.True(attribute.SupportsGet);
        }

      [Fact]
      public async Task GivenChangeLink_SetChangeLinkinViewData()
      {
         await _subject.OnGetAsync("10060613", change: true);

         Assert.True((bool)_subject.ViewData["ChangeLink"]);
      }
    }
}
