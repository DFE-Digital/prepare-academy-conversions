using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Services.WordDocument;
using System;

namespace ApplyToBecomeInternal.Models
{
	public class HtbTemplate
	{
		[DocumentText("SchoolUrn")]
		public string SchoolUrn { get; set; }
		[DocumentText("SchoolName")]
		public string SchoolName { get; set; }
		[DocumentText("LocalAuthority")]
		public string LocalAuthority { get; set; }
		public string ApplicationReferenceNumber { get; set; }
		public string ProjectStatus { get; set; }
		public string ApplicationReceivedDate { get; set; }
		public string AssignedDate { get; set; }
		[DocumentText("HeadTeacherBoardDate")]
		public string HeadTeacherBoardDate { get; set; }
		public string BaselineDate { get; set; }

		//school/trust info
		[DocumentText("RecommendationForProject")]
		public string RecommendationForProject { get; set; }
		[DocumentText("Author")]
		public string Author { get; set; }
		[DocumentText("Version")]
		public string Version { get; set; }
		[DocumentText("ClearedBy")]
		public string ClearedBy { get; set; }
		[DocumentText("AcademyOrderRequired")]
		public string AcademyOrderRequired { get; set; }
		[DocumentText("PreviousHeadTeacherBoardDate")]
		public string PreviousHeadTeacherBoardDate { get; set; }
		public string PreviousHeadTeacherBoardLink { get; set; }
		[DocumentText("TrustReferenceNumber")]
		public string TrustReferenceNumber { get; set; }
		[DocumentText("TrustName")]
		public string NameOfTrust { get; set; }
		[DocumentText("SponsorReferenceNumber")]
		public string SponsorReferenceNumber { get; set; }
		[DocumentText("SponsorName")]
		public string SponsorName { get; set; }
		[DocumentText("AcademyTypeAndRoute")]
		public string AcademyTypeAndRoute { get; set; }
		[DocumentText("ProposedAcademyOpeningDate")]
		public string ProposedAcademyOpeningDate { get; set; }

		//general info
		[DocumentText("SchoolPhase")]
		public string SchoolPhase { get; set; }
		[DocumentText("AgeRange")]
		public string AgeRange { get; set; }
		[DocumentText("SchoolType")]
		public string SchoolType { get; set; }
		[DocumentText("NumberOnRoll")]
		public string NumberOnRoll { get; set; }
		[DocumentText("PercentageSchoolFull")]
		public string PercentageSchoolFull { get; set; }
		[DocumentText("SchoolCapacity")]
		public string SchoolCapacity { get; set; }
		[DocumentText("PublishedAdmissionNumber")]
		public string PublishedAdmissionNumber { get; set; }
		[DocumentText("PercentageFreeSchoolMeals")]
		public string PercentageFreeSchoolMeals { get; set; }
		[DocumentText("PartOfPfiScheme")]
		public string PartOfPfiScheme { get; set; }
		[DocumentText("ViabilityIssues")]
		public string ViabilityIssues { get; set; }
		[DocumentText("FinancialDeficit")]
		public string FinancialDeficit { get; set; }
		[DocumentText("IsSchoolLinkedToADiocese")]
		public string IsSchoolLinkedToADiocese { get; set; }
		[DocumentText("PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust")]
		public string PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust { get; set; }
		[DocumentText("DistanceFromSchoolToTrustHeadquarters", IsRichText = true)]
		public string DistanceFromSchoolToTrustHeadquarters { get; set; }
		[DocumentText("DistanceFromSchoolToTrustHeadquartersAdditionalInformation")]
		public string DistanceFromSchoolToTrustHeadquartersAdditionalInformation { get; set; }
		[DocumentText("ParliamentaryConstituency")]
		public string ParliamentaryConstituency { get; set; }

		//school performance ofsted information
		[DocumentText("PersonalDevelopment")]
		public string PersonalDevelopment { get; set; }
		[DocumentText("BehaviourAndAttitudes")]
		public string BehaviourAndAttitudes { get; set; }
		[DocumentText("EarlyYearsProvision")]
		public string EarlyYearsProvision { get; set; }
		[DocumentText("OfstedLastInspection")]
		public string OfstedLastInspection { get; set; }
		[DocumentText("EffectivenessOfLeadershipAndManagement")]
		public string EffectivenessOfLeadershipAndManagement { get; set; }
		[DocumentText("OverallEffectiveness")]
		public string OverallEffectiveness { get; set; }
		[DocumentText("QualityOfEducation")]
		public string QualityOfEducation { get; set; }
		[DocumentText("SixthFormProvision")]
		public string SixthFormProvision { get; set; }
		[DocumentText("SchoolPerformanceAdditionalInformation")]
		public string SchoolPerformanceAdditionalInformation { get; set; }

