namespace ApplyToBecomeInternal.Models
{
	public static class Links
	{
		public static class ApplicationForm
		{
			public static readonly LinkItem Index = new LinkItem { Page = "/ApplicationForm/Index" };
		}

		public static class ProjectType
		{
			public static readonly LinkItem Index = new LinkItem {BackText = "Back to project type", Page = "/ProjectType/Index"};
		}
		public static class ProjectAssignment
		{
			public static readonly LinkItem Index = new LinkItem { BackText = "Back to all conversion projects", Page = "/ProjectAssignment/Index" };
		}
		public static class ProjectList
		{
			public static readonly LinkItem Index = new LinkItem { BackText = "Back to all conversion projects", Page = "/ProjectList/Index" };
		}

		public static class ProjectNotes
		{
			public static readonly LinkItem Index = new LinkItem { Page = "/ProjectNotes/Index" };
			public static readonly LinkItem NewNote = new LinkItem { Page = "/ProjectNotes/NewNote" };
		}

		public static class TaskList
		{
			public static readonly LinkItem Index = new LinkItem { BackText = "Back to task list", Page = "/TaskList/Index" };
			public static readonly LinkItem PreviewHTBTemplate = new LinkItem { BackText = "Back to preview", Page = "/TaskList/PreviewProjectTemplate" };
			public static readonly LinkItem GenerateHTBTemplate = new LinkItem { Page = "/TaskList/DownloadProjectTemplate" };
		}

		public static class SchoolPerformance
		{
			public static readonly LinkItem ConfirmSchoolPerformance = new LinkItem { Page = "/TaskList/SchoolPerformance/ConfirmSchoolPerformance" };
			public static readonly LinkItem AdditionalInformation = new LinkItem { Page = "/TaskList/SchoolPerformance/AdditionalInformation" };
		}

		public static class SchoolApplicationForm
		{
			public static readonly LinkItem Index = new LinkItem { Page = "/ApplicationForm/Index" };
			public static readonly LinkItem SchoolApplicationTab = new LinkItem { Page = "/ApplicationForm/SchoolApplicationTab" };
		}

		public static class LocalAuthorityInformationTemplateSection
		{
			public static readonly LinkItem ConfirmLocalAuthorityInformationTemplateDates = new LinkItem { Page = "/TaskList/LocalAuthorityInformationTemplate/ConfirmLocalAuthorityInformationTemplateDates" };
			public static readonly LinkItem RecordLocalAuthorityInformationTemplateDates = new LinkItem { Page = "/TaskList/LocalAuthorityInformationTemplate/RecordLocalAuthorityInformationTemplateDates" };
		}

		public static class SchoolAndTrustInformationSection
		{
			public static readonly LinkItem ConfirmSchoolAndTrustInformation = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/ConfirmSchoolAndTrustInformation" };
			public static readonly LinkItem ProjectRecommendation = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/ProjectRecommendation" };
			public static readonly LinkItem Author = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/Author" };
			public static readonly LinkItem ClearedBy = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/ClearedBy" };
			public static readonly LinkItem AcademyOrderRequired = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/AcademyOrderRequired" };
			public static readonly LinkItem HeadTeacherBoardDate = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/AdvisoryBoardDate" };
			public static readonly LinkItem PreviousHeadTeacherBoardDateQuestion = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/PreviousAdvisoryBoard" };
			public static readonly LinkItem PreviousHeadTeacherBoardDate = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/PreviousAdvisoryBoardDate" };
			public static readonly LinkItem ProposedAcademyOpeningDate = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/ProposedAcademyOpeningDate" };
			public static readonly LinkItem RouteAndGrant = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/RouteAndGrant" };
		}

		public static class GeneralInformationSection
		{
			public static readonly LinkItem ConfirmGeneralInformation = new LinkItem { Page = "/TaskList/GeneralInformation/ConfirmGeneralInformation" };
			public static readonly LinkItem PublishedAdmissionNumber = new LinkItem { Page = "/TaskList/GeneralInformation/PublishedAdmissionNumber" };
			public static readonly LinkItem ViabilityIssues = new LinkItem { Page = "/TaskList/GeneralInformation/ViabilityIssues" };
			public static readonly LinkItem FinancialDeficit = new LinkItem { Page = "/TaskList/GeneralInformation/FinancialDeficit" };
			public static readonly LinkItem DistanceFromTrustHeadquarters = new LinkItem { Page = "/TaskList/GeneralInformation/DistanceFromTrustHeadquarters" };
			public static readonly LinkItem MPDetails = new LinkItem { Page = "/TaskList/GeneralInformation/MPDetails" };
		}

		public static class RationaleSection
		{
			public static readonly LinkItem ConfirmProjectAndTrustRationale = new LinkItem { Page = "/TaskList/Rationale/ConfirmProjectAndTrustRationale" };
			public static readonly LinkItem RationaleForProject = new LinkItem { Page = "/TaskList/Rationale/RationaleForProject" };
			public static readonly LinkItem RationaleForTrust = new LinkItem { Page = "/TaskList/Rationale/RationaleForTrust" };
		}

