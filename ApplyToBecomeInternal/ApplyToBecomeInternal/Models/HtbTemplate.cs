using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Extensions;
using System;
using static ApplyToBecomeInternal.Services.WordDocumentService;

namespace ApplyToBecomeInternal.Models
{
	public class HtbTemplate
	{
		[PlaceholderText("SchoolUrn")]
		public string SchoolUrn { get; set; }
		[PlaceholderText("SchoolName")]
		public string SchoolName { get; set; }
		[PlaceholderText("LocalAuthority")]
		public string LocalAuthority { get; set; }
		public string ApplicationReferenceNumber { get; set; }
		public string ProjectStatus { get; set; }
		public string ApplicationReceivedDate { get; set; }
		public string AssignedDate { get; set; }
		[PlaceholderText("HeadTeacherBoardDate")]
		public string HeadTeacherBoardDate { get; set; }
		public string OpeningDate { get; set; }
		public string BaselineDate { get; set; }

		//school/trust info
		[PlaceholderText("RecommendationForProject")]
		public string RecommendationForProject { get; set; }
		[PlaceholderText("Author")]
		public string Author { get; set; }
		[PlaceholderText("Version")]
		public string Version { get; set; }
		[PlaceholderText("ClearedBy")]
		public string ClearedBy { get; set; }
		[PlaceholderText("AcademyOrderRequired")]
		public string AcademyOrderRequired { get; set; }
		[PlaceholderText("PreviousHeadTeacherBoardDate")]
		public string PreviousHeadTeacherBoardDate { get; set; }
		public string PreviousHeadTeacherBoardLink { get; set; }
		[PlaceholderText("TrustReferenceNumber")]
		public string TrustReferenceNumber { get; set; }
		[PlaceholderText("TrustName")]
		public string NameOfTrust { get; set; }
		[PlaceholderText("SponsorReferenceNumber")]
		public string SponsorReferenceNumber { get; set; }
		[PlaceholderText("SponsorName")]
		public string SponsorName { get; set; }
		[PlaceholderText("AcademyTypeAndRoute")]
		public string AcademyTypeAndRoute { get; set; }
		[PlaceholderText("ProposedAcademyOpeningDate")]
		public string ProposedAcademyOpeningDate { get; set; }

		//general info
		[PlaceholderText("SchoolPhase")]
		public string SchoolPhase { get; set; }
		[PlaceholderText("AgeRange")]
		public string AgeRange { get; set; }
		[PlaceholderText("SchoolType")]
		public string SchoolType { get; set; }
		[PlaceholderText("NumberOnRoll")]
		public string NumberOnRoll { get; set; }
		[PlaceholderText("PercentageSchoolFull")]
		public string PercentageSchoolFull { get; set; }
		[PlaceholderText("SchoolCapacity")]
		public string SchoolCapacity { get; set; }
		[PlaceholderText("PublishedAdmissionNumber")]
		public string PublishedAdmissionNumber { get; set; }
		[PlaceholderText("PercentageFreeSchoolMeals")]
		public string PercentageFreeSchoolMeals { get; set; }
		[PlaceholderText("PartOfPfiScheme")]
		public string PartOfPfiScheme { get; set; }
		[PlaceholderText("ViabilityIssues")]
		public string ViabilityIssues { get; set; }
		[PlaceholderText("FinancialDeficit")]
		public string FinancialDeficit { get; set; }
		[PlaceholderText("IsSchoolLinkedToADiocese")]
		public string IsSchoolLinkedToADiocese { get; set; }
		[PlaceholderText("PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust")]
		public string PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust { get; set; }
		[PlaceholderText("DistanceFromSchoolToTrustHeadquarters")]
		public string DistanceFromSchoolToTrustHeadquarters { get; set; }
		[PlaceholderText("DistanceFromSchoolToTrustHeadquartersAdditionalInformation")]
		public string DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; set; }
		[PlaceholderText("ParliamentaryConstituency")]
		public string ParliamentaryConstituency { get; set; }

		//school performance ofsted information
		[PlaceholderText("PersonalDevelopment")]
		public string PersonalDevelopment { get; set; }
		[PlaceholderText("BehaviourAndAttitudes")]
		public string BehaviourAndAttitudes { get; set; }
		[PlaceholderText("EarlyYearsProvision")]
		public string EarlyYearsProvision { get; set; }
		[PlaceholderText("OfstedLastInspection")]
		public string OfstedLastInspection { get; set; }
		[PlaceholderText("EffectivenessOfLeadershipAndManagement")]
		public string EffectivenessOfLeadershipAndManagement { get; set; }
		[PlaceholderText("OverallEffectiveness")]
		public string OverallEffectiveness { get; set; }
		[PlaceholderText("QualityOfEducation")]
		public string QualityOfEducation { get; set; }
		[PlaceholderText("SixthFormProvision")]
		public string SixthFormProvision { get; set; }
		[PlaceholderText("SchoolPerformanceAdditionalInformation")]
		public string SchoolPerformanceAdditionalInformation { get; set; }

		// rationale
		[PlaceholderText("RationaleForProject")]
		public string RationaleForProject { get; set; }
		[PlaceholderText("RationaleForTrust")]
		public string RationaleForTrust { get; set; }

		// risk and issues
		[PlaceholderText("RisksAndIssues")]
		public string RisksAndIssues { get; set; }
		[PlaceholderText("EqualitiesImpactAssessmentConsidered")]
		public string EqualitiesImpactAssessmentConsidered { get; set; }

