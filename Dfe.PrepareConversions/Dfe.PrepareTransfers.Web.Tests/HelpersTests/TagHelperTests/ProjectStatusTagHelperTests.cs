using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.Dfe.PrepareTransfers.Helpers.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareTransfers.Web.Tests.HelpersTests.TagHelperTests;

public class ProjectStatusTagHelperTests
{
   [Theory]
   [InlineData(ProjectStatuses.NotStarted, "Not Started", "govuk-tag--grey")]
   [InlineData(ProjectStatuses.InProgress, "In Progress", "govuk-tag--blue")]
   [InlineData(ProjectStatuses.Completed, "Completed", null)]
   [InlineData(ProjectStatuses.Empty, "", null)]
   public void GivenNotStartedStatus_ReturnsRedNotStartedTag(ProjectStatuses projectStatus,
      string expectedStatusText, string expectedCssClass)
   {
      // Arrange
      ProjectStatusTagHelper projectStatusTagHelper = new() { Status = projectStatus };
      TagHelperContext tagHelperContext = new(
         new TagHelperAttributeList { { "id", "elementId" } },
         new Dictionary<object, object>(),
         Guid.NewGuid().ToString("N"));
      TagHelperOutput tagHelperOutput = new("projectstatus",
         new TagHelperAttributeList(),
         (result, encoder) =>
         {
            DefaultTagHelperContent tagHelperContent = new();
            tagHelperContent.SetHtmlContent(string.Empty);
            return Task.FromResult<TagHelperContent>(tagHelperContent);
         });

      // Act
      projectStatusTagHelper.Process(tagHelperContext, tagHelperOutput);

      // Assert
      Assert.Equal("strong", tagHelperOutput.TagName);
      Assert.Equal(expectedStatusText, tagHelperOutput.Content.GetContent());
      Assert.Equal($"govuk-tag {expectedCssClass} moj-task-list__tag", tagHelperOutput.Attributes["class"].Value);
   }
}