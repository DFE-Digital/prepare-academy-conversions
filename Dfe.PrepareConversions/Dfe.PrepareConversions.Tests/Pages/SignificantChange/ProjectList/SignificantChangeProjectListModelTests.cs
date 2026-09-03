
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Pages.SignificantChange.ProjectList;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.ProjectList;

public class SignificantChangeProjectListModelTests
{
   [Fact]
   public async Task OnGetAsync_FloatsSignedInUserToTopOfAssignees_ThenAlphabetical()
   {
      Mock<ISignificantChangeProjectRepository> repository = BuildRepository(
         new SignificantChangeFilterParameters
         {
            AssignedUsers =
            [
               new FilterValueDisplay { Value = "Zoe Adams", Display = "Zoe Adams" },
               new FilterValueDisplay { Value = "Alice Brown", Display = "Alice Brown" },
               new FilterValueDisplay { Value = "Ste Smith", Display = "Ste Smith" }
            ]
         });

      IndexModel sut = BuildPageModel(repository.Object, signedInUserName: "Smith, Ste");

      await sut.OnGetAsync();

      sut.Filters.AvailableAssignees.Select(assignee => assignee.Display)
         .Should().ContainInOrder("Ste Smith", "Alice Brown", "Zoe Adams");
   }

   [Fact]
   public async Task OnGetAsync_WhenFilterParametersAreEmpty_LeavesAccordionsEmptyWithoutThrowing()
   {
      Mock<ISignificantChangeProjectRepository> repository =
         BuildRepository(new SignificantChangeFilterParameters(), HttpStatusCode.NotFound);

      IndexModel sut = BuildPageModel(repository.Object, signedInUserName: "Smith, Ste");

      await sut.OnGetAsync();

      sut.Filters.AvailableStatuses.Should().BeEmpty();
      sut.Filters.AvailableTiers.Should().BeEmpty();
      sut.Filters.AvailableAssignees.Should().BeEmpty();
      sut.Filters.AvailableRoutes.Should().BeEmpty();
      sut.Projects.Should().BeEmpty();
   }

   private static Mock<ISignificantChangeProjectRepository> BuildRepository(
      SignificantChangeFilterParameters filterParameters,
      HttpStatusCode filterParametersStatus = HttpStatusCode.OK)
   {
      Mock<ISignificantChangeProjectRepository> repository = new();

      repository
         .Setup(x => x.GetAllProjects(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
            It.IsAny<string[]>(), It.IsAny<string[]>(), It.IsAny<byte[]>(), It.IsAny<string[]>()))
         .ReturnsAsync(new ApiResponse<ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>(
            HttpStatusCode.OK, null));

      repository
         .Setup(x => x.GetFilterParameters())
         .ReturnsAsync(new ApiResponse<SignificantChangeFilterParameters>(
            filterParametersStatus, filterParameters));

      return repository;
   }

   private static IndexModel BuildPageModel(
      ISignificantChangeProjectRepository repository, string signedInUserName)
   {
      ClaimsPrincipal user = new(new ClaimsIdentity([new Claim("name", signedInUserName)]));
      DefaultHttpContext httpContext = new() { User = user };

      return new IndexModel(repository)
      {
         PageContext = { HttpContext = httpContext },
         TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
      };
   }
}