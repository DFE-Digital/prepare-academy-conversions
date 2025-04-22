using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Models.Benefits;
using Dfe.PrepareTransfers.Web.Models.Forms;
using Dfe.PrepareTransfers.Web.Models.LegalRequirements;
using Dfe.PrepareTransfers.Web.Pages.Projects;
using Dfe.PrepareTransfers.Web.Pages.Projects.BenefitsAndRisks;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Index = Dfe.PrepareTransfers.Web.Pages.Projects.Features.Index;
using LatestOfstedJudgementIndex = Dfe.PrepareTransfers.Web.Pages.Projects.LatestOfstedJudgement.Index;

namespace Dfe.PrepareTransfers.Web.Pages.TaskList.HtbDocument
{
    public class Preview : CommonPageModel
    {
        private readonly IGetInformationForProject _getInformationForProject;
        private readonly IProjects _projects;
        private readonly ErrorService _errorService;
        public Index FeaturesSummaryViewModel { get; set; }
        public BenefitsSummaryViewModel BenefitsSummaryViewModel { get; set; }
        public LegalRequirementsViewModel LegalRequirementsViewModel { get; set; }
        public Projects.TransferDates.Index TransferDatesSummaryViewModel { get; set; }
        public Projects.AcademyAndTrustInformation.Index AcademyAndTrustInformationSummaryViewModel { get; set; }
        public Projects.Rationale.Index RationaleSummaryViewModel { get; set; }
        public List<PreviewPageAcademyModel> Academies { get; private set; }

         public Preview(IGetInformationForProject getInformationForProject, IProjects projects, ErrorService errorService)
         {
            _getInformationForProject = getInformationForProject;
            _projects = projects;
            _errorService = errorService;
         }

