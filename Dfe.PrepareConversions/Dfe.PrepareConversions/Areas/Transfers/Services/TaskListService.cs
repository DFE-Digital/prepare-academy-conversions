﻿using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Data.Models.Projects;
using System;
using System.Linq;
using Index = Dfe.PrepareTransfers.Web.Pages.Projects.Index;

namespace Dfe.PrepareTransfers.Web.Services
{
    public class TaskListService : ITaskListService
    {
        private readonly IProjects _projectRepository;

        public TaskListService(IProjects projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public void BuildTaskListStatuses(Index indexPage)
        {
            var project = _projectRepository.GetByUrn(indexPage.Urn).Result;
            indexPage.ProjectReference = project.Result.Reference;
            indexPage.IncomingTrustReferenceNumber = project.Result.IncomingTrustReferenceNumber;
            indexPage.IncomingTrustName = !string.IsNullOrEmpty(project.Result.IncomingTrustName) ? project.Result.IncomingTrustName.ToTitleCase() : project.Result.OutgoingTrustName.ToTitleCase();
            indexPage.Academies = project.Result.TransferringAcademies
                .Select(a => new Tuple<string, string>(a.OutgoingAcademyUkprn, a.OutgoingAcademyName)).ToList();
            indexPage.AcademyAndTrustInformationStatus = GetAcademyAndTrustInformationStatus(project.Result);
            indexPage.FeatureTransferStatus = GetFeatureTransferStatus(project.Result);
            indexPage.TransferDatesStatus = GetTransferDatesStatus(project.Result);
            indexPage.BenefitsAndOtherFactorsStatus = GetBenefitsAndOtherFactorsStatus(project.Result);
            indexPage.LegalRequirementsStatus = GetLegalRequirementsStatus(project.Result);
            indexPage.RationaleStatus = GetRationaleStatus(project.Result);
            indexPage.PublicSectorEqualityDutyStatus = GetPublicSectorEqualityDutyStatus(project.Result);
            indexPage.ProjectStatus = project.Result.Status;
            indexPage.AssignedUser = project.Result.AssignedUser;
            indexPage.IsFormAMAT = project.Result.IsFormAMat.HasValue && project.Result.IsFormAMat.Value;
            indexPage.IsReadOnly = project.Result.IsReadOnly;
            indexPage.ProjectSentToCompleteDate = project.Result.ProjectSentToCompleteDate;
            indexPage.HeadTeacherBoardDate = project.Result.Dates.Htb;

            // Public Sector Equality Duty
            indexPage.PublicEqualityDutyImpact = project.Result.PublicEqualityDutyImpact;
            indexPage.PublicEqualityDutyReduceImpactReason = project.Result.PublicEqualityDutyReduceImpactReason;
            indexPage.PublicEqualityDutySectionComplete = project.Result.PublicEqualityDutySectionComplete;
      }

      private static ProjectStatuses GetAcademyAndTrustInformationStatus(Project project)
        {
            TransferAcademyAndTrustInformation academyAndTrustInformation = project.AcademyAndTrustInformation;

            if (string.IsNullOrEmpty(academyAndTrustInformation.Author) &&
                academyAndTrustInformation.Recommendation ==
                TransferAcademyAndTrustInformation.RecommendationResult.Empty)
            {
                return ProjectStatuses.NotStarted;
            }

            return string.IsNullOrEmpty(academyAndTrustInformation.Author) ||
                   academyAndTrustInformation.Recommendation ==
                   TransferAcademyAndTrustInformation.RecommendationResult.Empty
               ? ProjectStatuses.InProgress
               : ProjectStatuses.Completed;
        }

        private static ProjectStatuses GetFeatureTransferStatus(Project project)
        {
            if (project.Features.ReasonForTheTransfer == TransferFeatures.ReasonForTheTransferTypes.Empty &&
                                                         project.Features.TypeOfTransfer == TransferFeatures.TransferTypes.Empty)
            {
                return ProjectStatuses.NotStarted;
            }

            return project.Features.IsCompleted == true ? ProjectStatuses.Completed : ProjectStatuses.InProgress;
        }

         private static ProjectStatuses GetPublicSectorEqualityDutyStatus(Project project)
         {
            if (project.PublicEqualityDutySectionComplete != null && project.PublicEqualityDutySectionComplete == true)
            {
               return ProjectStatuses.Completed;
            }

            if (string.IsNullOrWhiteSpace(project.PublicEqualityDutyImpact))
            {
               return ProjectStatuses.NotStarted;
            }

            return ProjectStatuses.InProgress;
         }

        private static ProjectStatuses GetTransferDatesStatus(Project project)
        {
            if ((string.IsNullOrEmpty(project.Dates.Target) && (project.Dates.HasTargetDateForTransfer ?? true)) &&
                (string.IsNullOrEmpty(project.Dates.Htb) && (project.Dates.HasHtbDate ?? true)))
            {
                return ProjectStatuses.NotStarted;
            }

            if (project.Dates.IsCompleted is true)
            {
                return ProjectStatuses.Completed;
            }

            return ProjectStatuses.InProgress;
        }

        private static ProjectStatuses GetBenefitsAndOtherFactorsStatus(Project project)
        {
            if ((project.Benefits.IntendedBenefits == null || !project.Benefits.IntendedBenefits.Any()) &&
                (project.Benefits.OtherFactors == null || !project.Benefits.OtherFactors.Any()))
            {
                return ProjectStatuses.NotStarted;
            }

            return project.Benefits.IsCompleted == true ? ProjectStatuses.Completed : ProjectStatuses.InProgress;
        }
        private static ProjectStatuses GetLegalRequirementsStatus(Project project)
        {
            if (project.LegalRequirements.DiocesanConsent == null &&
                    project.LegalRequirements.IncomingTrustAgreement == null &&
                    project.LegalRequirements.OutgoingTrustConsent == null &&
                    project.LegalRequirements.IsCompleted == null)
            {
                return ProjectStatuses.NotStarted;
            }
            return project.LegalRequirements.IsCompleted == true ? ProjectStatuses.Completed : ProjectStatuses.InProgress;
        }

        private static ProjectStatuses GetRationaleStatus(Project project)
        {
            if (string.IsNullOrEmpty(project.Rationale.Project) &&
                string.IsNullOrEmpty(project.Rationale.Trust))
            {
                return ProjectStatuses.NotStarted;
            }

            return project.Rationale.IsCompleted == true ? ProjectStatuses.Completed : ProjectStatuses.InProgress;
        }
    }
}