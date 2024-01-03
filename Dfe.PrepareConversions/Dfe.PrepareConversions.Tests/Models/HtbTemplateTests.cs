using AutoFixture;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Models;
using System;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Models;

public class HtbTemplateTests
{
   public class WholeTemplateTests
   {
      private readonly SchoolOverview _schoolOverview;
      private readonly AcademyConversionProject _project;
      private readonly SchoolPerformance _schoolPerformance;
      private readonly HtbTemplate _template;

      public WholeTemplateTests()
      {
         Fixture fixture = new();
         _project = fixture.Create<AcademyConversionProject>();
         _schoolPerformance = fixture.Create<SchoolPerformance>();
         _schoolOverview = fixture.Create<SchoolOverview>();
         KeyStagePerformance keyStagePerformance = new() { KeyStage2 = fixture.CreateMany<KeyStage2PerformanceResponse>(3) };

         _template = HtbTemplate.Build(_project, _schoolPerformance, _schoolOverview, keyStagePerformance);
      }

      [Fact]
      public void ItBuildsTheSchoolAndTrustInformationAndProjectDatesAndSchoolPerformanceSuccessfully()
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
         Assert.Equal(_schoolPerformance, _template.SchoolPerformance);

         Assert.Equal(_project.GoverningBodyResolution.SplitPascalCase(), _template.GoverningBodyResolution);
         Assert.Equal(_project.Consultation.SplitPascalCase(), _template.Consultation);
         Assert.Equal(_project.DiocesanConsent.SplitPascalCase(), _template.DiocesanConsent);
         Assert.Equal(_project.FoundationConsent.SplitPascalCase(), _template.FoundationConsent);
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
      public void ItBuildstheSchoolOverviewSuccessfully()
      {
         Assert.Equal(_template.SchoolPhase, _schoolOverview.SchoolPhase);
         Assert.Equal(_template.AgeRange, $"{_schoolOverview.AgeRangeLower} to {_schoolOverview.AgeRangeUpper}");
         Assert.Equal(_template.SchoolType, _schoolOverview.SchoolType);
         Assert.Equal(_template.NumberOnRoll, _schoolOverview.NumberOnRoll?.ToString());
         Assert.Equal(_template.PercentageSchoolFull, _schoolOverview.NumberOnRoll.AsPercentageOf(_schoolOverview.SchoolCapacity));
         Assert.Equal(_template.SchoolCapacity, _schoolOverview.SchoolCapacity?.ToString());
         Assert.Equal(_template.PublishedAdmissionNumber, _project.PublishedAdmissionNumber);
         Assert.Equal(_template.PercentageFreeSchoolMeals, $"{_schoolOverview.PercentageFreeSchoolMeals}%");
         Assert.Equal(_template.PartOfPfiScheme, _project.PartOfPfiScheme);
         Assert.Equal(_template.ViabilityIssues, _project.ViabilityIssues);
         Assert.Equal(_template.NumberOfPlacesFundedFor, _project.NumberOfPlacesFundedFor.ToString());
         Assert.Equal(_template.NumberOfResidentialPlaces, _project.NumberOfResidentialPlaces.ToString());
         Assert.Equal(_template.NumberOfFundedResidentialPlaces, _project.NumberOfFundedResidentialPlaces.ToString());
         Assert.Equal(_template.FinancialDeficit, _project.FinancialDeficit);
         Assert.Equal(_template.IsSchoolLinkedToADiocese, _schoolOverview.IsSchoolLinkedToADiocese);
         Assert.Equal(_template.DistanceFromSchoolToTrustHeadquarters, $"{_project.DistanceFromSchoolToTrustHeadquarters.ToSafeString()} miles");
         Assert.Equal(_template.DistanceFromSchoolToTrustHeadquartersAdditionalInformation, _project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation);
         Assert.Equal(_template.ParliamentaryConstituency, _schoolOverview.ParliamentaryConstituency);
         Assert.Equal(_template.MemberOfParliamentNameAndParty, _project.MemberOfParliamentNameAndParty);
      }

