using System;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.Models;

public static class Links
{
   private static readonly List<LinkItem> _links = new();

   private static LinkItem AddLinkItem(string page, string backText = "Back")
   {
      LinkItem item = new() { Page = page, BackText = backText };
      _links.Add(item);
      return item;
   }

   public static LinkItem ByPage(string page)
   {
      return _links.FirstOrDefault(x => string.Equals(page, x.Page, StringComparison.InvariantCultureIgnoreCase));
   }

   public static class ApplicationForm
   {
      public static readonly LinkItem Index = AddLinkItem(page: "/ApplicationForm/Index");
   }

   public static class AnnexB
   {
      public static readonly LinkItem Index = AddLinkItem(backText: "Back to task list", page: "/AnnexB/Index");
      public static readonly LinkItem Edit = AddLinkItem(page: "/AnnexB/Edit");
   }

   public static class ProjectType
   {
      public static readonly LinkItem Index = AddLinkItem(backText: "Back to project type", page: "/ProjectType/Index");
   }

   public static class ProjectAssignment
   {
      public static readonly LinkItem Index = AddLinkItem(backText: "Back to all conversion projects", page: "/ProjectAssignment/Index");
   }

   public static class ProjectList
   {
      public static readonly LinkItem Index = AddLinkItem(backText: "Back to all conversion projects", page: "/ProjectList/Index");
   }

   public static class ProjectNotes
   {
      public static readonly LinkItem Index = AddLinkItem(page: "/ProjectNotes/Index");
      public static readonly LinkItem NewNote = AddLinkItem(page: "/ProjectNotes/NewNote");
   }

   public static class TaskList
   {
      public static readonly LinkItem Index = AddLinkItem(backText: "Back to task list", page: "/TaskList/Index");
      public static readonly LinkItem PreviewHTBTemplate = AddLinkItem(backText: "Back to preview", page: "/TaskList/PreviewProjectTemplate");
      public static readonly LinkItem GenerateHTBTemplate = AddLinkItem(page: "/TaskList/DownloadProjectTemplate");
   }

   public static class FormAMat
   {
      public static readonly LinkItem Index = AddLinkItem(backText: "Back to project listing", page: "/FormAMat/Index");
      public static readonly LinkItem OtherSchoolsInMat = AddLinkItem(backText: "Back to project listing", page: "/FormAMat/OtherSchoolsInMat");
   }

   public static class SchoolPerformance
   {
      public static readonly LinkItem ConfirmSchoolPerformance = AddLinkItem(page: "/TaskList/SchoolPerformance/ConfirmSchoolPerformance");
      public static readonly LinkItem AdditionalInformation = AddLinkItem(page: "/TaskList/SchoolPerformance/AdditionalInformation");
   }

   public static class SchoolApplicationForm
   {
      public static readonly LinkItem Index = AddLinkItem(page: "/ApplicationForm/Index");
      public static readonly LinkItem SchoolApplicationTab = AddLinkItem(page: "/ApplicationForm/SchoolApplicationTab");
   }

   public static class LocalAuthorityInformationTemplateSection
   {
      public static readonly LinkItem ConfirmLocalAuthorityInformationTemplateDates =
         AddLinkItem(page: "/TaskList/LocalAuthorityInformationTemplate/ConfirmLocalAuthorityInformationTemplateDates");

      public static readonly LinkItem RecordLocalAuthorityInformationTemplateDates =
         AddLinkItem(page: "/TaskList/LocalAuthorityInformationTemplate/RecordLocalAuthorityInformationTemplateDates");
   }

   public static class SchoolAndTrustInformationSection
   {
      public static readonly LinkItem ConfirmSchoolAndTrustInformation = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/ConfirmSchoolAndTrustInformation");
      public static readonly LinkItem ProjectRecommendation = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/ProjectRecommendation");
      public static readonly LinkItem Author = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/Author");
      public static readonly LinkItem ClearedBy = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/ClearedBy");
      public static readonly LinkItem AcademyOrderRequired = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/AcademyOrderRequired");
      public static readonly LinkItem HeadTeacherBoardDate = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/AdvisoryBoardDate");
      public static readonly LinkItem PreviousHeadTeacherBoardDateQuestion = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/PreviousAdvisoryBoard");
      public static readonly LinkItem PreviousHeadTeacherBoardDate = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/PreviousAdvisoryBoardDate");
      public static readonly LinkItem Form7Received = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/Form7Received");
      public static readonly LinkItem Form7ReceivedDate = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/Form7ReceivedDate");
      public static readonly LinkItem ProposedAcademyOpeningDate = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/ProposedAcademyOpeningDate");
      public static readonly LinkItem RouteAndGrant = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/RouteAndGrant");
      public static readonly LinkItem DaoPackSent = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/DaoPackSentDate");
   }

   public static class GeneralInformationSection
   {
      public static readonly LinkItem ConfirmGeneralInformation = AddLinkItem(page: "/TaskList/GeneralInformation/ConfirmGeneralInformation");
      public static readonly LinkItem PublishedAdmissionNumber = AddLinkItem(page: "/TaskList/GeneralInformation/PublishedAdmissionNumber");
      public static readonly LinkItem ViabilityIssues = AddLinkItem(page: "/TaskList/GeneralInformation/ViabilityIssues");
      public static readonly LinkItem FinancialDeficit = AddLinkItem(page: "/TaskList/GeneralInformation/FinancialDeficit");
      public static readonly LinkItem DistanceFromTrustHeadquarters = AddLinkItem(page: "/TaskList/GeneralInformation/DistanceFromTrustHeadquarters");
      public static readonly LinkItem MPDetails = AddLinkItem(page: "/TaskList/GeneralInformation/MPDetails");
   }

