using ApplyToBecome.Data;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Pages;
using AutoFixture;
using Moq;
using System.Net;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages
{
	public class BaseAcademyConversionProjectPageModelTests
	{
		private readonly Mock<IAcademyConversionProjectRepository> _repository;
		private readonly AcademyConversionProject _foundProject;
		private readonly BaseAcademyConversionProjectPageModel _model;

		public BaseAcademyConversionProjectPageModelTests()
		{
			_repository = new Mock<IAcademyConversionProjectRepository>();
			_foundProject = new Fixture().Create<AcademyConversionProject>();
			_repository.Setup(r => r.GetProjectById(10)).ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, _foundProject));
			_model = new BaseAcademyConversionProjectPageModel(_repository.Object);
		}

		[Fact]
		public async void GivenProjectId_GetsTheProjectFromTheRepository()
		{
			await _model.OnGetAsync(10);

			_repository.Verify(r => r.GetProjectById(10), Times.Once());
		}

		[Fact]
		public async void GivenProjectId_AssignsTheProjectToThePageModel()
		{
			await _model.OnGetAsync(10);
			Assert.Equal(_model.Project.Id, _foundProject.Id.ToString());
		}
	}
}