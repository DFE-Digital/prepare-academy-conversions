using AutoFixture.Xunit2;
using Dfe.Academies.Contracts.V4.Trusts;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.Trust;
using Dfe.PrepareConversions.Data.Services.Interfaces;
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
   public async Task OnGetSearch_ReturnsTrustNames([Frozen] Mock<ITrustsRepository> trustsRepository, TrustDtoResponse trusts)
   {
      // Arrange
      SearchTrustModel sut = new(trustsRepository.Object, new ErrorService());
      trustsRepository.Setup(m => m.SearchTrusts(It.IsAny<string>())).ReturnsAsync(trusts);

      // Act
      IActionResult result = await sut.OnGetSearch("name");
      List<SearchResponse> json = ExtractType<List<SearchResponse>>(Assert.IsType<JsonResult>(result));

      // Assert
      Assert.Equal($"{trusts.Data[0].Name.ToTitleCase()} ({trusts.Data[0].Ukprn})", json[0].value);
      Assert.Equal($"{trusts.Data[1].Name.ToTitleCase()} ({trusts.Data[1].Ukprn})", json[1].value);
      Assert.Equal($"{trusts.Data[2].Name.ToTitleCase()} ({trusts.Data[2].Ukprn})", json[2].value);
   }

   [Theory]
   [AutoMoqData]
   public async Task OnGetSearch_ReturnsSuggestion([Frozen] Mock<ITrustsRepository> trustsRepository)
   {
      // Arrange
      SearchTrustModel sut = new(trustsRepository.Object, new ErrorService());
      TrustDto trust = new() { Name = "bristol", Ukprn = "100", CompaniesHouseNumber = "100" };
      trustsRepository.Setup(m => m.SearchTrusts(It.IsAny<string>())).ReturnsAsync(
         new TrustDtoResponse { Data = new List<TrustDto> { trust } });

      // Act
      IActionResult result = await sut.OnGetSearch(trust.Name);
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
      TrustDto trust = new() { Name = "Bristol", Ukprn = "100", CompaniesHouseNumber = "100" };
      trustsRepository.Setup(m => m.SearchTrusts(It.IsAny<string>())).ReturnsAsync(
         new TrustDtoResponse { Data = new List<TrustDto> { trust } });

      // Act
      IActionResult result = await sut.OnGetSearch($"{trust.Name} ({trust.Ukprn})");
      List<SearchResponse> json = ExtractType<List<SearchResponse>>(Assert.IsType<JsonResult>(result));

      // Assert
      Assert.Equal($"<strong>Bristol</strong> (100)<br />Companies House number: {trust.CompaniesHouseNumber}",
         SanatiseString(json[0].suggestion));
      Assert.Equal($"{trust.Name} ({trust.Ukprn})", json[0].value);
   }

   [Theory]
   [AutoMoqData]
   public async Task OnGetSearch_DoesNotThrow_WhenEmptyTrust([Frozen] Mock<ITrustsRepository> trustsRepository)
   {
      // Arrange
      SearchTrustModel sut = new(trustsRepository.Object, new ErrorService());
      TrustDto trust = new() { Name = string.Empty, Ukprn = string.Empty, CompaniesHouseNumber = string.Empty };
      trustsRepository.Setup(m => m.SearchTrusts(It.IsAny<string>())).ReturnsAsync(
         new TrustDtoResponse { Data = new List<TrustDto> { trust } });

      // Act
      IActionResult result = await sut.OnGetSearch(trust.Name);
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
      TrustDto trust = new() { Name = "Bristol", Ukprn = "100", CompaniesHouseNumber = "100" };
      trustsRepository.Setup(m => m.SearchTrusts(It.IsAny<string>())).ReturnsAsync(
         new TrustDtoResponse { Data = new List<TrustDto> { trust } });

      // Act
      await sut.OnGet(trust.Ukprn, string.Empty, string.Empty);

      // Assert
      Assert.Equal($"{trust.Name} ({trust.Ukprn})", sut.SearchQuery);
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
