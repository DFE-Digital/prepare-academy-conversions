using ApplyToBecome.Data;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Pages;
using ApplyToBecomeInternal.Services;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages
{
	public class UpdateAcademyConversionProjectPageModelTests
	{
		private readonly Mock<IAcademyConversionProjectRepository> _repository;
		private readonly Mock<ErrorService> _errorService;
		private readonly AcademyConversionProject _foundProject;
		private readonly UpdateAcademyConversionProjectPageModel _model;
		private AcademyConversionProjectPostModel _academyConversionProjectPostModel;

		public UpdateAcademyConversionProjectPageModelTests()
		{
			_repository = new Mock<IAcademyConversionProjectRepository>();
			_errorService = new Mock<ErrorService>();
			_foundProject = new Fixture().Create<AcademyConversionProject>();
			_academyConversionProjectPostModel = new Fixture().Create<AcademyConversionProjectPostModel>();
			_repository.Setup(r => r.GetProjectById(10)).ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, _foundProject));
			_repository.Setup(r => r.UpdateProject(10, It.IsAny<UpdateAcademyConversionProject>()))
				.ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, _foundProject));
			_model = new UpdateAcademyConversionProjectPageModel(_repository.Object, _errorService.Object);
		}

		[Fact]
		public async void OnSuccessfulSubmit_ItUpdatesTheProjectInTheRepository()
		{
			var pageModel = GetPageModel();

			await pageModel.OnPostAsync(10);
			_repository.Verify(r => r.UpdateProject(10, It.Is<UpdateAcademyConversionProject>(p => p.Author == _academyConversionProjectPostModel.Author)));
		}
		
		[Fact]
		public async void OnSuccessfulSubmit_ItRedirectsToTheSuccessPage()
		{
			var pageModel = GetPageModel();
			pageModel.SuccessPage = Links.TaskList.Index.Page;

			var response = await pageModel.OnPostAsync(10);
			var redirectResponse = Assert.IsType<RedirectToPageResult>(response);
			Assert.Equal(Links.TaskList.Index.Page, redirectResponse.PageName);
		}

		private UpdateAcademyConversionProjectPageModel GetPageModel()
		{
			var httpContext = new DefaultHttpContext();
			var modelState = new ModelStateDictionary();
			httpContext.Request.ContentType = "application/x-www-form-urlencoded";
			var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
			var modelMetadataProvider = new EmptyModelMetadataProvider();
			var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
			var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
			var pageContext = new PageContext(actionContext) { ViewData = viewData };

			var pageModel = new UpdateAcademyConversionProjectPageModel(_repository.Object, _errorService.Object)
			{
				PageContext = pageContext, TempData = tempData, Url = new UrlHelper(actionContext),
				AcademyConversionProject = _academyConversionProjectPostModel
			};
			return pageModel;
		}
	}
}