		// school budget info
		[PlaceholderText("RevenueCarryForwardAtEndMarchCurrentYear")]
		public string RevenueCarryForwardAtEndMarchCurrentYear { get; set; }
		[PlaceholderText("ProjectedRevenueBalanceAtEndMarchNextYear")]
		public string ProjectedRevenueBalanceAtEndMarchNextYear { get; set; }
		[PlaceholderText("CapitalCarryForwardAtEndMarchCurrentYear")]
		public string CapitalCarryForwardAtEndMarchCurrentYear { get; set; }
		[PlaceholderText("CapitalCarryForwardAtEndMarchNextYear")]
		public string CapitalCarryForwardAtEndMarchNextYear { get; set; }
		[PlaceholderText("SchoolBudgetInformationAdditionalInformation")]
		public string SchoolBudgetInformationAdditionalInformation { get; set; }

		public static HtbTemplate Build(AcademyConversionProject project, SchoolPerformance schoolPerformance, GeneralInformation generalInformation)
		{
			return new HtbTemplate
			{
				SchoolName = project.SchoolName,
				SchoolUrn = project.Urn.ToString(),
				LocalAuthority = project.LocalAuthority,
				ApplicationReceivedDate = project.ApplicationReceivedDate.ToDateString(),
				AssignedDate = project.AssignedDate.ToDateString(),
				HeadTeacherBoardDate = project.HeadTeacherBoardDate.ToDateString(),

				RecommendationForProject = project.RecommendationForProject,
				Author = project.Author,
				Version = DateTime.Today.ToDateString(),
				ClearedBy = project.ClearedBy,
				AcademyOrderRequired = project.AcademyOrderRequired,
				PreviousHeadTeacherBoardDate = project.PreviousHeadTeacherBoardDate.ToDateString(),
				PreviousHeadTeacherBoardLink = project.PreviousHeadTeacherBoardLink,
				TrustReferenceNumber = project.TrustReferenceNumber,
				NameOfTrust = project.NameOfTrust,
				SponsorReferenceNumber = project.SponsorReferenceNumber,
				SponsorName = project.SponsorName,
				AcademyTypeAndRoute = project.AcademyTypeAndRoute,
				ProposedAcademyOpeningDate = project.ProposedAcademyOpeningDate.ToDateString(),

				SchoolPhase = generalInformation.SchoolPhase,
				AgeRange = !string.IsNullOrEmpty(generalInformation.AgeRangeLower) && !string.IsNullOrEmpty(generalInformation.AgeRangeUpper)
					? $"{generalInformation.AgeRangeLower} to {generalInformation.AgeRangeUpper}"
					: "",
				SchoolType = generalInformation.SchoolType,
				NumberOnRoll = generalInformation.NumberOnRoll?.ToString(),
				PercentageSchoolFull = generalInformation.NumberOnRoll.AsPercentageOf(generalInformation.SchoolCapacity),
				SchoolCapacity = generalInformation.SchoolCapacity?.ToString(),
				PublishedAdmissionNumber = project.PublishedAdmissionNumber,
				PercentageFreeSchoolMeals = !string.IsNullOrEmpty(generalInformation.PercentageFreeSchoolMeals) ? $"{generalInformation.PercentageFreeSchoolMeals}%" : "",
				PartOfPfiScheme = project.PartOfPfiScheme,
				ViabilityIssues = project.ViabilityIssues,
				FinancialDeficit = project.FinancialDeficit,
				IsSchoolLinkedToADiocese = generalInformation.IsSchoolLinkedToADiocese,
				DistanceFromSchoolToTrustHeadquarters = project.DistanceFromSchoolToTrustHeadquarters.ToSafeString(),
				DistanceFromSchoolToTrustHeadquartersAdditionalInformation = project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
				ParliamentaryConstituency = generalInformation.ParliamentaryConstituency,

				OfstedLastInspection = schoolPerformance.OfstedLastInspection.ToDateString(),
				PersonalDevelopment = schoolPerformance.PersonalDevelopment.DisplayOfstedRating(),
				BehaviourAndAttitudes = schoolPerformance.BehaviourAndAttitudes.DisplayOfstedRating(),
				EarlyYearsProvision = schoolPerformance.EarlyYearsProvision.DisplayOfstedRating(),
				EffectivenessOfLeadershipAndManagement = schoolPerformance.EffectivenessOfLeadershipAndManagement.DisplayOfstedRating(),
				OverallEffectiveness = schoolPerformance.OverallEffectiveness.DisplayOfstedRating(),
				QualityOfEducation = schoolPerformance.QualityOfEducation.DisplayOfstedRating(),
				SixthFormProvision = schoolPerformance.SixthFormProvision.DisplayOfstedRating(),
				SchoolPerformanceAdditionalInformation = project.SchoolPerformanceAdditionalInformation,

				RationaleForProject = project.RationaleForProject,
				RationaleForTrust = project.RationaleForTrust,

				RisksAndIssues = project.RisksAndIssues,
				EqualitiesImpactAssessmentConsidered = project.EqualitiesImpactAssessmentConsidered,

				RevenueCarryForwardAtEndMarchCurrentYear = (project.RevenueCarryForwardAtEndMarchCurrentYear ?? 0).ToMoneyString(),
				ProjectedRevenueBalanceAtEndMarchNextYear = (project.ProjectedRevenueBalanceAtEndMarchNextYear ?? 0).ToMoneyString(),
				CapitalCarryForwardAtEndMarchCurrentYear = (project.CapitalCarryForwardAtEndMarchCurrentYear ?? 0).ToMoneyString(),
				CapitalCarryForwardAtEndMarchNextYear = (project.CapitalCarryForwardAtEndMarchNextYear ?? 0).ToMoneyString(),
				SchoolBudgetInformationAdditionalInformation = project.SchoolBudgetInformationAdditionalInformation
			};
		}
	}
}
