using AutoFixture;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.ViewModels;

namespace Dfe.PrepareConversions.Tests.ViewComponents
{
   public class SchoolAndTrustInformationViewComponentTests
   {
      private readonly Fixture _fixture;
      private readonly Mock<IAcademyConversionProjectRepository> _academyConversionProjectRepository;

      public SchoolAndTrustInformationViewComponentTests()
      {
         _fixture = new Fixture();
         _academyConversionProjectRepository = new Mock<IAcademyConversionProjectRepository>();
      }

      private AcademyConversionProject CreateConversionProject(DateTime applicationReceivedDate)
      {
         return _fixture
            .Build<AcademyConversionProject>()
            .With(x => x.ApplicationReceivedDate, applicationReceivedDate)
            .With(x => x.AssignedUser, _fixture.Create<User>())
            .Create();
      }

      private SchoolAndTrustInformationViewComponent GetViewComponent(AcademyConversionProject project)
      {
         _academyConversionProjectRepository.Setup(s => s.GetProjectById(It.IsAny<int>()))
            .ReturnsAsync(new ApiResponse<AcademyConversionProject>(HttpStatusCode.OK, project));

         var routeData = new RouteData();
         routeData.Values.Add("id", "12345");

         var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
         viewData["IsPreview"] = true;

         SchoolAndTrustInformationViewComponent viewComponent = new SchoolAndTrustInformationViewComponent(_academyConversionProjectRepository.Object)
         {
            ViewComponentContext = new ViewComponentContext()
            {
               ViewContext = new ViewContext()
               {
                  RouteData = routeData,
                  ViewData = viewData
               }
            }
         };

         return viewComponent;
      }

      [Fact]
      public async Task When_Application_Received_After_Deadline_Should_Not_Allow_Conversion_Support_Grant()
      {
         // Arrange
         var applicationReceivedDate = new DateTime(2024, 12, 21, 0, 0, 1, DateTimeKind.Utc); // After the deadline
         AcademyConversionProject project = CreateConversionProject(applicationReceivedDate);

         SchoolAndTrustInformationViewComponent viewComponent = GetViewComponent(project);

         // Act
         var result = await viewComponent.InvokeAsync() as ViewViewComponentResult;
         Assert.NotNull(result);

         var model = result.ViewData.Model as SchoolAndTrustInformationViewModel;
         Assert.NotNull(result);
         Assert.False(model.IsApplicationReceivedBeforeSupportGrantDeadline);
      }

      [Fact]
      public async Task When_Application_Received_Before_Deadline_Should_Allow_Conversion_Support_Grant()
      {
         // Arrange
         var applicationReceivedDate = new DateTime(2024, 12, 20, 23, 59, 59, DateTimeKind.Utc); // Before the deadline
         AcademyConversionProject project = CreateConversionProject(applicationReceivedDate);

         SchoolAndTrustInformationViewComponent viewComponent = GetViewComponent(project);

         // Act
         var result = await viewComponent.InvokeAsync() as ViewViewComponentResult;
         Assert.NotNull(result);

         var model = result.ViewData.Model as SchoolAndTrustInformationViewModel;
         Assert.NotNull(result);
         Assert.True(model.IsApplicationReceivedBeforeSupportGrantDeadline);
      }
   }
}
