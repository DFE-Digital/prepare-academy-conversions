using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.Ofsted;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Text.Json;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.Ofsted;

public class OfstedBaseModelTests
{
   private const string EarlierQuestion = "Is the academy rated good or outstanding?";
   private const string MiddleQuestion =
      "Is the academy rated good or outstanding in leadership and management and quality of education?";
   private const string CurrentQuestion =
      "Is the academy meeting expected standard, strong standard or exceptional in all evaluation areas?";

   public static TheoryData<DateTime, string> InspectionDateCases =>
      new()
      {
         { new DateTime(2024, 9, 18, 23, 59, 59, DateTimeKind.Utc), EarlierQuestion },
         { new DateTime(2024, 9, 19, 0, 0, 0, DateTimeKind.Utc), MiddleQuestion },
         { new DateTime(2025, 11, 9, 0, 0, 0, DateTimeKind.Utc), MiddleQuestion },
         { new DateTime(2025, 11, 10, 0, 0, 0, DateTimeKind.Utc), CurrentQuestion }
      };

   [Theory]
   [MemberData(nameof(InspectionDateCases))]
   public void GetTaskTitleQuestion_returns_the_title_for_the_inspection_date(
      DateTime inspectionDate,
      string expectedTitle)
   {
      var model = CreateModel(inspectionDate);

      Assert.Equal(expectedTitle, model.GetTaskTitleQuestion(123));
   }

   [Fact]
   public void GetTaskTitleQuestion_returns_current_title_when_session_has_no_inspection()
   {
      var model = CreateModel(null);

      Assert.Equal(CurrentQuestion, model.GetTaskTitleQuestion(123));
   }

   private static TestableOfstedBaseModel CreateModel(DateTime? inspectionDate)
   {
      OfstedPageModelTestHelpers.MemorySession session = new();

      if (inspectionDate.HasValue)
      {
         SetSignificantChangeOfstedInspectionCommand inspection = new(inspectionDate, null, null, null);
         session.SetString(
            $"{OfstedBaseModel.OFSTED_INSPECTION_SESSION_KEY}_123",
            JsonSerializer.Serialize(inspection));
      }

      return new TestableOfstedBaseModel(
         Mock.Of<ISignificantChangeProjectRepository>(),
         session);
   }

   private sealed class TestableOfstedBaseModel(
      ISignificantChangeProjectRepository repository,
      ISession session) : OfstedBaseModel(repository, session)
   {
      protected override string TaskTitle => "Ofsted";
   }
}
