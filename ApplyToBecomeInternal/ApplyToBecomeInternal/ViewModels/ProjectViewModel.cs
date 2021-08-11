using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Extensions;
using System;

namespace ApplyToBecomeInternal.ViewModels
{
	public class ProjectViewModel
	{
		public ProjectViewModel(AcademyConversionProject project)
		{
			Id = project.Id.ToString();
			SchoolName = project.SchoolName;
			SchoolURN = project.Urn.ToString();
			LocalAuthority = project.LocalAuthority;
			ApplicationReceivedDate = project.ApplicationReceivedDate.ToDateString();
			AssignedDate = project.AssignedDate.ToDateString();
			Phase = project.ProjectStatus;
			HeadTeacherBoardDate = project.HeadTeacherBoardDate;

			LocalAuthorityInformationTemplateSentDate = project.LocalAuthorityInformationTemplateSentDate;
			LocalAuthorityInformationTemplateReturnedDate = project.LocalAuthorityInformationTemplateReturnedDate;
			LocalAuthorityInformationTemplateComments = project.LocalAuthorityInformationTemplateComments;
			LocalAuthorityInformationTemplateLink = project.LocalAuthorityInformationTemplateLink;
			LocalAuthorityInformationTemplateSectionComplete = project.LocalAuthorityInformationTemplateSectionComplete ?? false;

			RecommendationForProject = project.RecommendationForProject;
			Author = project.Author;
			Version = project.Version;
			ClearedBy = project.ClearedBy;
			AcademyOrderRequired = project.AcademyOrderRequired;
			PreviousHeadTeacherBoardDateQuestion = project.PreviousHeadTeacherBoardDateQuestion;
			PreviousHeadTeacherBoardDate = project.PreviousHeadTeacherBoardDate;
			PreviousHeadTeacherBoardLink = project.PreviousHeadTeacherBoardLink;
			TrustReferenceNumber = project.TrustReferenceNumber;
			NameOfTrust = project.NameOfTrust;
			SponsorReferenceNumber = project.SponsorReferenceNumber;
			SponsorName = project.SponsorName;
			AcademyTypeAndRoute = project.AcademyTypeAndRoute;
			ProposedAcademyOpeningDate = project.ProposedAcademyOpeningDate;
			SchoolAndTrustInformationSectionComplete = project.SchoolAndTrustInformationSectionComplete ?? false;

			PublishedAdmissionNumber = project.PublishedAdmissionNumber;
			ViabilityIssues = project.ViabilityIssues;
			FinancialDeficit = project.FinancialDeficit;
			DistanceFromSchoolToTrustHeadquarters = project.DistanceFromSchoolToTrustHeadquarters;
			DistanceFromSchoolToTrustHeadquartersAdditionalInformation = project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation;
			GeneralInformationSectionComplete = project.GeneralInformationSectionComplete ?? false;

			SchoolPerformanceAdditionalInformation = project.SchoolPerformanceAdditionalInformation;

			RationaleForProject = project.RationaleForProject;
			RationaleForTrust = project.RationaleForTrust;
			RationaleSectionComplete = project.RationaleSectionComplete ?? false;

			RisksAndIssues = project.RisksAndIssues;
			EqualitiesImpactAssessmentConsidered = project.EqualitiesImpactAssessmentConsidered;
			RisksAndIssuesSectionComplete = project.RisksAndIssuesSectionComplete ?? false;

			YearOneProjectedCapacity = project.YearOneProjectedCapacity;
			YearOneProjectedPupilNumbers = project.YearOneProjectedPupilNumbers;
			YearTwoProjectedCapacity = project.YearTwoProjectedCapacity;
			YearTwoProjectedPupilNumbers = project.YearTwoProjectedPupilNumbers;
			YearThreeProjectedCapacity = project.YearThreeProjectedCapacity;
			YearThreeProjectedPupilNumbers = project.YearThreeProjectedPupilNumbers;
			YearFourProjectedCapacity = project.YearFourProjectedCapacity;
			YearFourProjectedPupilNumbers = project.YearFourProjectedPupilNumbers;
			SchoolPupilForecastsAdditionalInformation = project.SchoolPupilForecastsAdditionalInformation;

			RevenueCarryForwardAtEndMarchCurrentYear = project.RevenueCarryForwardAtEndMarchCurrentYear ?? 0;
			ProjectedRevenueBalanceAtEndMarchNextYear = project.ProjectedRevenueBalanceAtEndMarchNextYear ?? 0;
			CapitalCarryForwardAtEndMarchCurrentYear = project.CapitalCarryForwardAtEndMarchCurrentYear ?? 0;
			CapitalCarryForwardAtEndMarchNextYear = project.CapitalCarryForwardAtEndMarchNextYear ?? 0;
			SchoolBudgetInformationAdditionalInformation = project.SchoolBudgetInformationAdditionalInformation;
			SchoolBudgetInformationSectionComplete = project.SchoolBudgetInformationSectionComplete ?? false;

			KeyStage2PerformanceAdditionalInformation = project.KeyStage2PerformanceAdditionalInformation;
			KeyStage4PerformanceAdditionalInformation = project.KeyStage4PerformanceAdditionalInformation;
			KeyStage5PerformanceAdditionalInformation = project.KeyStage5PerformanceAdditionalInformation;
		}

