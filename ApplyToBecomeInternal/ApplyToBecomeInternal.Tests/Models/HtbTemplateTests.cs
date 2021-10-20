using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.KeyStagePerformance;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models;
using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models
{
	public class HtbTemplateTests
	{
		public class WholeTemplateTests
		{
			private readonly HtbTemplate _template;
			private readonly AcademyConversionProject _project;
			private readonly SchoolPerformance _schoolPerformance;
			private readonly GeneralInformation _generalInformation;
			private readonly KeyStagePerformance _keyStagePerformance;

			public WholeTemplateTests()
			{
				var fixture = new Fixture();
				_project = fixture.Create<AcademyConversionProject>();
				_schoolPerformance = fixture.Create<SchoolPerformance>();
				_generalInformation = fixture.Create<GeneralInformation>();
				_keyStagePerformance = new KeyStagePerformance
				{
					KeyStage2 = fixture.CreateMany<KeyStage2PerformanceResponse>(3)
				};

				_template = HtbTemplate.Build(_project, _schoolPerformance, _generalInformation, _keyStagePerformance);
			}

			[Fact]
			public void ItBuildsTheTemplateSuccessfully()
			{
				// CML are these asserts the wrong way round? - i.e. actual, expected
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
				Assert.Equal(_template.AcademyTypeRouteAndConversionGrant, $"{_project.AcademyTypeAndRoute} - {_project.ConversionSupportGrantAmount?.ToMoneyString(true)}");
				Assert.Equal(_template.ConversionSupportGrantChangeReason, _project.ConversionSupportGrantChangeReason);
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
				Assert.Equal(_template.DistanceFromSchoolToTrustHeadquarters, $"{_project.DistanceFromSchoolToTrustHeadquarters.ToSafeString()} miles");
				Assert.Equal(_template.DistanceFromSchoolToTrustHeadquartersAdditionalInformation, _project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation);
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


		public class KeyStagePerformanceTests
		{
			private readonly AcademyConversionProject _project;
			private readonly SchoolPerformance _schoolPerformance;
			private readonly GeneralInformation _generalInformation;
			private readonly Fixture _fixture;

			public KeyStagePerformanceTests()
			{
				_fixture = new Fixture();
				_project = _fixture.Create<AcademyConversionProject>();
				_schoolPerformance = _fixture.Create<SchoolPerformance>();
				_generalInformation = _fixture.Create<GeneralInformation>();
			}

			[Fact]
			public void GivenNoKeyStage2DataToDisplay_DoesNotPopulateKeyStage2Data()
			{
				var keyStagePerformance = new KeyStagePerformance();
				var template = HtbTemplate.Build(_project, _schoolPerformance, _generalInformation, keyStagePerformance);
				
				Assert.Null(template.KeyStage2);
			}
			
			[Fact]
			public void GivenNoKeyStage4DataToDisplay_DoesNotPopulateKeyStage4Data()
			{
				var keyStagePerformance = new KeyStagePerformance();
				var template = HtbTemplate.Build(_project, _schoolPerformance, _generalInformation, keyStagePerformance);
				
				Assert.Null(template.KeyStage4);
			}
			
			[Fact]
			public void GivenNoKeyStage5DataToDisplay_DoesNotPopulateKeyStage5Data()
			{
				var keyStagePerformance = new KeyStagePerformance();
				var template = HtbTemplate.Build(_project, _schoolPerformance, _generalInformation, keyStagePerformance);
				
				Assert.Null(template.KeyStage5);
			}

			[Fact]
			public void GivenKeyStageData_PopulatesKeyStageData()
			{
				var keyStagePerformance = new KeyStagePerformance
				{
					KeyStage2 = _fixture.CreateMany<KeyStage2PerformanceResponse>(4),
					KeyStage4 = _fixture.CreateMany<KeyStage4PerformanceResponse>(3),
					KeyStage5 = _fixture.CreateMany<KeyStage5PerformanceResponse>(2)
				};

				var template = HtbTemplate.Build(_project, _schoolPerformance, _generalInformation, keyStagePerformance);

				Assert.NotNull(template.KeyStage2);
				Assert.Equal(4, template.KeyStage2.Count());
				Assert.NotNull(template.KeyStage4);
				Assert.NotNull(template.KeyStage5);
				Assert.Equal(2, template.KeyStage5.Count());
			}
		}
	}
}