         private void MapModel(Project project, List<Academy> outgoingAcademies)
         {
            ProjectReference = project.Reference;
            PublicEqualityDutyImpact = project.PublicEqualityDutyImpact;
            PublicEqualityDutyReduceImpactReason = project.PublicEqualityDutyReduceImpactReason;
            PublicEqualityDutySectionComplete = project.PublicEqualityDutySectionComplete;

            HeadTeacherBoardDate = project.Dates.Htb;

            FeaturesSummaryViewModel = new Index(null)
            {
               Urn = project.Urn,
               TypeOfTransfer = project.Features.TypeOfTransfer,
               OtherTypeOfTransfer = project.Features.OtherTypeOfTransfer,
               OutgoingAcademyUrn = project.OutgoingAcademyUrn,
               ReasonForTheTransfer = project.Features.ReasonForTheTransfer,
               SpecificReasonForTheTransfer = project.Features.SpecificReasonsForTheTransfer,
               ReturnToPreview = true
            };

            BenefitsSummaryViewModel = new BenefitsSummaryViewModel(
                project.Benefits.IntendedBenefits.ToList(),
                project.Benefits.OtherIntendedBenefit,
                OtherFactors.BuildOtherFactorsItemViewModel(project.Benefits.OtherFactors)
                    .Where(o => o.Checked)
                    .ToList(),
                project.Urn,
                project.OutgoingAcademyUrn,
                project.Benefits.AnyRisks,
                project.Benefits.EqualitiesImpactAssessmentConsidered
            )
            {
               ReturnToPreview = true
            };

            LegalRequirementsViewModel = new LegalRequirementsViewModel(
                project.LegalRequirements.IncomingTrustAgreement,
                project.LegalRequirements.DiocesanConsent,
                project.LegalRequirements.OutgoingTrustConsent,
                project.Urn,
                project.IsReadOnly,
                project.ProjectSentToCompleteDate
            )
            {
               ReturnToPreview = true
            };

            TransferDatesSummaryViewModel = new Projects.TransferDates.Index(_projects)
            {
               Urn = project.Urn,
               ReturnToPreview = true,
               OutgoingAcademyUrn = project.OutgoingAcademyUrn,
               AdvisoryBoardDate = project.Dates.Htb,
               HasAdvisoryBoardDate = project.Dates.HasHtbDate,
               TargetDate = project.Dates.Target,
               HasTargetDate = project.Dates.HasTargetDateForTransfer
            };

            AcademyAndTrustInformationSummaryViewModel =
                new Projects.AcademyAndTrustInformation.Index(_getInformationForProject, _projects)
                {
                   Recommendation = project.AcademyAndTrustInformation.Recommendation,
                   Author = project.AcademyAndTrustInformation.Author,
                   AdvisoryBoardDate = project.Dates?.Htb,
                   IncomingTrustName = project.IncomingTrustName,
                   TargetDate = project.Dates?.Target,
                   OutgoingAcademyUrn = project.OutgoingAcademyUrn,
                   Urn = project.Urn,
                   ReturnToPreview = true,
                   IsReadOnly = project.IsReadOnly
                };

            RationaleSummaryViewModel = new Pages.Projects.Rationale.Index(_projects)
            {
               ProjectRationale = project.Rationale.Project,
               TrustRationale = project.Rationale.Trust,
               OutgoingAcademyUrn = project.OutgoingAcademyUrn,
               Urn = project.Urn,
               ReturnToPreview = true
            };

            var previewPageAcademyModels = new List<PreviewPageAcademyModel>();
            foreach (var previewPageAcademyModel in outgoingAcademies.Select(academy =>
                 new PreviewPageAcademyModel()
                 {
                    Academy = academy,
                    EducationPerformance = academy.EducationPerformance,
                    GeneralInformationViewModel =
                         new Pages.Projects.GeneralInformation.Index(_getInformationForProject)
                         {
                            ReturnToPreview = true,
                            Urn = project.Urn,
                            AcademyUkprn = academy.Ukprn,
                            SchoolPhase = academy.GeneralInformation.SchoolPhase,
                            AgeRange = academy.GeneralInformation.AgeRange,
                            Capacity = academy.GeneralInformation.Capacity,
                            NumberOnRoll =
                                 $"{academy.GeneralInformation.NumberOnRoll} ({academy.GeneralInformation.PercentageFull})",
                            FreeSchoolMeals = academy.GeneralInformation.PercentageFsm,
                            PublishedAdmissionNumber = academy.PublishedAdmissionNumber,
                            PrivateFinanceInitiative = $"{academy.PFIScheme} {(string.IsNullOrWhiteSpace(academy.PFISchemeDetails) ? string.Empty : $" - {academy.PFISchemeDetails}")}",
                            ViabilityIssues = academy.ViabilityIssues,
                            FinancialDeficit = academy.FinancialDeficit,
                            SchoolType = academy.GeneralInformation.SchoolType,
                            DiocesePercent = academy.GeneralInformation.DiocesesPercent,
                            DistanceFromAcademyToTrustHq = $"{academy.DistanceFromAcademyToTrustHq} Miles {(string.IsNullOrWhiteSpace(academy.DistanceFromAcademyToTrustHqDetails) ? string.Empty : $" - {academy.DistanceFromAcademyToTrustHqDetails}")}",
                            MP = academy.MPNameAndParty
                         },
                    PupilNumbersViewModel = new PupilNumbers(_getInformationForProject, _projects)
                    {
                       GirlsOnRoll = academy.PupilNumbers.GirlsOnRoll,
                       BoysOnRoll = academy.PupilNumbers.BoysOnRoll,
                       WithStatementOfSEN = academy.PupilNumbers.WithStatementOfSen,
                       WithEAL = academy.PupilNumbers.WhoseFirstLanguageIsNotEnglish,
                       FreeSchoolMealsLast6Years = academy.PupilNumbers
                             .PercentageEligibleForFreeSchoolMealsDuringLast6Years,
                       OutgoingAcademyUrn = academy.Urn,
                       AcademyUkprn = academy.Ukprn,
                       IsPreview = true,
                       Urn = project.Urn,
                       AdditionalInformationViewModel = new AdditionalInformationViewModel
                       {
                          AdditionalInformation = academy.PupilNumbers.AdditionalInformation,
                          HintText =
                                 "If you add comments, they'll be included in the pupil numbers section of your project template.",
                          Urn = project.Urn,
                          ReturnToPreview = true
                       }
                    },
                    LatestOfstedJudgementViewModel =
                         new LatestOfstedJudgementIndex(_getInformationForProject, _projects)
                         {
                            Urn = project.Urn,
                            OutgoingAcademyUrn = project.OutgoingAcademyUrn,
                            AcademyUkprn = academy.Ukprn,
                            LatestOfstedJudgement = academy.LatestOfstedJudgement,
                            ReturnToPreview = true,
                            AdditionalInformationViewModel = new AdditionalInformationViewModel
                            {
                               AdditionalInformation = academy.LatestOfstedJudgement.AdditionalInformation,
                               HintText =
                                     "If you add comments, they'll be included in the latest Ofsted judgement section of your project template.",
                               Urn = project.Urn,
                               ReturnToPreview = true
                            },
                            IsPreview = true
                         }
                 }))
            {
               previewPageAcademyModels.Add(previewPageAcademyModel);
               Academies = previewPageAcademyModels;
            }
         }

         private void Validate()
         {
            if (string.IsNullOrWhiteSpace(HeadTeacherBoardDate))
            {
               _errorService.AddError($"/transfers/project/{Urn}/transfer-dates/advisory-board-date?returnToPreview=true",
                  "Set an Advisory Board date before you generate your project template");
            }

            var isPsedValid = PrepareConversions.Models.PreviewPublicSectorEqualityDutyModel.IsValid(PublicEqualityDutyImpact, PublicEqualityDutyReduceImpactReason, PublicEqualityDutySectionComplete ?? false);
            if (!isPsedValid)
            {
               _errorService.AddError($"/transfers/project/{Urn}/public-sector-equality-duty?returnToPreview=true",
                  "Consider the Public Sector Equality Duty");
            }
         }

         public async Task<IActionResult> OnGet()
         {
            var response = await _getInformationForProject.Execute(Urn);

            MapModel(response.Project, response.OutgoingAcademies);

            return Page();
         }

         public async Task<IActionResult> OnPostAsync(string urn)
         {
            var response = await _getInformationForProject.Execute(Urn);

            MapModel(response.Project, response.OutgoingAcademies);

            Validate();

            if (_errorService.HasErrors())
            {
               return Page();
            }

            return RedirectToPage("/TaskList/HtbDocument/Download", new { Urn });
         }
   }
}