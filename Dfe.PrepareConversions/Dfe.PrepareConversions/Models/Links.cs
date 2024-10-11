using DocumentFormat.OpenXml.InkML;
using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Models;

public static class Links
{
   private static readonly List<LinkItem> _links = [];
   private static string _transfersUrl;
   public static string TransfersUrl => _transfersUrl;

   private static bool _isApplicationDocumentsEnabled;
   public static bool IsApplicationDocumentsEnabled => _isApplicationDocumentsEnabled;

   public static LinkItem AddLinkItem(string page, string backText = "Back")
   {
      LinkItem item = new() { Page = page, BackText = backText };
      _links.Add(item);
      return item;
   }
   public static void InitializeTransfersUrl(string transfersUrl)
   {
      _transfersUrl = transfersUrl;
   }

   public static void InializeProjectDocumentsEnabled(bool isApplicationDocumentsEnabled) {
      _isApplicationDocumentsEnabled = isApplicationDocumentsEnabled;
   }
   public static LinkItem ByPage(string page)
   {
      return _links.Find(x => string.Equals(page, x.Page, StringComparison.InvariantCultureIgnoreCase));
   }

   public static class ApplicationForm
   {
      public static readonly LinkItem Index = AddLinkItem(page: "/ApplicationForm/Index");
   }

   public static class AnnexB
   {
      public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/AnnexB/Index");
      public static readonly LinkItem Edit = AddLinkItem(page: "/AnnexB/Edit");
   }

   public static class SchoolImprovementPlans
   {
      public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/SchoolImprovementPlans/Index");
      public static readonly LinkItem WhoArrangedThePlan = AddLinkItem(backText: "Back", page: "/SchoolImprovementPlans/WhoArrangedThePlan");
      public static readonly LinkItem WhoProvidedThePlan = AddLinkItem(backText: "Back", page: "/SchoolImprovementPlans/WhoProvidedThePlan");
      public static readonly LinkItem StartDateOfThePlan = AddLinkItem(backText: "Back", page: "/SchoolImprovementPlans/StartDateOfThePlan");
      public static readonly LinkItem EndDateOfThePlan = AddLinkItem(backText: "Back", page: "/SchoolImprovementPlans/EndDateOfThePlan");
      public static readonly LinkItem CommentsOnThePlan = AddLinkItem(backText: "Back", page: "/SchoolImprovementPlans/CommentsOnThePlan");
      public static readonly LinkItem ConfidenceLevelOfThePlan = AddLinkItem(backText: "Back", page: "/SchoolImprovementPlans/ConfidenceLevelOfThePlan");
      public static readonly LinkItem Summary = AddLinkItem(backText: "Back", page: "/SchoolImprovementPlans/Summary");
   }

   public static class ExternalApplicationForm
   {
      public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/ExternalApplicationForm/Index");
      public static readonly LinkItem Edit = AddLinkItem(page: "/ExternalApplicationForm/Edit");
   }

   public static class ProjectType
   {
      public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/ProjectType/Index");
   }

   public static class ProjectAssignment
   {
      public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/ProjectAssignment/Index");
      public static readonly LinkItem FormAMatProjectAssignment = AddLinkItem(backText: "Back", page: "/ProjectAssignment/FormAMatProjectAssignment");
   }

   public static class ProjectList
   {
      public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/ProjectList/Index");
      public static readonly LinkItem FormAMat = AddLinkItem(backText: "Back", page: "/FormAMat/ProjectList");
      public static readonly LinkItem ProjectGroups = AddLinkItem(backText: "Back", page: "/Groups/ProjectList");
   }

   public static class ProjectNotes
   {
      public static readonly LinkItem Index = AddLinkItem(page: "/ProjectNotes/Index");
      public static readonly LinkItem NewNote = AddLinkItem(page: "/ProjectNotes/NewNote");
   }

   public static class ApplicationDocuments
   {
      public static readonly LinkItem Index = AddLinkItem(page: "/ApplicationDocuments/Index");
   }

