using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Dfe.PrepareTransfers.Web.Services.Responses;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Data.Models.KeyStagePerformance;
using Dfe.PrepareTransfers.Data.Models.Projects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Web.Services
{
    public class GetInformationForProject : IGetInformationForProject
    {
        private readonly IProjects _projectsRepository;
        private readonly IAcademies _academiesRepository;
        private readonly IEducationPerformance _educationPerformanceRepository;

        public GetInformationForProject(IAcademies academiesRepository,
            IProjects projectsRepository, IEducationPerformance educationPerformanceRepository)
        {
            _academiesRepository = academiesRepository;
            _projectsRepository = projectsRepository;
            _educationPerformanceRepository = educationPerformanceRepository;
        }

        public async Task<GetInformationForProjectResponse> Execute(string projectUrn)
        {
            var projectResult = await _projectsRepository.GetByUrn(projectUrn);

            var outgoingAcademies = new List<Academy>();

            foreach (var transferringAcademy in projectResult.Result.TransferringAcademies)
            {
                var academyResult =
                    await _academiesRepository.GetAcademyByUkprn(transferringAcademy.OutgoingAcademyUkprn);
                var academy = academyResult;
                SetAdditionalInformation(academy, transferringAcademy);
                SetGeneralInformation(academy, transferringAcademy);
                academy.EducationPerformance = await SetPerformanceData(transferringAcademy, academy.LocalAuthorityName,
                    projectResult.Result.Urn);
                outgoingAcademies.Add(academy);
            }

            var response = new GetInformationForProjectResponse
            {
                Project = projectResult.Result,
                OutgoingAcademies = outgoingAcademies
            };

            return response;
        }

        private async Task<EducationPerformance> SetPerformanceData(TransferringAcademy transferringAcademy,
            string localAuthorityName, string projectUrn)
        {
            var educationPerformanceResult =
                await _educationPerformanceRepository.GetByAcademyUrn(transferringAcademy.OutgoingAcademyUrn);
            var performance = educationPerformanceResult.Result;
            performance.KeyStage2AdditionalInformation = transferringAcademy.KeyStage2PerformanceAdditionalInformation;
            performance.KeyStage4AdditionalInformation = transferringAcademy.KeyStage4PerformanceAdditionalInformation;
            performance.KeyStage5AdditionalInformation = transferringAcademy.KeyStage5PerformanceAdditionalInformation;
            performance.AcademyName = transferringAcademy.OutgoingAcademyName;
            performance.AcademyUkprn = transferringAcademy.OutgoingAcademyUkprn;
            performance.ProjectUrn = projectUrn;
            performance.LocalAuthorityName = localAuthorityName;
            return performance;
        }

        private static void SetAdditionalInformation(Academy academyDomain, TransferringAcademy academy)
        {
            academyDomain.PupilNumbers.AdditionalInformation = academy.PupilNumbersAdditionalInformation;
            academyDomain.LatestOfstedJudgement.AdditionalInformation = academy.LatestOfstedReportAdditionalInformation;
        }
        private static void SetGeneralInformation(Academy academyDomain, TransferringAcademy academy)
        {
            academyDomain.PFIScheme = academy.PFIScheme;
            academyDomain.PFISchemeDetails = academy.PFISchemeDetails;

            academyDomain.DistanceFromAcademyToTrustHq = academy.DistanceFromAcademyToTrustHq;
            academyDomain.DistanceFromAcademyToTrustHqDetails = academy.DistanceFromAcademyToTrustHqDetails;
            academyDomain.ViabilityIssues = academy.ViabilityIssues;
            academyDomain.FinancialDeficit = academy.FinancialDeficit;
            academyDomain.MPNameAndParty = academy.MPNameAndParty;
            academyDomain.PublishedAdmissionNumber = academy.PublishedAdmissionNumber;
        }
    }
}