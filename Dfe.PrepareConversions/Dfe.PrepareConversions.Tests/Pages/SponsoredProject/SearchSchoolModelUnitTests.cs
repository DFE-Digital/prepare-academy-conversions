using AutoFixture.Xunit2;
using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Pages.SponsoredProject;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.Tests.Customisations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SponsoredProject;

public class SearchSchoolModelUnitTests
{
   [Theory]
   [AutoMoqData]
   public async Task OnGetSearch_ReturnsSchoolNames([Frozen] Mock<IGetEstablishment> getEstablishment, List<EstablishmentSearchResponse> schools)
   {
      // Arrange
      SearchSchoolModel sut = new(getEstablishment.Object, new ErrorService());
      getEstablishment.Setup(m => m.SearchEstablishments(It.IsAny<string>())).ReturnsAsync(schools);

      // Act
      IActionResult result = await sut.OnGetSearch("name");
      List<SearchResponse> json = ExtractType<List<SearchResponse>>(Assert.IsType<JsonResult>(result));

      // Assert
      Assert.Equal($"{schools[0].Name} ({schools[0].Urn})", json[0].value);
      Assert.Equal($"{schools[1].Name} ({schools[1].Urn})", json[1].value);
      Assert.Equal($"{schools[2].Name} ({schools[2].Urn})", json[2].value);
   }

   [Theory]
   [AutoMoqData]
   public async Task OnGetSearch_ReturnsSuggestion([Frozen] Mock<IGetEstablishment> getEstablishment)
   {
      // Arrange
      SearchSchoolModel sut = new(getEstablishment.Object, new ErrorService());
      EstablishmentSearchResponse searchResponse = new() { Name = "bristol", Urn = "100" };
      getEstablishment.Setup(m => m.SearchEstablishments(It.IsAny<string>())).ReturnsAsync(new List<EstablishmentSearchResponse> { searchResponse });

      // Act
      IActionResult result = await sut.OnGetSearch(searchResponse.Name);
      List<SearchResponse> json = ExtractType<List<SearchResponse>>(Assert.IsType<JsonResult>(result));

      // Assert
      Assert.Equal($"<strong>{searchResponse.Name}</strong> ({searchResponse.Urn})", json[0].suggestion);
   }

   [Theory]
   [AutoMoqData]
   public async Task OnGetSearch_ReturnsSuggestion_WhenUrnIsIncludedInSearch([Frozen] Mock<IGetEstablishment> getEstablishment)
   {
      // Arrange
      SearchSchoolModel sut = new(getEstablishment.Object, new ErrorService());
      EstablishmentSearchResponse searchResponse = new() { Name = "bristol", Urn = "100" };
      getEstablishment.Setup(m => m.SearchEstablishments(It.IsAny<string>())).ReturnsAsync(new List<EstablishmentSearchResponse> { searchResponse });

      // Act
      IActionResult result = await sut.OnGetSearch($"{searchResponse.Name} ({searchResponse.Urn})");
      List<SearchResponse> json = ExtractType<List<SearchResponse>>(Assert.IsType<JsonResult>(result));

      // Assert
      Assert.Equal($"{searchResponse.Name} ({searchResponse.Urn})", json[0].value);
      Assert.Equal($"<strong>{searchResponse.Name}</strong> ({searchResponse.Urn})", json[0].suggestion);
   }

   [Theory]
   [AutoMoqData]
   public async Task OnGetSearch_DoesNotThrow_WhenEmptySchool([Frozen] Mock<IGetEstablishment> getEstablishment)
   {
      // Arrange
      SearchSchoolModel sut = new(getEstablishment.Object, new ErrorService());
      EstablishmentSearchResponse searchResponse = new() { Name = string.Empty, Urn = string.Empty };
      getEstablishment.Setup(m => m.SearchEstablishments(It.IsAny<string>())).ReturnsAsync(new List<EstablishmentSearchResponse> { searchResponse });

      // Act
      IActionResult result = await sut.OnGetSearch(searchResponse.Name);
      List<SearchResponse> json = ExtractType<List<SearchResponse>>(Assert.IsType<JsonResult>(result));

      // Assert
      Assert.Equal(string.Empty, json[0].suggestion);
   }

   [Theory]
   [AutoMoqData]
   public async Task OnGetSearch_SearchSchool_Prepopulated([Frozen] Mock<IGetEstablishment> getEstablishment)
   {
      // Arrange
      SearchSchoolModel sut = new(getEstablishment.Object, new ErrorService())
      {
         TempData = new TempDataDictionary(Mock.Of<HttpContext>(), Mock.Of<ITempDataProvider>())
      };
      EstablishmentResponse establishmentResponse = new() { EstablishmentName = "Bristol", Urn = "100" };
      getEstablishment.Setup(m => m.GetEstablishmentByUrn(It.IsAny<string>())).ReturnsAsync(establishmentResponse);


      // Act
      await sut.OnGet(establishmentResponse.Urn);

      // Assert
      Assert.Equal($"{establishmentResponse.EstablishmentName} ({establishmentResponse.Urn})", sut.SearchQuery);
   }

   private static T ExtractType<T>(JsonResult result)
   {
      string resultAsJson = JsonConvert.SerializeObject(result.Value);
      return JsonConvert.DeserializeObject<T>(resultAsJson);
   }
}
