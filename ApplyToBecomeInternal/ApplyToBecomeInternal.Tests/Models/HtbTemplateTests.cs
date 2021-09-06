using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models;
using AutoFixture;
using System;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models
{
	public class HtbTemplateTests
	{
		private readonly AcademyConversionProject _project;
		private readonly SchoolPerformance _schoolPerformance;
		private readonly GeneralInformation _generalInformation;
		private readonly HtbTemplate _template;

		public HtbTemplateTests()
		{
			var fixture = new Fixture();
			_project = fixture.Create<AcademyConversionProject>();
			_schoolPerformance = fixture.Create<SchoolPerformance>();
			_generalInformation = fixture.Create<GeneralInformation>();

			_template = HtbTemplate.Build(_project, _schoolPerformance, _generalInformation);
		}

		[Fact]
		public void ItBuildsTheTemplateSuccessfully()
		{
			Assert.Equal(_template.SchoolName, _project.SchoolName);
			Assert.Equal(_template.SchoolUrn, _project.Urn.ToString());
			Assert.Equal(_template.LocalAuthority, _project.LocalAuthority);
			Assert.Equal(_template.ApplicationReceivedDate, _project.ApplicationReceivedDate.ToDateString());
			Assert.Equal(_template.AssignedDate, _project.AssignedDate.ToDateString());
			Assert.Equal(_template.HeadTeacherBoardDate, _project.HeadTeacherBoardDate.ToDateString());

			Assert.Equal(_template.RecommendationForProject, _project.RecommendationForProject);
			Assert.Equal(_template.Author, _project.Author);
			Assert.Equal(_template.Version, DateTime.Today.ToDateString());
			Assert.Equal(_template.ClearedBy, _project.ClearedBy);
			Assert.Equal(_template.AcademyOrderRequired, _project.AcademyOrderRequired);
			Assert.Equal(_template.PreviousHeadTeacherBoardDate, _project.PreviousHeadTeacherBoardDate.ToDateString());
			Assert.Equal(_template.PreviousHeadTeacherBoardLink, _project.PreviousHeadTeacherBoardLink);
			Assert.Equal(_template.TrustReferenceNumber, _project.TrustReferenceNumber);
			Assert.Equal(_template.NameOfTrust, _project.NameOfTrust);
			Assert.Equal(_template.SponsorReferenceNumber, _project.SponsorReferenceNumber);
			Assert.Equal(_template.SponsorName, _project.SponsorName);
			Assert.Equal(_template.AcademyTypeAndRoute, _project.AcademyTypeAndRoute);
			Assert.Equal(_template.ProposedAcademyOpeningDate, _project.ProposedAcademyOpeningDate.ToDateString());

			Assert.Equal(_template.SchoolPhase, _generalInformation.SchoolPhase);
			Assert.Equal(_template.AgeRange, $"{_generalInformation.AgeRangeLower} to {_generalInformation.AgeRangeUpper}");
			Assert.Equal(_template.SchoolType, _generalInformation.SchoolType);
			Assert.Equal(_template.NumberOnRoll, _generalInformation.NumberOnRoll?.ToString());
			Assert.Equal(_template.PercentageSchoolFull, _generalInformation.NumberOnRoll.AsPercentageOf(_generalInformation.SchoolCapacity));
			Assert.Equal(_template.SchoolCapacity, _generalInformation.SchoolCapacity?.ToString());
			Assert.Equal(_template.PublishedAdmissionNumber, _project.PublishedAdmissionNumber);
			Assert.Equal(_template.PercentageFreeSchoolMeals, $"{_generalInformation.PercentageFreeSchoolMeals}%");
			Assert.Equal(_template.PartOfPfiScheme, _project.PartOfPfiScheme);
			Assert.Equal(_template.ViabilityIssues, _project.ViabilityIssues);
			Assert.Equal(_template.FinancialDeficit, _project.FinancialDeficit);
			Assert.Equal(_template.IsSchoolLinkedToADiocese, _generalInformation.IsSchoolLinkedToADiocese);
			Assert.Equal(_template.DistanceFromSchoolToTrustHeadquarters,
				$"{_project.DistanceFromSchoolToTrustHeadquarters.ToSafeString()}<br>{_project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation}");
			Assert.Equal(_template.ParliamentaryConstituency, _generalInformation.ParliamentaryConstituency);

			Assert.Equal(_template.OfstedLastInspection, _schoolPerformance.OfstedLastInspection.ToDateString());
			Assert.Equal(_template.PersonalDevelopment, _schoolPerformance.PersonalDevelopment.DisplayOfstedRating());
			Assert.Equal(_template.BehaviourAndAttitudes, _schoolPerformance.BehaviourAndAttitudes.DisplayOfstedRating());
			Assert.Equal(_template.EarlyYearsProvision, _schoolPerformance.EarlyYearsProvision.DisplayOfstedRating());
			Assert.Equal(_template.EffectivenessOfLeadershipAndManagement, _schoolPerformance.EffectivenessOfLeadershipAndManagement.DisplayOfstedRating());
			Assert.Equal(_template.OverallEffectiveness, _schoolPerformance.OverallEffectiveness.DisplayOfstedRating());
			Assert.Equal(_template.QualityOfEducation, _schoolPerformance.QualityOfEducation.DisplayOfstedRating());
			Assert.Equal(_template.SixthFormProvision, _schoolPerformance.SixthFormProvision.DisplayOfstedRating());
			Assert.Equal(_template.SchoolPerformanceAdditionalInformation, _project.SchoolPerformanceAdditionalInformation);

			Assert.Equal(_template.RationaleForProject, _project.RationaleForProject);
			Assert.Equal(_template.RationaleForTrust, _project.RationaleForTrust);

			Assert.Equal(_template.RisksAndIssues, _project.RisksAndIssues);
			Assert.Equal(_template.EqualitiesImpactAssessmentConsidered, _project.EqualitiesImpactAssessmentConsidered);

			Assert.Equal(_template.RevenueCarryForwardAtEndMarchCurrentYear, _project.RevenueCarryForwardAtEndMarchCurrentYear?.ToMoneyString());
			Assert.Equal(_template.ProjectedRevenueBalanceAtEndMarchNextYear, _project.ProjectedRevenueBalanceAtEndMarchNextYear?.ToMoneyString());
			Assert.Equal(_template.CapitalCarryForwardAtEndMarchCurrentYear, _project.CapitalCarryForwardAtEndMarchCurrentYear?.ToMoneyString());
			Assert.Equal(_template.CapitalCarryForwardAtEndMarchNextYear, _project.CapitalCarryForwardAtEndMarchNextYear?.ToMoneyString());
			Assert.Equal(_template.SchoolBudgetInformationAdditionalInformation, _project.SchoolBudgetInformationAdditionalInformation);

			Assert.Equal(_template.YearOneProjectedCapacity, _project.YearOneProjectedCapacity.ToString());
			Assert.Equal(_template.YearOneProjectedPupilNumbers, _project.YearOneProjectedPupilNumbers.ToStringOrDefault());
			Assert.Equal(_template.YearOnePercentageSchoolFull, _project.YearOneProjectedPupilNumbers.AsPercentageOf(_project.YearOneProjectedCapacity));
			Assert.Equal(_template.YearTwoProjectedCapacity, _project.YearTwoProjectedCapacity.ToString());
			Assert.Equal(_template.YearTwoProjectedPupilNumbers, _project.YearTwoProjectedPupilNumbers.ToString());
			Assert.Equal(_template.YearTwoPercentageSchoolFull, _project.YearTwoProjectedPupilNumbers.AsPercentageOf(_project.YearTwoProjectedCapacity));
			Assert.Equal(_template.YearThreeProjectedCapacity, _project.YearThreeProjectedCapacity.ToString());
			Assert.Equal(_template.YearThreeProjectedPupilNumbers, _project.YearThreeProjectedPupilNumbers.ToString());
			Assert.Equal(_template.YearThreePercentageSchoolFull, _project.YearThreeProjectedPupilNumbers.AsPercentageOf(_project.YearThreeProjectedCapacity));
			Assert.Equal(_template.SchoolPupilForecastsAdditionalInformation, _project.SchoolPupilForecastsAdditionalInformation);
		}
	}
}