   public static class TaskList
   {
      public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/TaskList/Index");
      public static readonly LinkItem PreviewHTBTemplate = AddLinkItem(backText: "Back", page: "/TaskList/PreviewProjectTemplate");
      public static readonly LinkItem GenerateHTBTemplate = AddLinkItem(page: "/TaskList/DownloadProjectTemplate");
   }

   public static class FormAMat
   {
      public static readonly LinkItem Index = AddLinkItem(backText: "Back", page: "/FormAMat/Index");
      // not sure these are right probably need to look at this, they look the wrong way round
      public static readonly LinkItem OtherSchoolsInMat = AddLinkItem(backText: "Back to project listing", page: "/FormAMat/FormAMatParentIndex");
      public static readonly LinkItem FormAMatProjectPage = AddLinkItem(backText: "Back to project listing", page: "/schools-in-this-mat");
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
      public static readonly LinkItem ConfirmSchoolAndTrustInformation = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/ConversionDetails");
      public static readonly LinkItem ProjectRecommendation = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/ProjectRecommendation");
      public static readonly LinkItem UpdateTrust = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/UpdateTrust");
      public static readonly LinkItem Author = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/Author");
      public static readonly LinkItem ClearedBy = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/ClearedBy");
      public static readonly LinkItem HeadTeacherBoardDate = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/AdvisoryBoardDate");
      public static readonly LinkItem Form7Received = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/Form7Received");
      public static readonly LinkItem Form7ReceivedDate = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/Form7ReceivedDate");
      public static readonly LinkItem RouteAndGrant = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/Voluntary/RouteAndGrant");
      public static readonly LinkItem GrantDetails = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/Sponsored/GrantDetails");
      public static readonly LinkItem GrantType = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/Sponsored/GrantType");
      public static readonly LinkItem NumberOfSites = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/Sponsored/NumberOfSites");
      public static readonly LinkItem DaoPackSent = AddLinkItem(page: "/TaskList/SchoolAndTrustInformation/DaoPackSentDate");
   }

   public static class SchoolOverviewSection
   {
      public static readonly LinkItem ConfirmSchoolOverview = AddLinkItem(page: "/TaskList/SchoolOverview/SchoolOverview");
      public static readonly LinkItem PublishedAdmissionNumber = AddLinkItem(page: "/TaskList/SchoolOverview/PublishedAdmissionNumber");
      public static readonly LinkItem PupilsAttendingGroup = AddLinkItem(page: "/TaskList/SchoolOverview/PRUPupilsAttendingGroup");
      public static readonly LinkItem ViabilityIssues = AddLinkItem(page: "/TaskList/SchoolOverview/ViabilityIssues");
      public static readonly LinkItem NumberOfPlacesFundedFor = AddLinkItem(page: "/TaskList/SchoolOverview/SENNumberOfPlacesFundedFor");
      public static readonly LinkItem NumberOfResidentialPlaces = AddLinkItem(page: "/TaskList/SchoolOverview/SENNumberOfResidentialPlaces");
      public static readonly LinkItem NumberOfFundedResidentialPlaces = AddLinkItem(page: "/TaskList/SchoolOverview/SENNumberOfFundedResidentialPlaces");
      public static readonly LinkItem FinancialDeficit = AddLinkItem(page: "/TaskList/SchoolOverview/FinancialDeficit");
      public static readonly LinkItem DistanceFromTrustHeadquarters = AddLinkItem(page: "/TaskList/SchoolOverview/DistanceFromTrustHeadquarters");
      public static readonly LinkItem MPDetails = AddLinkItem(page: "/TaskList/SchoolOverview/MPDetails");
      public static readonly LinkItem PartOfPfiScheme = AddLinkItem(page: "/TaskList/SchoolOverview/PartOfPfiScheme");

