using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Data.Models.Projects;
using Dfe.PrepareTransfers.Data.TRAMS.ExtensionMethods;
using Dfe.PrepareTransfers.Web.Dfe.PrepareTransfers.Helpers;
using Dfe.PrepareTransfers.Web.Models.ProjectTemplate;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Dfe.PrepareTransfers.Web.Services.Responses;
using Dfe.PrepareTransfers.Helpers;

namespace Dfe.PrepareTransfers.Web.Services
{
    public class GetProjectTemplateModel(IGetInformationForProject getInformationForProject) : IGetProjectTemplateModel
    {
      private static readonly string EmptyFieldMessage = "No data";

      public async Task<GetProjectTemplateModelResponse> Execute(string projectUrn)
        {
            var informationForProjectResult = await getInformationForProject.Execute(projectUrn);
            var project = informationForProjectResult.Project;
            var academies = informationForProjectResult.OutgoingAcademies;
            var projectTemplateModel = new ProjectTemplateModel
            {
                Recommendation = project.AcademyAndTrustInformation.Recommendation != TransferAcademyAndTrustInformation.RecommendationResult.Empty ?
                    EnumHelpers<TransferAcademyAndTrustInformation.RecommendationResult>.GetDisplayValue(
                    project.AcademyAndTrustInformation.Recommendation) : EmptyFieldMessage,
                Author = project.AcademyAndTrustInformation.Author ?? EmptyFieldMessage,
                ProjectName = project.IncomingTrustName,
                ProjectReference = project.Reference,
                IncomingTrustName = project.IncomingTrustName,
                IncomingTrustUkprn = project.IncomingTrustUkprn,
                RationaleForProject = project.Rationale.Project ?? EmptyFieldMessage,
                RationaleForTrust = project.Rationale.Trust ?? EmptyFieldMessage,
                ClearedBy = "",
                Version = DateTime.Now.ToString("yyyyMMdd", CultureInfo.CurrentUICulture),
                DateOfHtb = DatesHelper.FormatDateString(project.Dates.Htb, project.Dates.HasHtbDate) ?? EmptyFieldMessage,
                DateOfProposedTransfer =
                    DatesHelper.FormatDateString(project.Dates.Target, project.Dates.HasTargetDateForTransfer)?? EmptyFieldMessage,

                ReasonForTheTransfer = project.Features
                        .ReasonForTheTransfer != TransferFeatures.ReasonForTheTransferTypes.Empty ?
                    EnumHelpers<TransferFeatures.ReasonForTheTransferTypes>.GetDisplayValue(project.Features
                        .ReasonForTheTransfer) : EmptyFieldMessage,
                SpecificReasonsForTheTransfer = project.Features.SpecificReasonsForTheTransfer.Any() 
                ? String.Join(", ", project.Features.SpecificReasonsForTheTransfer.Select(x => EnumHelpers<TransferFeatures.SpecificReasonForTheTransferTypes>.GetDisplayValue(x))) 
                : EmptyFieldMessage,
                TypeOfTransfer = TransferTypeSelector(project),
                TransferBenefits = GetTransferBenefits(project.Benefits),
                IncomingTrustAgreement = project.LegalRequirements.IncomingTrustAgreement != null ?project.LegalRequirements.IncomingTrustAgreement.ToDescription() : EmptyFieldMessage,
                OutgoingTrustConsent = project.LegalRequirements.OutgoingTrustConsent != null ? project.LegalRequirements.OutgoingTrustConsent.ToDescription() : EmptyFieldMessage,
                DiocesanConsent = project.LegalRequirements.DiocesanConsent != null  ? project.LegalRequirements.DiocesanConsent.ToDescription() : EmptyFieldMessage,
                EqualitiesImpactAssessmentConsidered = project.Benefits.EqualitiesImpactAssessmentConsidered == true ? "Yes" : "Not considered",
                AnyRisks = project.Benefits.AnyRisks.ToDisplay(),
                OtherFactors = GetOtherFactors(project.Benefits),
                Academies = GetAcademyData(academies),
                ListOfTransferBenefits = project.Benefits.IntendedBenefits,
                OtherIntendedBenefit = project.Benefits.OtherIntendedBenefit,
                ListOfOtherFactors = project.Benefits.OtherFactors,
                AnyIdentifiedRisks = project.Benefits.AnyRisks,
               // Public sector equality duty
               PublicEqualityDutyImpact = project.PublicEqualityDutyImpact,
               PublicEqualityDutyReduceImpactReason = project.PublicEqualityDutyReduceImpactReason
            };

            return new GetProjectTemplateModelResponse
            {
                ProjectTemplateModel = projectTemplateModel
            };
        }

