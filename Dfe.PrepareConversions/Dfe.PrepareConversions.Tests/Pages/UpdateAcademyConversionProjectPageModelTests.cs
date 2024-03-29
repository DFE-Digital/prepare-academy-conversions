using AutoFixture;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.Tests.Customisations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages;

public class UpdateAcademyConversionProjectPageModelTests
{
   private readonly AcademyConversionProjectPostModel _academyConversionProjectPostModel;
   private readonly Mock<ErrorService> _errorService;
   private readonly Mock<IAcademyConversionProjectRepository> _repository;

   public UpdateAcademyConversionProjectPageModelTests()
   {
      Fixture fixture = new();
      fixture.Customizations.Add(new RandomDateBuilder(DateTime.Today, DateTime.Now.AddSeconds(1)));

      _repository = new Mock<IAcademyConversionProjectRepository>();
      _errorService = new Mock<ErrorService>();
      AcademyConversionProject foundProject = fixture.Create<AcademyConversionProject>();

      _academyConversionProjectPostModel = fixture.Create<AcademyConversionProjectPostModel>();
      _academyConversionProjectPostModel.EndOfNextFinancialYear = _academyConversionProjectPostModel.EndOfCurrentFinancialYear?.AddYears(1);

      _repository.Setup(r => r.GetProjectById(10)).ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, foundProject));
      _repository.Setup(r => r.UpdateProject(10, It.IsAny<UpdateAcademyConversionProject>()))
         .ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, foundProject));
   }

   [Fact]
   public async Task OnSuccessfulSubmit_ItUpdatesTheProjectInTheRepository()
   {
      UpdateAcademyConversionProjectPageModel pageModel = GetPageModel();
      pageModel.SuccessPage = string.Empty;
      await pageModel.OnPostAsync(10);
      _repository.Verify(r => r.UpdateProject(10, It.Is<UpdateAcademyConversionProject>(p => p.Author == _academyConversionProjectPostModel.Author)));
   }

   [Fact]
   public async Task OnSuccessfulSubmit_ItRedirectsToTheSuccessPage()
   {
      UpdateAcademyConversionProjectPageModel pageModel = GetPageModel();
      pageModel.SuccessPage = Links.TaskList.Index.Page;

      IActionResult response = await pageModel.OnPostAsync(10);
      RedirectToPageResult redirectResponse = Assert.IsType<RedirectToPageResult>(response);
      Assert.Equal(Links.TaskList.Index.Page, redirectResponse.PageName);
   }

   private UpdateAcademyConversionProjectPageModel GetPageModel()
   {
      DefaultHttpContext httpContext = new();
      ModelStateDictionary modelState = new();
      httpContext.Request.ContentType = "application/x-www-form-urlencoded";
      ActionContext actionContext = new(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
      EmptyModelMetadataProvider modelMetadataProvider = new();
      ViewDataDictionary viewData = new(modelMetadataProvider, modelState);
      TempDataDictionary tempData = new(httpContext, Mock.Of<ITempDataProvider>());
      PageContext pageContext = new(actionContext) { ViewData = viewData };

      UpdateAcademyConversionProjectPageModel pageModel = new(_repository.Object, _errorService.Object)
      {
         PageContext = pageContext, TempData = tempData, Url = new UrlHelper(actionContext), AcademyConversionProject = _academyConversionProjectPostModel
      };
      return pageModel;
   }
}
