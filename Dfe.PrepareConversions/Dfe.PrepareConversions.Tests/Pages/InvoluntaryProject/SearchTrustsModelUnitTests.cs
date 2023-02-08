using AutoFixture.Xunit2;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Data.Tests.AutoFixture;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Pages.InvoluntaryProject;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.InvoluntaryProject
{
   public class SearchTrustModelUnitTests
	{
		[Theory, AutoMoqData]
		public async Task OnGetSearch_ReturnsTrustNames([Frozen] Mock<ITrustsRespository> trustsRepository, TrustsResponse trusts,
		   Mock<ISession> session)
		{
			// Arrange
			var sut = new SearchTrustModel(trustsRepository.Object, session.Object, new ErrorService());
			trustsRepository.Setup(m => m.SearchTrusts(It.IsAny<string>())).ReturnsAsync(trusts);

			// Act
			var result = await sut.OnGetSearch("name");
			var json = ExtractType<List<SearchResponse>>(Assert.IsType<JsonResult>(result));

			// Assert
			Assert.Equal($"{trusts.Data[0].GroupName.ToTitleCase()} ({trusts.Data[0].Ukprn})", json[0].value);
			Assert.Equal($"{trusts.Data[1].GroupName.ToTitleCase()} ({trusts.Data[1].Ukprn})", json[1].value);
			Assert.Equal($"{trusts.Data[2].GroupName.ToTitleCase()} ({trusts.Data[2].Ukprn})", json[2].value);
		}

		[Theory, AutoMoqData]
		public async Task OnGetSearch_ReturnsSuggestion([Frozen] Mock<ITrustsRespository> trustsRepository, Mock<ISession> session)
		{
			// Arrange
			var sut = new SearchTrustModel(trustsRepository.Object, session.Object, new ErrorService());
			var trust = new Trust { GroupName = "bristol", Ukprn = "100", CompaniesHouseNumber = "100" };
			trustsRepository.Setup(m => m.SearchTrusts(It.IsAny<string>())).ReturnsAsync(
			   new TrustsResponse { Data = new List<Trust> { trust } });

			// Act
			var result = await sut.OnGetSearch(trust.GroupName);
			var json = ExtractType<List<SearchResponse>>(Assert.IsType<JsonResult>(result));

			// Assert
			Assert.Equal($"<strong>Bristol</strong> (100)\r\n\t\t\t\t\t\t\t\t\t<br />\r\n\t\t\t\t\t\t\t\t\tCompanies House number: {trust.CompaniesHouseNumber}",
			   json[0].suggestion);
		}

		[Theory, AutoMoqData]
		public async Task OnGetSearch_ReturnsSuggestion_WhenUkprnIsIncludedInSearch([Frozen] Mock<ITrustsRespository> trustsRepository, Mock<ISession> session)
		{
			// Arrange
			var sut = new SearchTrustModel(trustsRepository.Object, session.Object, new ErrorService());
			var trust = new Trust { GroupName = "Bristol", Ukprn = "100", CompaniesHouseNumber = "100" };
			trustsRepository.Setup(m => m.SearchTrusts(It.IsAny<string>())).ReturnsAsync(
			   new TrustsResponse { Data = new List<Trust> { trust } });

			// Act
			var result = await sut.OnGetSearch($"{trust.GroupName} ({trust.Ukprn})");
			var json = ExtractType<List<SearchResponse>>(Assert.IsType<JsonResult>(result));

			// Assert
			Assert.Equal($"<strong>Bristol</strong> (100)\r\n\t\t\t\t\t\t\t\t\t<br />\r\n\t\t\t\t\t\t\t\t\tCompanies House number: {trust.CompaniesHouseNumber}",
				  json[0].suggestion);
			Assert.Equal($"{trust.GroupName} ({trust.Ukprn})", json[0].value);
		}

		[Theory, AutoMoqData]
		public async Task OnGetSearch_DoesNotThrow_WhenEmptyTrust([Frozen] Mock<ITrustsRespository> trustsRepository, Mock<ISession> session)
		{
			// Arrange
			var sut = new SearchTrustModel(trustsRepository.Object, session.Object, new ErrorService());
			var trust = new Trust { GroupName = string.Empty, Ukprn = string.Empty, CompaniesHouseNumber = string.Empty };
			trustsRepository.Setup(m => m.SearchTrusts(It.IsAny<string>())).ReturnsAsync(
			   new TrustsResponse { Data = new List<Trust> { trust } });

			// Act
			var result = await sut.OnGetSearch(trust.GroupName);
			var json = ExtractType<List<SearchResponse>>(Assert.IsType<JsonResult>(result));

			// Assert
			Assert.Equal(string.Empty, json[0].suggestion);
		}

		[Theory, AutoMoqData]
		public void OnGetSearch_SearchTrust_Prepopulated([Frozen] Mock<ITrustsRespository> trustsRepository, Mock<ISession> session)
		{
			// Arrange
			var sut = new SearchTrustModel(trustsRepository.Object, session.Object, new ErrorService());
			var trust = new Trust { GroupName = "Bristol", Ukprn = "100", CompaniesHouseNumber = "100" };
			trustsRepository.Setup(m => m.SearchTrusts(It.IsAny<string>())).ReturnsAsync(
			   new TrustsResponse { Data = new List<Trust> { trust } });

			var output = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(trust));
			session.Setup(m => m.TryGetValue(It.IsAny<string>(), out output));

			// Act
			sut.OnGet();

			// Assert
			Assert.Equal($"{trust.GroupName} ({trust.Ukprn})", sut.SearchQuery);
		}

		private static T ExtractType<T>(JsonResult result)
		{
			var resultAsJson = JsonConvert.SerializeObject(result.Value);
			return JsonConvert.DeserializeObject<T>(resultAsJson);
		}
	}
}