        public string TransferTypeSelector(Project project)
        {
            
            if (project.Features.TypeOfTransfer == TransferFeatures.TransferTypes.Other)
            {
                return $"Other: {project.Features.OtherTypeOfTransfer}";
            }

            else if (project.Features.TypeOfTransfer == TransferFeatures.TransferTypes.Empty)
            {
               return EmptyFieldMessage;
            }

            else
            {
                return EnumHelpers<TransferFeatures.TransferTypes>.GetDisplayValue(project.Features.TypeOfTransfer);
            }         
        }

        private static List<ProjectTemplateAcademyModel> GetAcademyData(List<Academy> academies)
        {
            var academyModels = new List<ProjectTemplateAcademyModel>();
            foreach (var academy in academies)
            {
                var academyModel = new ProjectTemplateAcademyModel();

                academyModel.AcademyTypeAndRoute = academy.EstablishmentType;
                academyModel.SchoolName = academy.Name;
                academyModel.SchoolUrn = academy.Urn;
                academyModel.AcademyTypeAndRoute = academy.EstablishmentType;
                academyModel.SchoolType = academy.GeneralInformation.SchoolType;
                academyModel.SchoolPhase = academy.GeneralInformation.SchoolPhase;
                academyModel.AgeRange = academy.GeneralInformation.AgeRange;
                academyModel.SchoolCapacity = academy.GeneralInformation.Capacity;
                academyModel.PublishedAdmissionNumber = academy.PublishedAdmissionNumber;
                academyModel.NumberOnRoll =
                    $"{academy.GeneralInformation.NumberOnRoll} ({academy.GeneralInformation.PercentageFull})";
                academyModel.PercentageFreeSchoolMeals = academy.GeneralInformation.PercentageFsm;
                academyModel.OverallEffectiveness = academy.LatestOfstedJudgement.OverallEffectiveness;
                academyModel.ViabilityIssues = academy.ViabilityIssues;
                academyModel.FinancialDeficit = academy.FinancialDeficit;
                academyModel.Pfi = $"{academy.PFIScheme} {(string.IsNullOrWhiteSpace(academy.PFISchemeDetails) ? string.Empty : $" - {academy.PFISchemeDetails}")}";
                academyModel.PercentageGoodOrOutstandingInDiocesanTrust = academy.GeneralInformation.DiocesesPercent;
                academyModel.DistanceFromTheAcademyToTheTrustHeadquarters =
                    $"{academy.DistanceFromAcademyToTrustHq} Miles {(string.IsNullOrWhiteSpace(academy.DistanceFromAcademyToTrustHqDetails) ? string.Empty : $" - {academy.DistanceFromAcademyToTrustHqDetails}")}";
                academyModel.MpAndParty = academy.MPNameAndParty;
                academyModel.GirlsOnRoll = academy.PupilNumbers.GirlsOnRoll;
                academyModel.BoysOnRoll = academy.PupilNumbers.BoysOnRoll;
                academyModel.PupilsWithSen = academy.PupilNumbers.WithStatementOfSen;
                academyModel.PupilsWithFirstLanguageNotEnglish = academy.PupilNumbers.WhoseFirstLanguageIsNotEnglish;
                academyModel.PupilsFsm6Years =
                    academy.PupilNumbers.PercentageEligibleForFreeSchoolMealsDuringLast6Years;
                academyModel.PupilNumbersAdditionalInformation = academy.PupilNumbers.AdditionalInformation;
                academyModel.OfstedReport = academy.LatestOfstedJudgement.OfstedReport;
                academyModel.OfstedAdditionalInformation = academy.LatestOfstedJudgement.AdditionalInformation;
                academyModel.KeyStage2Performance = academy.EducationPerformance.KeyStage2Performance;
                academyModel.KeyStage4Performance =
                    PerformanceDataHelpers.GetFormattedKeyStage4Results(academy.EducationPerformance
                        .KeyStage4Performance);
                academyModel.KeyStage5Performance = academy.EducationPerformance.KeyStage5Performance;
                academyModel.KeyStage2AdditionalInformation =
                    academy.EducationPerformance.KeyStage2AdditionalInformation;
                academyModel.KeyStage4AdditionalInformation =
                    academy.EducationPerformance.KeyStage4AdditionalInformation;
                academyModel.KeyStage5AdditionalInformation =
                    academy.EducationPerformance.KeyStage5AdditionalInformation;
                academyModel.LocalAuthorityName = academy.LocalAuthorityName;
                academyModel.QualityOfEducation = academy.LatestOfstedJudgement.QualityOfEducation;
                academyModel.BehaviourAndAttitudes = academy.LatestOfstedJudgement.BehaviourAndAttitudes;
                academyModel.PersonalDevelopment = academy.LatestOfstedJudgement.PersonalDevelopment;
                academyModel.EffectivenessOfLeadershipAndManagement = academy.LatestOfstedJudgement.EffectivenessOfLeadershipAndManagement;
                academyModel.EarlyYearsProvision = academy.LatestOfstedJudgement.EarlyYearsProvision;
                academyModel.EarlyYearsProvisionApplicable = academy.LatestOfstedJudgement.EarlyYearsProvisionApplicable;
                academyModel.SixthFormProvision = academy.LatestOfstedJudgement.SixthFormProvision;
                academyModel.SixthFormProvisionApplicable = academy.LatestOfstedJudgement.SixthFormProvisionApplicable;
                academyModel.DateOfLatestSection8Inspection = DatesHelper.DateStringToGovUkDate(academy.LatestOfstedJudgement.DateOfLatestSection8Inspection);
                academyModel.LatestInspectionIsSection8 = academy.LatestOfstedJudgement.LatestInspectionIsSection8;
                academyModel.InspectionEndDate = DatesHelper.DateStringToGovUkDate(academy.LatestOfstedJudgement.InspectionEndDate);

                academyModels.Add(academyModel);
            }

            return academyModels;
        }

        private static List<Tuple<string,string>> GetOtherFactors(TransferBenefits transferBenefits)
        {
           return transferBenefits.OtherFactors
                .OrderBy(o => (int) o.Key)
                .Select(otherFactor => new Tuple<string,string>(
                    EnumHelpers<TransferBenefits.OtherFactor>.GetDisplayValue(otherFactor.Key),
                    otherFactor.Value)).ToList();
        }

        private static string GetTransferBenefits(TransferBenefits transferBenefits)
        {
            var benefitSummary = transferBenefits.IntendedBenefits
                .FindAll(EnumHelpers<TransferBenefits.IntendedBenefit>.HasDisplayValue)
                .Select(EnumHelpers<TransferBenefits.IntendedBenefit>.GetDisplayValue)
                .ToList();

            if (transferBenefits.IntendedBenefits.Contains(TransferBenefits.IntendedBenefit.Other))
            {
                benefitSummary.Add($"Other: {transferBenefits.OtherIntendedBenefit}");
            }

            var sb = new StringBuilder();
            foreach (var benefit in benefitSummary)
            {
                sb.Append($"{benefit}\n");
            }

            return sb.ToString();
        }
    }
}