﻿using Dfe.PrepareTransfers.Web.Services;
using Dfe.PrepareTransfers.Web.Services.Responses;
using Moq;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Xunit;
using Dfe.PrepareTransfers.Web.Models.ProjectTemplate;
using Dfe.PrepareTransfers.Web.Services.DocumentGenerators;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Web.Tests.ServicesTests
{
    public class CreateProjectTemplateTests
    {
        private readonly CreateProjectTemplate _subject;
        private readonly GetProjectTemplateModel _templateSubject;
        private readonly Mock<IGetProjectTemplateModel> _getHtbDocumentForProject;
        private readonly string _projectUrn = "projectId";
        
        private readonly Mock<IGetInformationForProject> _getInformationForProject;

        public CreateProjectTemplateTests()
        {
            _getHtbDocumentForProject = new Mock<IGetProjectTemplateModel>();
            _subject = new CreateProjectTemplate(_getHtbDocumentForProject.Object);
            _getInformationForProject = new Mock<IGetInformationForProject>();
            _templateSubject = new GetProjectTemplateModel(_getInformationForProject.Object);
      }

        public class ExecuteTests : CreateProjectTemplateTests
        {
            [Fact]
            public async Task GivenGetHtbDocumentForProjectReturnsNotFound_ReturnsCorrectError()
            {
                _getHtbDocumentForProject.Setup(r => r.Execute(It.IsAny<string>())).ReturnsAsync(
                    new GetProjectTemplateModelResponse()
                    {
                        ResponseError = new ServiceResponseError
                        {
                            ErrorCode = ErrorCode.NotFound,
                            ErrorMessage = "Error message"
                        }
                    });

                var result = await _subject.Execute(_projectUrn);

                Assert.False(result.IsValid);
                Assert.Equal(ErrorCode.NotFound, result.ResponseError.ErrorCode);
                Assert.Equal("Not found", result.ResponseError.ErrorMessage);
            }

            [Fact]
            public async Task GivenGetHtbDocumentForProjectReturnsServiceError_ReturnsCorrectError()
            {
                _getHtbDocumentForProject.Setup(r => r.Execute(It.IsAny<string>())).ReturnsAsync(
                    new GetProjectTemplateModelResponse()
                    {
                        ResponseError = new ServiceResponseError
                        {
                            ErrorCode = ErrorCode.ApiError,
                            ErrorMessage = "Error message"
                        }
                    });

                var result = await _subject.Execute(_projectUrn);

                Assert.False(result.IsValid);
                Assert.Equal(ErrorCode.ApiError, result.ResponseError.ErrorCode);
                Assert.Equal("API has encountered an error", result.ResponseError.ErrorMessage);
            }

            [Fact]
            public async Task GivenGetHtbDocument_Returns_Valid_Word_Document_Stream_Result()
            {
               var projectTemplateResponse = new GetProjectTemplateModelResponse
               {
                  ProjectTemplateModel = new() {
                     Academies = [
                        new ProjectTemplateAcademyModel() { SchoolName = "Manchester Academie School" }
                     ],
                     PublicEqualityDutyImpact = "Lilely",
                     PublicEqualityDutyReduceImpactReason = "Likely reason"
                  }
               };

               _getHtbDocumentForProject.Setup(r => r.Execute(It.IsAny<string>())).ReturnsAsync(projectTemplateResponse);

               var result = await _subject.Execute(_projectUrn);

               Assert.True(result.IsValid);
            }
        }
    }
}
