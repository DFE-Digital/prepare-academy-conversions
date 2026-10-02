using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.ViewModels;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.Utils;

public static class SignificantChangeTaskListBuilder
{
   private static readonly IReadOnlyList<SignificantChangeTaskSectionDefinition> TaskSections =
   [
      KeyProjectsDatesSection(1),
      AdmissionsVariationSection(2),
      ConsultationDetailsSection(3),
      AcademyPerformanceSection(4),
      PublicSectorEqualityDutySection(5),
      LandTransactionAndPlanningPermissionSection(6),
      FinancialDetailsSection(7),
      HighQualityTrustFrameworkSection(8),
      RecommendationOnHowToProceedSection(9),
   ];

   private static SignificantChangeTaskSectionDefinition KeyProjectsDatesSection(int displayOrder)
   {
      return new SignificantChangeTaskSectionDefinition(
         "key-project-dates",
         "Key project dates",
         displayOrder,
         [
            new SignificantChangeTaskDefinition(
               "confirm-project-dates",
               "Confirm project dates",
               1,
               Links.SignificantChange.ConfirmProjectDates,
               project => GetTaskStatus(project.ProjectDatesStatus))
         ]
      );
   }

   private static SignificantChangeTaskSectionDefinition AdmissionsVariationSection(int displayOrder)
   {
      return new SignificantChangeTaskSectionDefinition(
         "admissions-variation",
         "Admissions variation",
         displayOrder,
         []
      );
   }

   private static SignificantChangeTaskSectionDefinition ConsultationDetailsSection(int displayOrder)
   {
      return new SignificantChangeTaskSectionDefinition(
         "consultation-details",
         "Consultation details",
         displayOrder,
         [
            new SignificantChangeTaskDefinition(
               "stakeholder-consultation",
               "Stakeholder consultation",
               1,
               Links.SignificantChange.StakeholderConsultation,
               project => GetTaskStatus(project.StakeholderConsultationStatus)),
            new SignificantChangeTaskDefinition(
               "consultation-duration",
               "Consultation duration",
               2,
               Links.SignificantChange.ConsultationDuration,
               project => GetTaskStatus(project.ConsultationDurationStatus)),
            new SignificantChangeTaskDefinition(
               "admission-variation-consultation",
               "Admission variation consultation",
               3,
               Links.SignificantChange.AdmissionVariationConsultation,
               project => GetTaskStatus(project.AdmissionVariationStatus)),

            new SignificantChangeTaskDefinition(
               "stakeholder-objections",
               "Stakeholder objections",
               4,
               Links.SignificantChange.StakeholderObjections,
               project => GetTaskStatus(project.StakeholderObjectionsStatus)),

            new SignificantChangeTaskDefinition(
               "religious-body-consultation",
               "Religious body consultation",
               5,
               Links.SignificantChange.ReligiousBodyConsultation,
               project => GetTaskStatus(project.ReligiousBodyConsultationStatus))
         ]
      );
   }

   private static SignificantChangeTaskSectionDefinition AcademyPerformanceSection(int displayOrder)
   {
      return new SignificantChangeTaskSectionDefinition(
         "academy-performance",
         "Academy performance",
         displayOrder,
         []
      );
   }

   private static SignificantChangeTaskSectionDefinition PublicSectorEqualityDutySection(int displayOrder)
   {
      return new SignificantChangeTaskSectionDefinition(
         "public-sector-equality-duty",
         "Public Sector Equality Duty",
         displayOrder,
         [
            new SignificantChangeTaskDefinition(
               "public-sector-equality-duty",
               "Public Sector Equality Duty",
               1,
               Links.SignificantChange.PublicSectorEqualityDuty,
               project => GetTaskStatus(project.EqualitiesImpactAssessmentStatus))
         ]
      );
   }

   private static SignificantChangeTaskSectionDefinition LandTransactionAndPlanningPermissionSection(int displayOrder)
   {
      return new SignificantChangeTaskSectionDefinition(
         "land-transaction-application-and-planning-permission",
         "Land transaction application and planning permission",
         displayOrder,
         [
            new SignificantChangeTaskDefinition(
               "planning-permission",
               "Planning Permission",
               1,
               Links.SignificantChange.PlanningPermission,
               project => GetTaskStatus(project.PlanningPermissionTaskStatus))
         ]
      );
   }

   private static SignificantChangeTaskSectionDefinition FinancialDetailsSection(int displayOrder)
   {
      return new SignificantChangeTaskSectionDefinition(
         "financial-details",
         "Financial details",
         displayOrder,
         [
            new SignificantChangeTaskDefinition(
               "funding",
               "Funding",
               1,
               Links.SignificantChange.Funding,
               project => GetTaskStatus(project.FundingStatus))
         ]
      );
   }

   private static SignificantChangeTaskSectionDefinition HighQualityTrustFrameworkSection(int displayOrder)
   {
      return new SignificantChangeTaskSectionDefinition(
         "high-quality-trust-framework",
         "High Quality Trust Framework",
         displayOrder,
         []
      );
   }

   private static SignificantChangeTaskSectionDefinition RecommendationOnHowToProceedSection(int displayOrder)
   {
      return new SignificantChangeTaskSectionDefinition(
         "recommendation-on-how-to-proceed",
         "Recommendation on how to proceed",
         displayOrder,
         [
            new SignificantChangeTaskDefinition(
               "significant-change-recommendation",
               "Significant change recommendation",
               1,
               Links.SignificantChange.SignificantChangeRecommendation,
               project => GetTaskStatus(project.RecommendationStatus))
         ]
      );
   }

   public static SignificantChangeTaskListViewModel Build(SignificantChangeProjectViewBaseModel project)
   {
      List<SignificantChangeTaskSectionViewModel> sections = [.. TaskSections
         .OrderBy(section => section.DisplayOrder)
         .Select(section => new SignificantChangeTaskSectionViewModel
         {
            Key = section.Key,
            Title = section.Title,
            DisplayOrder = section.DisplayOrder,
            Tasks = [.. section.Tasks
               .Where(task => task.IsVisible(project))
               .OrderBy(task => task.DisplayOrder)
               .Select(task => new SignificantChangeTaskItemViewModel
               {
                  Key = task.Key,
                  Title = task.Title,
                  Page = task.Link.Page,
                  DisplayOrder = task.DisplayOrder,
                  Status = task.GetStatus(project)
               })]
         })];

      return new SignificantChangeTaskListViewModel
      {
         Sections = sections
      };
   }

   private sealed record SignificantChangeTaskSectionDefinition(
      string Key,
      string Title,
      int DisplayOrder,
      IReadOnlyList<SignificantChangeTaskDefinition> Tasks);

   private sealed record SignificantChangeTaskDefinition(
      string Key,
      string Title,
      int DisplayOrder,
      LinkItem Link,
      Func<SignificantChangeProjectViewBaseModel, TaskListItemViewModel> GetStatus)
   {
      public Func<SignificantChangeProjectViewBaseModel, bool> IsVisible { get; init; } = _ => true;
   }

    private static TaskListItemViewModel GetTaskStatus(SignificantChangeTaskStatus status)
    {
        return status switch
        {
            SignificantChangeTaskStatus.Completed => TaskListItemViewModel.Completed,
            SignificantChangeTaskStatus.InProgress => TaskListItemViewModel.InProgress,
            _ => TaskListItemViewModel.NotStarted
        };
    }
}