      [Fact]
      public void ItBuildsTheBudgetInformationSuccessfully()
      {
         Assert.Equal(_template.EndOfCurrentFinancialYear, $"{_project.EndOfCurrentFinancialYear.ToDateString()}");
         Assert.Equal(_template.RevenueCarryForwardAtEndMarchCurrentYear, $"£{_project.RevenueCarryForwardAtEndMarchCurrentYear?.ToMoneyString()}");
         Assert.Equal(_template.CapitalCarryForwardAtEndMarchCurrentYear, $"£{_project.CapitalCarryForwardAtEndMarchCurrentYear?.ToMoneyString()}");
         Assert.Equal(_template.EndOfNextFinancialYear, $"{_project.EndOfNextFinancialYear.ToDateString()}");
         Assert.Equal(_template.ProjectedRevenueBalanceAtEndMarchNextYear, $"£{_project.ProjectedRevenueBalanceAtEndMarchNextYear?.ToMoneyString()}");
         Assert.Equal(_template.CapitalCarryForwardAtEndMarchNextYear, $"£{_project.CapitalCarryForwardAtEndMarchNextYear?.ToMoneyString()}");
         Assert.Equal(_template.SchoolBudgetInformationAdditionalInformation, _project.SchoolBudgetInformationAdditionalInformation);
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
      private readonly SchoolOverview _schoolOverview;
      private readonly KeyStagePerformance _keyStagePerformance;
      private readonly AcademyConversionProject _project;
      private readonly SchoolPerformance _schoolPerformance;

      public NullValuesTests()
      {
         _project = new AcademyConversionProject();
         _schoolPerformance = new SchoolPerformance();
         _schoolOverview = new SchoolOverview();
         _keyStagePerformance = new KeyStagePerformance();
      }

      [Fact]
      public void ItSubstitutesNullSponsorInfoWithMeaningfulWording()
      {
         HtbTemplate template = HtbTemplate.Build(_project, _schoolPerformance, _schoolOverview, _keyStagePerformance);

         Assert.Equal("Not applicable", template.SponsorName);
         Assert.Equal("Not applicable", template.SponsorReferenceNumber);
      }

      [Fact]
      public void ItDealsWithNullValuesWhenPopulatingTheFieldsForTheFooter()
      {
         HtbTemplate template = HtbTemplate.Build(_project, _schoolPerformance, _schoolOverview, _keyStagePerformance);

         Assert.Equal("Author: ", template.Author);
         Assert.Equal("Cleared by: ", template.ClearedBy);
      }

      [Fact]
      public void ItDealsWithNullValuesWhenPopulatingTheFieldsForTheSchoolOverview()
      {
         HtbTemplate template = HtbTemplate.Build(_project, _schoolPerformance, _schoolOverview, _keyStagePerformance);

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
         Assert.Null(template.NumberOfPlacesFundedFor);
         Assert.Null(template.NumberOfResidentialPlaces);
         Assert.Null(template.NumberOfFundedResidentialPlaces);
         Assert.Null(template.FinancialDeficit);
         Assert.Null(template.IsSchoolLinkedToADiocese);
         Assert.Null(template.DistanceFromSchoolToTrustHeadquarters);
         Assert.Null(template.DistanceFromSchoolToTrustHeadquartersAdditionalInformation);
         Assert.Null(template.ParliamentaryConstituency);
         Assert.Null(template.MemberOfParliamentNameAndParty);
      }


      public class KeyStagePerformanceTests
      {
         private readonly Fixture _fixture;
         private readonly SchoolOverview _schoolOverview;
         private readonly AcademyConversionProject _project;
         private readonly SchoolPerformance _schoolPerformance;

         public KeyStagePerformanceTests()
         {
            _fixture = new Fixture();
            _project = _fixture.Create<AcademyConversionProject>();
            _schoolPerformance = _fixture.Create<SchoolPerformance>();
            _schoolOverview = _fixture.Create<SchoolOverview>();
         }

         [Fact]
         public void GivenNoKeyStage2DataToDisplay_DoesNotPopulateKeyStage2Data()
         {
            KeyStagePerformance keyStagePerformance = new();
            HtbTemplate template = HtbTemplate.Build(_project, _schoolPerformance, _schoolOverview, keyStagePerformance);

            Assert.Null(template.KeyStage2);
         }

         [Fact]
         public void GivenNoKeyStage4DataToDisplay_DoesNotPopulateKeyStage4Data()
         {
            KeyStagePerformance keyStagePerformance = new();
            HtbTemplate template = HtbTemplate.Build(_project, _schoolPerformance, _schoolOverview, keyStagePerformance);

            Assert.Null(template.KeyStage4);
         }

         [Fact]
         public void GivenNoKeyStage5DataToDisplay_DoesNotPopulateKeyStage5Data()
         {
            KeyStagePerformance keyStagePerformance = new();
            HtbTemplate template = HtbTemplate.Build(_project, _schoolPerformance, _schoolOverview, keyStagePerformance);

            Assert.Null(template.KeyStage5);
         }

         [Fact]
         public void GivenKeyStageData_PopulatesKeyStageData()
         {
            KeyStagePerformance keyStagePerformance = new()
            {
               KeyStage2 = _fixture.CreateMany<KeyStage2PerformanceResponse>(4),
               KeyStage4 = _fixture.CreateMany<KeyStage4PerformanceResponse>(3),
               KeyStage5 = _fixture.CreateMany<KeyStage5PerformanceResponse>(2)
            };

            HtbTemplate template = HtbTemplate.Build(_project, _schoolPerformance, _schoolOverview, keyStagePerformance);

            Assert.NotNull(template.KeyStage2);
            Assert.Equal(4, template.KeyStage2.Count());
            Assert.NotNull(template.KeyStage4);
            Assert.NotNull(template.KeyStage5);
            Assert.Equal(2, template.KeyStage5.Count());
         }
      }
   }
}
