using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Data.Models.Projects;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Data
{
   public interface IProjects
   {
      Task<RepositoryResult<List<ProjectSearchResult>>> GetProjects(GetProjectSearchModel searchModel);
      Task<RepositoryResult<Project>> GetByUrn(string urn);
      Task<RepositoryResult<Project>> Create(Project project);
      Task<bool> UpdateRationale(Project project);
      Task<bool> UpdateFeatures(Project project);
      Task<bool> UpdateBenefits(Project project);
      Task<bool> UpdateGeneralInfomation(Project project);
      Task<bool> UpdateLegalRequirements(Project project);
      Task<bool> UpdateDates(Project project);
      Task<bool> UpdateDates(Project project, List<ReasonChange> reasonsChanged, string ChangedBy);
      Task<bool> UpdateAcademy(string urn, TransferringAcademy academy);
      Task<bool> UpdateAcademyGeneralInformation(string projectUrn, TransferringAcademy transferringAcademy);
      Task<bool> UpdateStatus(Project project);
      Task<bool> UpdateIncomingTrust(string urn, string projectName, string incomingTrustReferenceNumber, string incomingTrustUKPRN = "");

      Task<bool> AssignUser(Project project);
      Task<ApiResponse<FileStreamResult>> DownloadProjectExport(GetProjectSearchModel searchModel);
      Task<ApiResponse<ProjectFilterParameters>> GetFilterParameters();
      Task<IEnumerable<OpeningDateHistoryDto>> GetOpeningDateHistory(int urn);
      Task DeleteProjectAsync(string urn);
   }
}