		// rationale
		[DocumentText("RationaleForProject", IsRichText = true)]
		public string RationaleForProject { get; set; }
		[DocumentText("RationaleForTrust", IsRichText = true)]
		public string RationaleForTrust { get; set; }

		// risk and issues
		[DocumentText("RisksAndIssues", IsRichText = true)]
		public string RisksAndIssues { get; set; }
		[DocumentText("EqualitiesImpactAssessmentConsidered")]
		public string EqualitiesImpactAssessmentConsidered { get; set; }

		// school budget info
		[DocumentText("RevenueCarryForwardAtEndMarchCurrentYear")]
		public string RevenueCarryForwardAtEndMarchCurrentYear { get; set; }
		[DocumentText("ProjectedRevenueBalanceAtEndMarchNextYear")]
		public string ProjectedRevenueBalanceAtEndMarchNextYear { get; set; }
		[DocumentText("CapitalCarryForwardAtEndMarchCurrentYear")]
		public string CapitalCarryForwardAtEndMarchCurrentYear { get; set; }
		[DocumentText("CapitalCarryForwardAtEndMarchNextYear")]
		public string CapitalCarryForwardAtEndMarchNextYear { get; set; }
		[DocumentText("SchoolBudgetInformationAdditionalInformation")]
		public string SchoolBudgetInformationAdditionalInformation { get; set; }

		// school pupil forecasts
		[DocumentText("YearOneProjectedCapacity")]
		public string YearOneProjectedCapacity { get; set; }
		[DocumentText("YearOneProjectedPupilNumbers")]
		public string YearOneProjectedPupilNumbers { get; set; }
		[DocumentText("YearOnePercentageSchoolFull")]
		public string YearOnePercentageSchoolFull { get; set; }
		[DocumentText("YearTwoProjectedCapacity")]
		public string YearTwoProjectedCapacity { get; set; }
		[DocumentText("YearTwoProjectedPupilNumbers")]
		public string YearTwoProjectedPupilNumbers { get; set; }
		[DocumentText("YearTwoPercentageSchoolFull")]
		public string YearTwoPercentageSchoolFull { get; set; }
		[DocumentText("YearThreeProjectedCapacity")]
		public string YearThreeProjectedCapacity { get; set; }
		[DocumentText("YearThreeProjectedPupilNumbers")]
		public string YearThreeProjectedPupilNumbers { get; set; }
		[DocumentText("YearThreePercentageSchoolFull")]
		public string YearThreePercentageSchoolFull { get; set; }
		[DocumentText("SchoolPupilForecastsAdditionalInformation")]
		public string SchoolPupilForecastsAdditionalInformation { get; set; }

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
				DistanceFromSchoolToTrustHeadquarters = $"{project.DistanceFromSchoolToTrustHeadquarters.ToSafeString()}<br>{project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation}",
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

				RevenueCarryForwardAtEndMarchCurrentYear = project.RevenueCarryForwardAtEndMarchCurrentYear?.ToMoneyString(),
				ProjectedRevenueBalanceAtEndMarchNextYear = project.ProjectedRevenueBalanceAtEndMarchNextYear?.ToMoneyString(),
				CapitalCarryForwardAtEndMarchCurrentYear = project.CapitalCarryForwardAtEndMarchCurrentYear?.ToMoneyString(),
				CapitalCarryForwardAtEndMarchNextYear = project.CapitalCarryForwardAtEndMarchNextYear?.ToMoneyString(),
				SchoolBudgetInformationAdditionalInformation = project.SchoolBudgetInformationAdditionalInformation,

				YearOneProjectedCapacity = project.YearOneProjectedCapacity.ToString(),
				YearOneProjectedPupilNumbers = project.YearOneProjectedPupilNumbers.ToStringOrDefault(),
				YearOnePercentageSchoolFull = project.YearOneProjectedPupilNumbers.AsPercentageOf(project.YearOneProjectedCapacity),
				YearTwoProjectedCapacity = project.YearTwoProjectedCapacity.ToString(),
				YearTwoProjectedPupilNumbers = project.YearTwoProjectedPupilNumbers.ToString(),
				YearTwoPercentageSchoolFull = project.YearTwoProjectedPupilNumbers.AsPercentageOf(project.YearTwoProjectedCapacity),
				YearThreeProjectedCapacity = project.YearThreeProjectedCapacity.ToString(),
				YearThreeProjectedPupilNumbers = project.YearThreeProjectedPupilNumbers.ToString(),
				YearThreePercentageSchoolFull = project.YearThreeProjectedPupilNumbers.AsPercentageOf(project.YearThreeProjectedCapacity),
				SchoolPupilForecastsAdditionalInformation = project.SchoolPupilForecastsAdditionalInformation
			};
		}
	}
}