      public static readonly LinkItem NumberOfAlternativeProvisionPlaces = AddLinkItem(page: "/TaskList/SchoolOverview/PRUNumberOfAlternativeProvisionPlaces");
      public static readonly LinkItem NumberOfSENUnitPlaces = AddLinkItem(page: "/TaskList/SchoolOverview/SENUnitNumberOfPlaces");
      public static readonly LinkItem NumberOfMedicalPlaces = AddLinkItem(page: "/TaskList/SchoolOverview/PRUNumberOfMedicalPlaces");
      public static readonly LinkItem NumberOfPost16Places = AddLinkItem(page: "/TaskList/SchoolOverview/PRUNumberOfPost16Places");

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
      public static readonly LinkItem ConfirmSchoolBudgetInformation = AddLinkItem(page: "/TaskList/SchoolBudgetInformation/Budget");
      public static readonly LinkItem UpdateSchoolBudgetInformation = AddLinkItem(page: "/TaskList/SchoolBudgetInformation/UpdateSchoolBudgetInformation");
      public static readonly LinkItem AdditionalInformation = AddLinkItem(page: "/TaskList/SchoolBudgetInformation/AdditionalInformation");
   }

   public static class SchoolPupilForecastsSection
   {
      public static readonly LinkItem ConfirmSchoolPupilForecasts = AddLinkItem(page: "/TaskList/SchoolPupilForecasts/PupilForecasts");
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

      public static readonly LinkItem EducationalAttendance =
         AddLinkItem(page: "/TaskList/KeyStagePerformance/EducationalAttendance");

      public static readonly LinkItem EducationalAttendanceAdditionalInformation =
         AddLinkItem(page: "/TaskList/KeyStagePerformance/EducationalAttendanceAdditionalInformation");
   }

   public static class Decision
   {
      public static readonly LinkItem RecordDecision = AddLinkItem(backText: "Back", page: "/TaskList/Decision/RecordDecision");
      public static readonly LinkItem WhoDecided = AddLinkItem(backText: "Back", page: "/TaskList/Decision/WhoDecided");
      public static readonly LinkItem DAOPrecursor = AddLinkItem(backText: "Back", page: "/TaskList/Decision/DAOPrecursor");
      public static readonly LinkItem DAOBeforeYouStart = AddLinkItem(backText: "Back", page: "/TaskList/Decision/DAOBeforeYouStart");
      public static readonly LinkItem DeclineReason = AddLinkItem(backText: "Back", page: "/TaskList/Decision/DeclineReason");
      public static readonly LinkItem AnyConditions = AddLinkItem(backText: "Back", page: "/TaskList/Decision/AnyConditions");
      public static readonly LinkItem DecisionDate = AddLinkItem(backText: "Back", page: "/TaskList/Decision/DecisionDate");
      public static readonly LinkItem DecisionMaker = AddLinkItem(backText: "Back", page: "/TaskList/Decision/DecisionMaker");
      public static readonly LinkItem WhyDeferred = AddLinkItem(backText: "Back", page: "/TaskList/Decision/WhyDeferred");
      public static readonly LinkItem WhyWithdrawn = AddLinkItem(backText: "Back", page: "/TaskList/Decision/WhyWithdrawn");
      public static readonly LinkItem WhyDAORevoked = AddLinkItem(backText: "Back", page: "/TaskList/Decision/WhyDAORevoked");
      public static readonly LinkItem Summary = AddLinkItem(backText: "Back", page: "/TaskList/Decision/Summary");
      public static readonly LinkItem SubMenuRecordADecision = AddLinkItem(backText: "Back", page: "/TaskList/Decision/RecordADecision");
      public static readonly LinkItem AcademyOrderDate = AddLinkItem(backText: "Back", page: "/TaskList/Decision/AcademyOrderDate");

   }
   public static class DeleteProject
   {
      public static readonly LinkItem ConfirmToDeleteProject = AddLinkItem(page: "/TaskList/DeleteProject/ConfirmToDeleteProject");
   }

   public static class TrustTemplate
   {
      public static readonly LinkItem TrustTemplateGuidance = AddLinkItem(page: "/TaskList/TrustTemplate/TrustTemplateGuidance");
   }