		public static class RisksAndIssuesSection
		{
			public static readonly LinkItem ConfirmRisksAndIssues = new LinkItem { Page = "/TaskList/RisksAndIssues/ConfirmRisksAndIssues" };
			public static readonly LinkItem RisksAndIssues = new LinkItem { Page = "/TaskList/RisksAndIssues/RisksAndIssues" };
		}

		public static class LegalRequirements
		{
			public static readonly LinkItem Summary = new LinkItem { Page = "/TaskList/LegalRequirements/LegalSummary" };
			public static readonly LinkItem GoverningBodyResolution = new LinkItem { Page = "/TaskList/LegalRequirements/LegalGoverningBody" };
			public static readonly LinkItem Consultation = new LinkItem { Page = "/TaskList/LegalRequirements/LegalConsultation" };
			public static readonly LinkItem DiocesanConsent = new LinkItem { Page = "/TaskList/LegalRequirements/LegalDiocesanConsent" };
			public static readonly LinkItem FoundationConsent = new LinkItem { Page = "/TaskList/LegalRequirements/LegalFoundationConsent" };
		}

		public static class SchoolBudgetInformationSection
		{
			public static readonly LinkItem ConfirmSchoolBudgetInformation = new LinkItem { Page = "/TaskList/SchoolBudgetInformation/ConfirmSchoolBudgetInformation" };
			public static readonly LinkItem UpdateSchoolBudgetInformation = new LinkItem { Page = "/TaskList/SchoolBudgetInformation/UpdateSchoolBudgetInformation" };
			public static readonly LinkItem AdditionalInformation = new LinkItem { Page = "/TaskList/SchoolBudgetInformation/AdditionalInformation" };
		}

		public static class SchoolPupilForecastsSection
		{
			public static readonly LinkItem ConfirmSchoolPupilForecasts = new LinkItem { Page = "/TaskList/SchoolPupilForecasts/ConfirmSchoolPupilForecasts" };
			public static readonly LinkItem AdditionalInformation = new LinkItem { Page = "/TaskList/SchoolPupilForecasts/AdditionalInformation" };
		}

		public static class KeyStagePerformanceSection
		{
			public static readonly LinkItem KeyStage2PerformanceTables = new LinkItem { Page = "/TaskList/KeyStagePerformance/KeyStage2PerformanceTables" };
			public static readonly LinkItem KeyStage2PerformanceTablesAdditionalInformation = new LinkItem { Page = "/TaskList/KeyStagePerformance/KeyStage2PerformanceTablesAdditionalInformation" };
			public static readonly LinkItem KeyStage4PerformanceTables = new LinkItem { Page = "/TaskList/KeyStagePerformance/KeyStage4PerformanceTables" };
			public static readonly LinkItem KeyStage4PerformanceTablesAdditionalInformation = new LinkItem { Page = "/TaskList/KeyStagePerformance/KeyStage4PerformanceTablesAdditionalInformation" };
			public static readonly LinkItem KeyStage5PerformanceTables = new LinkItem { Page = "/TaskList/KeyStagePerformance/KeyStage5PerformanceTables" };
			public static readonly LinkItem KeyStage5PerformanceTablesAdditionalInformation = new LinkItem { Page = "/TaskList/KeyStagePerformance/KeyStage5PerformanceTablesAdditionalInformation" };
		}

		public static class Decision
		{
			public static readonly LinkItem RecordDecision = new LinkItem { BackText = "Back", Page = "/TaskList/Decision/RecordDecision" };
			public static readonly LinkItem WhoDecided = new LinkItem { BackText = "Back", Page = "/TaskList/Decision/WhoDecided" };
			public static readonly LinkItem DeclineReason = new LinkItem { BackText = "Back", Page = "/TaskList/Decision/DeclineReason" };
			public static readonly LinkItem AnyConditions = new LinkItem { BackText = "Back", Page = "/TaskList/Decision/AnyConditions" };
			public static readonly LinkItem DecisionDate = new LinkItem { BackText = "Back", Page = "/TaskList/Decision/DecisionDate" };
			public static readonly LinkItem WhyDeferred = new LinkItem { BackText = "Back", Page = "/TaskList/Decision/WhyDeferred" };
			public static readonly LinkItem Summary = new LinkItem { BackText = "Back", Page = "/TaskList/Decision/Summary" };
		}

		public static class TrustTemplate
		{
			public static readonly LinkItem TrustTemplateGuidance = new LinkItem { Page = "/TaskList/TrustTemplate/TrustTemplateGuidance" };
		}

		public static class Public
		{
			public static readonly LinkItem Accessibility = new LinkItem { Page = "/Public/AccessibilityStatement" };
			public static readonly LinkItem CookiePreferences = new LinkItem { Page = "/Public/CookiePreferences" };
		}
	}

	public class LinkItem
	{
		public string Page { get; set; }
		public string BackText { get; set; } = "Back";
	}
}
