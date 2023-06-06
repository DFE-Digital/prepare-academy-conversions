using AutoFixture.Xunit2;
using Dfe.PrepareConversions.Data.Models.Trust;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Pages.SponsoredProject;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.Tests.Customisations;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SponsoredProject;

public class SearchTrustModelUnitTests
{
   [Theory]
   [AutoMoqData]
   public async Task OnGetSearch_ReturnsTrustNames([Frozen] Mock<ITrustsRepository> trustsRepository, TrustSummaryResponse trusts)
   {
      // Arrange
      SearchTrustModel sut = new(trustsRepository.Object, new ErrorService());
      trustsRepository.Setup(m => m.SearchTrusts(It.IsAny<string>())).ReturnsAsync(trusts);

      // Act
      IActionResult result = await sut.OnGetSearch("name");
      List<SearchResponse> json = ExtractType<List<SearchResponse>>(Assert.IsType<JsonResult>(result));

      // Assert
      Assert.Equal($"{trusts.Data[0].GroupName.ToTitleCase()} ({trusts.Data[0].Ukprn})", json[0].value);
      Assert.Equal($"{trusts.Data[1].GroupName.ToTitleCase()} ({trusts.Data[1].Ukprn})", json[1].value);
      Assert.Equal($"{trusts.Data[2].GroupName.ToTitleCase()} ({trusts.Data[2].Ukprn})", json[2].value);
   }

   [Theory]
   [AutoMoqData]
   public async Task OnGetSearch_ReturnsSuggestion([Frozen] Mock<ITrustsRepository> trustsRepository)
   {
      // Arrange
      SearchTrustModel sut = new(trustsRepository.Object, new ErrorService());
      TrustSummary trust = new() { GroupName = "bristol", Ukprn = "100", CompaniesHouseNumber = "100" };
      trustsRepository.Setup(m => m.SearchTrusts(It.IsAny<string>())).ReturnsAsync(
         new TrustSummaryResponse { Data = new List<TrustSummary> { trust } });

      // Act
      IActionResult result = await sut.OnGetSearch(trust.GroupName);
      List<SearchResponse> json = ExtractType<List<SearchResponse>>(Assert.IsType<JsonResult>(result));

      // Assert
      Assert.Equal($"<strong>Bristol</strong> (100)<br />Companies House number: {trust.CompaniesHouseNumber}",
         SanatiseString(json[0].suggestion));
   }

   [Theory]
   [AutoMoqData]
   public async Task OnGetSearch_ReturnsSuggestion_WhenUkprnIsIncludedInSearch([Frozen] Mock<ITrustsRepository> trustsRepository)
   {
      // Arrange
      SearchTrustModel sut = new(trustsRepository.Object, new ErrorService());
      TrustSummary trust = new() { GroupName = "Bristol", Ukprn = "100", CompaniesHouseNumber = "100" };
      trustsRepository.Setup(m => m.SearchTrusts(It.IsAny<string>())).ReturnsAsync(
         new TrustSummaryResponse { Data = new List<TrustSummary> { trust } });

      // Act
      IActionResult result = await sut.OnGetSearch($"{trust.GroupName} ({trust.Ukprn})");
      List<SearchResponse> json = ExtractType<List<SearchResponse>>(Assert.IsType<JsonResult>(result));

      // Assert
      Assert.Equal($"<strong>Bristol</strong> (100)<br />Companies House number: {trust.CompaniesHouseNumber}",
         SanatiseString(json[0].suggestion));
      Assert.Equal($"{trust.GroupName} ({trust.Ukprn})", json[0].value);
   }

   [Theory]
   [AutoMoqData]
   public async Task OnGetSearch_DoesNotThrow_WhenEmptyTrust([Frozen] Mock<ITrustsRepository> trustsRepository)
   {
      // Arrange
      SearchTrustModel sut = new(trustsRepository.Object, new ErrorService());
      TrustSummary trust = new() { GroupName = string.Empty, Ukprn = string.Empty, CompaniesHouseNumber = string.Empty };
      trustsRepository.Setup(m => m.SearchTrusts(It.IsAny<string>())).ReturnsAsync(
         new TrustSummaryResponse { Data = new List<TrustSummary> { trust } });

      // Act
      IActionResult result = await sut.OnGetSearch(trust.GroupName);
      List<SearchResponse> json = ExtractType<List<SearchResponse>>(Assert.IsType<JsonResult>(result));

      // Assert
      Assert.Equal(string.Empty, json[0].suggestion);
   }

   [Theory]
   [AutoMoqData]
   public async Task OnGetSearch_SearchTrust_Prepopulated([Frozen] Mock<ITrustsRepository> trustsRepository)
   {
      // Arrange
      SearchTrustModel sut = new(trustsRepository.Object, new ErrorService());
      TrustSummary trust = new() { GroupName = "Bristol", Ukprn = "100", CompaniesHouseNumber = "100" };
      trustsRepository.Setup(m => m.SearchTrusts(It.IsAny<string>())).ReturnsAsync(
         new TrustSummaryResponse { Data = new List<TrustSummary> { trust } });

      // Act
      await sut.OnGet(trust.Ukprn, string.Empty);

      // Assert
      Assert.Equal($"{trust.GroupName} ({trust.Ukprn})", sut.SearchQuery);
   }

   private static string SanatiseString(string input)
   {
      return input.Replace("\r", "").Replace("\n", "").Replace("\t", "");
   }

   private static T ExtractType<T>(JsonResult result)
   {
      string resultAsJson = JsonConvert.SerializeObject(result.Value);
      return JsonConvert.DeserializeObject<T>(resultAsJson);
   }
}
