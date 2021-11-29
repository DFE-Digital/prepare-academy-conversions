using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.KeyStagePerformance;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models;
using AutoFixture;
using System;
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
			public void ItBuildsTheSchoolAndTrustInformationAndProjectDatesSuccessfully()
			{
				Assert.Equal(_project.SchoolName, _template.SchoolName);
				Assert.Equal(_project.Urn.ToString(), _template.SchoolUrn);
				Assert.Equal($"{_project.SchoolName} - URN {_project.Urn}", _template.SchoolNameAndUrn);
				Assert.Equal($"{_project.NameOfTrust} - {_project.TrustReferenceNumber}", _template.TrustNameAndReferenceNumber);
				Assert.Equal(_project.LocalAuthority, _template.LocalAuthority);

				Assert.Equal(_project.RecommendationForProject, _template.RecommendationForProject);
				Assert.Equal(_project.AcademyOrderRequired, _template.AcademyOrderRequired);
				Assert.Equal($"{_project.AcademyTypeAndRoute} - {_project.ConversionSupportGrantAmount?.ToMoneyString(true)}", _template.AcademyTypeRouteAndConversionGrant);
				Assert.Equal(_project.HeadTeacherBoardDate.ToDateString(), _template.HeadTeacherBoardDate);
				Assert.Equal(_project.ProposedAcademyOpeningDate.ToDateString(), _template.ProposedAcademyOpeningDate);
				Assert.Equal(_project.PreviousHeadTeacherBoardDate.ToDateString(), _template.PreviousHeadTeacherBoardDate);

				Assert.Equal(_project.TrustReferenceNumber, _template.TrustReferenceNumber);
				Assert.Equal(_project.NameOfTrust, _template.NameOfTrust);
				Assert.Equal(_project.SponsorReferenceNumber, _template.SponsorReferenceNumber);
				Assert.Equal(_project.SponsorName, _template.SponsorName);
				Assert.Equal(_project.ConversionSupportGrantChangeReason, _template.ConversionSupportGrantChangeReason);

				Assert.Equal(_project.RationaleForProject, _template.RationaleForProject);
				Assert.Equal(_project.RationaleForTrust, _template.RationaleForTrust);

				Assert.Equal(_project.RisksAndIssues, _template.RisksAndIssues);
				Assert.Equal(_project.EqualitiesImpactAssessmentConsidered, _template.EqualitiesImpactAssessmentConsidered);
			}

			[Fact]
			public void ItPopulatesTheFieldsForTheFooter()
			{
				Assert.Equal($"Author: {_project.Author}", _template.Author);
				Assert.Equal($"Cleared by: {_project.ClearedBy}", _template.ClearedBy);
				Assert.Equal($"Version: {DateTime.Today.ToDateString()}", _template.Version);
			}

			[Fact]
			public void FieldsThatDontGoIntoTheWordDoc()
			{
				// fields that could be removed?
				Assert.Equal(_template.ApplicationReceivedDate, _project.ApplicationReceivedDate.ToDateString());
				Assert.Equal(_template.AssignedDate, _project.AssignedDate.ToDateString());
				Assert.Equal(_template.PreviousHeadTeacherBoardLink, _project.PreviousHeadTeacherBoardLink);
			}

			[Fact]
			public void ItBuildstheGeneralInformationSuccessfully()
			{
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
				Assert.Equal(_template.MPName, _project.MemberOfParliamentName);
				Assert.Equal(_template.MPParty, _project.MemberOfParliamentParty);
				Assert.Equal(_template.MPNameAndParty, $"{_template.MPName}, {_template.MPParty}");
			}

			[Fact]
			public void ItBuildsTheBudgetInformationSuccessfully()
			{
				Assert.Equal(_template.RevenueCarryForwardAtEndMarchCurrentYear, $"£{_project.RevenueCarryForwardAtEndMarchCurrentYear?.ToMoneyString()}");
				Assert.Equal(_template.ProjectedRevenueBalanceAtEndMarchNextYear,$"£{ _project.ProjectedRevenueBalanceAtEndMarchNextYear?.ToMoneyString()}");
				Assert.Equal(_template.CapitalCarryForwardAtEndMarchCurrentYear,$"£{_project.CapitalCarryForwardAtEndMarchCurrentYear?.ToMoneyString()}");
				Assert.Equal(_template.CapitalCarryForwardAtEndMarchNextYear, $"£{_project.CapitalCarryForwardAtEndMarchNextYear?.ToMoneyString()}");
				Assert.Equal(_template.SchoolBudgetInformationAdditionalInformation, _project.SchoolBudgetInformationAdditionalInformation);
			}

			[Fact]
			public void ItBuildsTheSchoolPerformanceSuccessfully()
			{
				Assert.Equal(_template.OfstedLastInspection, _schoolPerformance.OfstedLastInspection.ToDateString());
				Assert.Equal(_template.PersonalDevelopment, _schoolPerformance.PersonalDevelopment.DisplayOfstedRating());
				Assert.Equal(_template.BehaviourAndAttitudes, _schoolPerformance.BehaviourAndAttitudes.DisplayOfstedRating());
				Assert.Equal(_template.EarlyYearsProvision, _schoolPerformance.EarlyYearsProvision.DisplayOfstedRating());
				Assert.Equal(_template.EffectivenessOfLeadershipAndManagement, _schoolPerformance.EffectivenessOfLeadershipAndManagement.DisplayOfstedRating());
				Assert.Equal(_template.OverallEffectiveness, _schoolPerformance.OverallEffectiveness.DisplayOfstedRating());
				Assert.Equal(_template.QualityOfEducation, _schoolPerformance.QualityOfEducation.DisplayOfstedRating());
				Assert.Equal(_template.SixthFormProvision, _schoolPerformance.SixthFormProvision.DisplayOfstedRating());
				Assert.Equal(_template.SchoolPerformanceAdditionalInformation, _project.SchoolPerformanceAdditionalInformation);
			}

			[Fact]
			public void ItBuildsSchoolPupilForecastsSuccessfully()
			{
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

		public class NullValuesTests
		{
			private readonly AcademyConversionProject _project;
			private readonly SchoolPerformance _schoolPerformance;
			private readonly GeneralInformation _generalInformation;
			private readonly KeyStagePerformance _keyStagePerformance;

			public NullValuesTests()
			{
				_project = new AcademyConversionProject();
				_schoolPerformance = new SchoolPerformance();
				_generalInformation = new GeneralInformation();
				_keyStagePerformance = new KeyStagePerformance();
			}

			[Fact]
			public void ItSubstitutesNullSponsorInfoWithMeaningfulWording()
			{
				var template = HtbTemplate.Build(_project, _schoolPerformance, _generalInformation, _keyStagePerformance);

				Assert.Equal("Not applicable", template.SponsorName);
				Assert.Equal("Not applicable", template.SponsorReferenceNumber);
			}

			[Fact]
			public void ItSubstitutesNullOfstedDateWithMeaningfulWording()
			{
				var template = HtbTemplate.Build(_project, _schoolPerformance, _generalInformation, _keyStagePerformance);

				Assert.Equal("No data", template.OfstedLastInspection);
			}

			[Fact]
			public void ItDealsWithNullValuesWhenPopulatingTheFieldsForTheFooter()
			{
				var template = HtbTemplate.Build(_project, _schoolPerformance, _generalInformation, _keyStagePerformance);

				Assert.Equal($"Author: ", template.Author);
				Assert.Equal($"Cleared by: ", template.ClearedBy);
			}

			[Fact]
			public void ItDealsWithNullValuesWhenPopulatingTheFieldsForTheGeneralInformation()
			{
				var template = HtbTemplate.Build(_project, _schoolPerformance, _generalInformation, _keyStagePerformance);

				Assert.Null(template.SchoolPhase);
				Assert.Equal("", template.AgeRange);
				Assert.Null(template.SchoolType);
				Assert.Null(template.NumberOnRoll);
				Assert.Equal("", template.PercentageSchoolFull);
				Assert.Null(template.SchoolCapacity);
				Assert.Null(template.PublishedAdmissionNumber);
				Assert.Equal("", template.PercentageFreeSchoolMeals);
				Assert.Null(template.PartOfPfiScheme);
				Assert.Null(template.ViabilityIssues);
				Assert.Null(template.FinancialDeficit);
				Assert.Null(template.IsSchoolLinkedToADiocese);
				Assert.Null(template.DistanceFromSchoolToTrustHeadquarters);
				Assert.Null(template.DistanceFromSchoolToTrustHeadquartersAdditionalInformation);
				Assert.Null(template.ParliamentaryConstituency);
				Assert.Null(template.MPName);
				Assert.Null(template.MPParty);
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