   public static class Public
   {
      public static readonly LinkItem Accessibility = AddLinkItem(page: "/Public/AccessibilityStatement");
      public static readonly LinkItem CookiePreferences = AddLinkItem(page: "/Public/CookiePreferences");
      public static readonly LinkItem CookiePreferencesURL = AddLinkItem(page: "/public/cookie-Preferences");
   }

   public static class NewProject
   {
      public static readonly LinkItem SearchSchool = AddLinkItem(page: "/NewProject/SearchSchool");
      public static readonly LinkItem NewConversionInformation = AddLinkItem(page: "/NewProject/NewConversionInformation");
      public static readonly LinkItem SchoolApply = AddLinkItem(page: "/NewProject/SchoolApply");
      public static readonly LinkItem SearchTrusts = AddLinkItem(page: "/NewProject/SearchTrust");
      public static readonly LinkItem PreferredTrust = AddLinkItem(page: "/NewProject/PreferredTrust");
      public static readonly LinkItem IsThisFormAMat = AddLinkItem(page: "/NewProject/IsThisFormAMat");
      public static readonly LinkItem IsProjectAlreadyInPrepare = AddLinkItem(page: "/NewProject/IsProjectAlreadyInPreprare");
      public static readonly LinkItem LinkFormAMatProject = AddLinkItem(page: "/NewProject/LinkFormAMatProject");
      public static readonly LinkItem CreateNewFormAMat = AddLinkItem(page: "/NewProject/CreateNewFormAMat");
      public static readonly LinkItem Summary = AddLinkItem(page: "/NewProject/Summary");
   }

   public static class ProjectDates
   {
      public static readonly LinkItem ConfirmProjectDates = AddLinkItem(page: "/TaskList/ProjectDates/ConfirmProjectDates");
      public static readonly LinkItem AdvisoryBoardDate = AddLinkItem(page: "/TaskList/ProjectDates/AdvisoryBoardDate");
      public static readonly LinkItem PreviousAdvisoryBoard = AddLinkItem(page: "/TaskList/ProjectDates/PreviousAdvisoryBoardDate");
      public static readonly LinkItem PropsedConversionDate = AddLinkItem(page: "/TaskList/ProjectDates/ProposedConversionDate"); 
      public static readonly LinkItem ReasonForConversionDateChange = AddLinkItem(page: "/TaskList/ProjectDates/ReasonForConversionDateChange"); 
      public static readonly LinkItem ConversionDateHistory = AddLinkItem(page: "/TaskList/ProjectDates/ConversionDateHistory");
   }
   
   public static class ProjectGroups
   {
      public static readonly LinkItem CreateANewGroup = AddLinkItem(backText: "Back", page: "/Groups/CreateANewGroup");
      public static readonly LinkItem WhichTrustWillTheGroupJoin = AddLinkItem(page: "/Groups/SearchTrustForGroup");
      public static readonly LinkItem CheckIncomingTrustsDetails = AddLinkItem(page: "/Groups/CheckIncomingTrustsDetails");
      public static readonly LinkItem DoYouWantToAddConversions = AddLinkItem(page: "/Groups/DoYouWantToAddConversions");
      public static readonly LinkItem ConfirmToRemoveConversion = AddLinkItem(page: "/Groups/ConfirmToRemoveConversion");
      public static readonly LinkItem ConfirmToDeleteGroup = AddLinkItem(page: "/Groups/ConfirmToDeleteGroup");
      public static readonly LinkItem ProjectGroupAssignment = AddLinkItem(page: "/Groups/ProjectGroupAssignment");
      public static readonly LinkItem SelectConversions = AddLinkItem(page: "/Groups/SelectConversions");
      public static readonly LinkItem CheckConversionDetails= AddLinkItem(page: "/Groups/CheckConversionDetails");
      public static readonly LinkItem ProjectGroupIndex = AddLinkItem(page: "/Groups/ProjectGroupIndex");
   }
   
   public static class DiocesanConsent
   {
      public static readonly LinkItem Home = AddLinkItem(page: "/Projects/LegalRequirements/DiocesanConsent");
   }
   
   
}

public class LinkItem
{
   public string Page { get; set; }
   public string BackText { get; set; } = "Back";
}