		public string Id { get; }
		public string SchoolName { get; }
		public string SchoolURN { get; }
		public string LocalAuthority { get; }
		public string ApplicationReceivedDate { get; }
		public string AssignedDate { get; }
		public string Phase { get; }
		public DateTime? HeadTeacherBoardDate { get; set; }

		public DateTime? LocalAuthorityInformationTemplateSentDate { get; set; }
		public DateTime? LocalAuthorityInformationTemplateReturnedDate { get; set; }
		public string LocalAuthorityInformationTemplateComments { get; set; }
		public string LocalAuthorityInformationTemplateLink { get; set; }
		public bool LocalAuthorityInformationTemplateSectionComplete { get; set; }

		//school/trust info
		public string RecommendationForProject { get; set; }
		public string Author { get; set; }
		public string Version { get; set; }
		public string ClearedBy { get; set; }
		public string AcademyOrderRequired { get; set; }
		public string PreviousHeadTeacherBoardDateQuestion { get; set; }
		public DateTime? PreviousHeadTeacherBoardDate { get; set; }
		public string PreviousHeadTeacherBoardLink { get; set; }
		public string TrustReferenceNumber { get; set; }
		public string NameOfTrust { get; set; }
		public string SponsorReferenceNumber { get; set; }
		public string SponsorName { get; set; }
		public string AcademyTypeAndRoute { get; set; }
		public DateTime? ProposedAcademyOpeningDate { get; set; }
		public bool SchoolAndTrustInformationSectionComplete { get; set; }

		//general info
		public string PublishedAdmissionNumber { get; set; }
		public string ViabilityIssues { get; set; }
		public string FinancialDeficit { get; set; }
		public decimal? DistanceFromSchoolToTrustHeadquarters { get; set; }
		public string DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; set; }
		public bool GeneralInformationSectionComplete { get; set; }

		//school performance ofsted information
		public string SchoolPerformanceAdditionalInformation { get; set; }

		public string RationaleForProject { get; set; }
		public string RationaleForTrust { get; set; }
		public bool RationaleSectionComplete { get; set; }

		// risk and issues
		public string RisksAndIssues { get; set; }
		public string EqualitiesImpactAssessmentConsidered { get; set; }
		public bool RisksAndIssuesSectionComplete { get; set; }

		// pupil schools forecast
		public int? YearOneProjectedCapacity { get; set; }
		public int? YearOneProjectedPupilNumbers { get; set; }
		public int? YearTwoProjectedCapacity { get; set; }
		public int? YearTwoProjectedPupilNumbers { get; set; }
		public int? YearThreeProjectedCapacity { get; set; }
		public int? YearThreeProjectedPupilNumbers { get; set; }
		public int? YearFourProjectedCapacity { get; set; }
		public int? YearFourProjectedPupilNumbers { get; set; }
		public string SchoolPupilForecastsAdditionalInformation { get; set; }

		//school budget info
		public decimal RevenueCarryForwardAtEndMarchCurrentYear { get; set; }
		public decimal ProjectedRevenueBalanceAtEndMarchNextYear { get; set; }
		public decimal CapitalCarryForwardAtEndMarchCurrentYear { get; set; }
		public decimal CapitalCarryForwardAtEndMarchNextYear { get; set; }
		public string SchoolBudgetInformationAdditionalInformation { get; set; }
		public bool SchoolBudgetInformationSectionComplete { get; set; }

		public string KeyStage2PerformanceAdditionalInformation { get; set; }
		public string KeyStage4PerformanceAdditionalInformation { get; set; }
		public string KeyStage5PerformanceAdditionalInformation { get; set; }
	}
}
