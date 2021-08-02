namespace ApplyToBecomeInternal.Models
{
	public static class Links
	{
		public static class ApplicationForm
		{
			public static LinkItem Index = new LinkItem { Page = "/ApplicationForm/Index" };
		}

		public static class ProjectList
		{
			public static LinkItem Index = new LinkItem { BackText = "Back to all conversion projects", Page = "/ProjectList/Index" };
		}

		public static class ProjectNotes
		{
			public static LinkItem Index = new LinkItem { Page = "/ProjectNotes/Index" };
			public static LinkItem NewNote = new LinkItem { Page = "/ProjectNotes/NewNote" };
		}

		public static class TaskList
		{
			public static LinkItem Index = new LinkItem { BackText = "Back to task list", Page = "/TaskList/Index" };
			public static LinkItem PreviewHTBTemplate = new LinkItem { BackText = "Back to preview", Page = "/TaskList/PreviewHTBTemplate" };
			public static LinkItem GenerateHTBTemplate = new LinkItem { Page = "/TaskList/GenerateHTBTemplate" };
		}

		public static class SchoolPerformance
		{
			public static LinkItem ConfirmSchoolPerformance = new LinkItem { Page = "/TaskList/SchoolPerformance/ConfirmSchoolPerformance" };
			public static LinkItem AdditionalInformation = new LinkItem { Page = "/TaskList/SchoolPerformance/AdditionalInformation" };
		}

		public static class SchoolApplicationForm
		{
			public static LinkItem Index = new LinkItem { Page = "/ApplicationForm/Index" };
		}

		public static class LocalAuthorityInformationTemplateSection
		{
			public static LinkItem ConfirmLocalAuthorityInformationTemplateDates = new LinkItem { Page = "/TaskList/LocalAuthorityInformationTemplate/ConfirmLocalAuthorityInformationTemplateDates" };
			public static LinkItem RecordLocalAuthorityInformationTemplateDates = new LinkItem { Page = "/TaskList/LocalAuthorityInformationTemplate/RecordLocalAuthorityInformationTemplateDates" };
		}

		public static class SchoolAndTrustInformationSection
		{
			public static LinkItem ConfirmSchoolAndTrustInformation = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/ConfirmSchoolAndTrustInformation" };
			public static LinkItem ProjectRecommendation = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/ProjectRecommendation" };
			public static LinkItem Author = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/Author" };
			public static LinkItem ClearedBy = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/ClearedBy" };
			public static LinkItem AcademyOrderRequired = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/AcademyOrderRequired" };
			public static LinkItem HeadTeacherBoardDate = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/HeadTeacherBoardDate" };
			public static LinkItem PreviousHeadTeacherBoardDateQuestion = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/PreviousHeadTeacherBoardDateQuestion" };
			public static LinkItem PreviousHeadTeacherBoardDate = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/PreviousHeadTeacherBoardDate" };
			public static LinkItem ProposedAcademyOpeningDate = new LinkItem { Page = "/TaskList/SchoolAndTrustInformation/ProposedAcademyOpeningDate" };
		}

		public static class GeneralInformationSection
		{
			public static LinkItem ConfirmGeneralInformation = new LinkItem { Page = "/TaskList/GeneralInformation/ConfirmGeneralInformation" };
			public static LinkItem PublishedAdmissionNumber = new LinkItem { Page = "/TaskList/GeneralInformation/PublishedAdmissionNumber" };
			public static LinkItem ViabilityIssues = new LinkItem { Page = "/TaskList/GeneralInformation/ViabilityIssues" };
			public static LinkItem FinancialDeficit = new LinkItem { Page = "/TaskList/GeneralInformation/FinancialDeficit" };
			public static LinkItem DistanceFromTrustHeadquarters = new LinkItem { Page = "/TaskList/GeneralInformation/DistanceFromTrustHeadquarters" };
		}

		public static class RationaleSection
		{
			public static LinkItem ConfirmProjectAndTrustRationale = new LinkItem { Page = "/TaskList/Rationale/ConfirmProjectAndTrustRationale" };
			public static LinkItem RationaleForProject = new LinkItem { Page = "/TaskList/Rationale/RationaleForProject" };
			public static LinkItem RationaleForTrust = new LinkItem { Page = "/TaskList/Rationale/RationaleForTrust" };
		}

		public static class RisksAndIssuesSection
		{
			public static LinkItem ConfirmRisksAndIssues = new LinkItem { Page = "/TaskList/RisksAndIssues/ConfirmRisksAndIssues" };
			public static LinkItem RisksAndIssues = new LinkItem { Page = "/TaskList/RisksAndIssues/RisksAndIssues" };
		}

		public static class SchoolBudgetInformationSection
		{
			public static LinkItem ConfirmSchoolBudgetInformation = new LinkItem { Page = "/TaskList/SchoolBudgetInformation/ConfirmSchoolBudgetInformation" };
			public static LinkItem UpdateSchoolBudgetInformation = new LinkItem { Page = "/TaskList/SchoolBudgetInformation/UpdateSchoolBudgetInformation" };
			public static LinkItem AdditionalInformation = new LinkItem { Page = "/TaskList/SchoolBudgetInformation/AdditionalInformation" };
		}

		public static class SchoolPupilForecastsSection
		{
			public static LinkItem ConfirmSchoolPupilForecasts = new LinkItem { Page = "/TaskList/SchoolPupilForecasts/ConfirmSchoolPupilForecasts" };
			public static LinkItem AdditionalInformation = new LinkItem { Page = "/TaskList/SchoolPupilForecasts/AdditionalInformation" };
		}

		public static class KeyStagePerformanceSection
		{
			public static LinkItem KeyStage2PerformanceTables = new LinkItem { Page = "/TaskList/KeyStagePerformance/KeyStage2PerformanceTables" };
			public static LinkItem KeyStage2PerformanceTablesAdditionalInformation = new LinkItem { Page = "/TaskList/KeyStagePerformance/KeyStage2PerformanceTablesAdditionalInformation" };
			public static LinkItem KeyStage4PerformanceTables = new LinkItem { Page = "/TaskList/KeyStagePerformance/KeyStage4PerformanceTables" };
			public static LinkItem KeyStage5PerformanceTables = new LinkItem { Page = "/TaskList/KeyStagePerformance/KeyStage5PerformanceTables" };
		}
	}

	public class LinkItem
	{
		public string Page { get; set; }
		public string BackText { get; set; } = "Back";
	}
}