   public static class RationaleSection
   {
      public static readonly LinkItem ConfirmProjectAndTrustRationale = AddLinkItem(page: "/TaskList/Rationale/ConfirmProjectAndTrustRationale");
      public static readonly LinkItem RationaleForProject = AddLinkItem(page: "/TaskList/Rationale/RationaleForProject");
      public static readonly LinkItem RationaleForTrust = AddLinkItem(page: "/TaskList/Rationale/RationaleForTrust");
   }

   public static class RisksAndIssuesSection
   {
      public static readonly LinkItem ConfirmRisksAndIssues = AddLinkItem(page: "/TaskList/RisksAndIssues/ConfirmRisksAndIssues");
      public static readonly LinkItem RisksAndIssues = AddLinkItem(page: "/TaskList/RisksAndIssues/RisksAndIssues");
   }

   public static class LegalRequirements
   {
      public static readonly LinkItem Summary = AddLinkItem(page: "/TaskList/LegalRequirements/LegalSummary");
      public static readonly LinkItem GoverningBodyResolution = AddLinkItem(page: "/TaskList/LegalRequirements/LegalGoverningBody");
      public static readonly LinkItem Consultation = AddLinkItem(page: "/TaskList/LegalRequirements/LegalConsultation");
      public static readonly LinkItem DiocesanConsent = AddLinkItem(page: "/TaskList/LegalRequirements/LegalDiocesanConsent");
      public static readonly LinkItem FoundationConsent = AddLinkItem(page: "/TaskList/LegalRequirements/LegalFoundationConsent");
   }

   public static class SchoolBudgetInformationSection
   {
      public static readonly LinkItem ConfirmSchoolBudgetInformation = AddLinkItem(page: "/TaskList/SchoolBudgetInformation/ConfirmSchoolBudgetInformation");
      public static readonly LinkItem UpdateSchoolBudgetInformation = AddLinkItem(page: "/TaskList/SchoolBudgetInformation/UpdateSchoolBudgetInformation");
      public static readonly LinkItem AdditionalInformation = AddLinkItem(page: "/TaskList/SchoolBudgetInformation/AdditionalInformation");
   }

   public static class SchoolPupilForecastsSection
   {
      public static readonly LinkItem ConfirmSchoolPupilForecasts = AddLinkItem(page: "/TaskList/SchoolPupilForecasts/ConfirmSchoolPupilForecasts");
      public static readonly LinkItem AdditionalInformation = AddLinkItem(page: "/TaskList/SchoolPupilForecasts/AdditionalInformation");
   }

   public static class KeyStagePerformanceSection
   {
      public static readonly LinkItem KeyStage2PerformanceTables = AddLinkItem(page: "/TaskList/KeyStagePerformance/KeyStage2PerformanceTables");

      public static readonly LinkItem KeyStage2PerformanceTablesAdditionalInformation =
         AddLinkItem(page: "/TaskList/KeyStagePerformance/KeyStage2PerformanceTablesAdditionalInformation");

      public static readonly LinkItem KeyStage4PerformanceTables = AddLinkItem(page: "/TaskList/KeyStagePerformance/KeyStage4PerformanceTables");

      public static readonly LinkItem KeyStage4PerformanceTablesAdditionalInformation =
         AddLinkItem(page: "/TaskList/KeyStagePerformance/KeyStage4PerformanceTablesAdditionalInformation");

      public static readonly LinkItem KeyStage5PerformanceTables = AddLinkItem(page: "/TaskList/KeyStagePerformance/KeyStage5PerformanceTables");

      public static readonly LinkItem KeyStage5PerformanceTablesAdditionalInformation =
         AddLinkItem(page: "/TaskList/KeyStagePerformance/KeyStage5PerformanceTablesAdditionalInformation");
   }

   public static class Decision
   {
      public static readonly LinkItem RecordDecision = AddLinkItem(backText: "Back", page: "/TaskList/Decision/RecordDecision");
      public static readonly LinkItem WhoDecided = AddLinkItem(backText: "Back", page: "/TaskList/Decision/WhoDecided");
      public static readonly LinkItem DeclineReason = AddLinkItem(backText: "Back", page: "/TaskList/Decision/DeclineReason");
      public static readonly LinkItem AnyConditions = AddLinkItem(backText: "Back", page: "/TaskList/Decision/AnyConditions");
      public static readonly LinkItem DecisionDate = AddLinkItem(backText: "Back", page: "/TaskList/Decision/DecisionDate");
      public static readonly LinkItem WhyDeferred = AddLinkItem(backText: "Back", page: "/TaskList/Decision/WhyDeferred");
      public static readonly LinkItem Summary = AddLinkItem(backText: "Back", page: "/TaskList/Decision/Summary");
   }

   public static class TrustTemplate
   {
      public static readonly LinkItem TrustTemplateGuidance = AddLinkItem(page: "/TaskList/TrustTemplate/TrustTemplateGuidance");
   }

   public static class Public
   {
      public static readonly LinkItem Accessibility = AddLinkItem(page: "/Public/AccessibilityStatement");
      public static readonly LinkItem CookiePreferences = AddLinkItem(page: "/Public/CookiePreferences");
   }

   public static class InvoluntaryProject
   {
      public static readonly LinkItem SearchSchool = AddLinkItem(page: "/InvoluntaryProject/SearchSchool");
      public static readonly LinkItem SearchTrusts = AddLinkItem(page: "/InvoluntaryProject/SearchTrust");
      public static readonly LinkItem Summary = AddLinkItem(page: "/InvoluntaryProject/Summary");
   }
}

public class LinkItem
{
   public string Page { get; set; }
   public string BackText { get; set; } = "Back";
}
