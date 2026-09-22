
#nullable enable

using Dfe.PrepareConversions.Data.Models.SignificantChange;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services.Interfaces;

public interface ISignificantChangeProjectRepository
{
   public record SignificantChangeFilterOptions(
      string? Keyword = null,
      string[]? Statuses = null,
      string[]? Assignees = null,
      byte[]? Tiers = null,
      string[]? Routes = null,
      string[]? LocalAuthorities = null);

   Task<ApiResponse<SignificantChangeProjectResponse>> CreateProject(CreateSignificantProjectCommand command);

   Task<ApiResponse<ApiV2Wrapper<IEnumerable<SignificantChangeProjectResponse>>>> GetAllProjects(
      int page,
      int count,
      SignificantChangeFilterOptions? filterOptions = null);

   Task<ApiResponse<SignificantChangeProjectResponse>> GetProjectById(int id);

   Task<ApiResponse<SignificantChangeFilterParameters>> GetFilterParameters();

   Task SetAssignedUser(int id, SetAssignedUserSignificantChangeCommand updatedAssignedUser);
   Task RecordDecision(SignificantChangeDecision decision);
   Task UpdateDecision(SignificantChangeDecision decision);
   Task<ApiResponse<SignificantChangeDecision>> GetDecision(int id);
   Task SetConsultationDuration(int id, SetSignificantChangeConsultationDurationCommand command);
   Task SetStakeholderConsultation(int id, SetSignificantChangeStakeholderConsultationCommand command);
   Task SetEqualitiesImpactAssessment(int id, SetSignificantChangeEqualitiesImpactAssessmentCommand command);
   Task SetReligiousBodyConsultation(int id, SetSignificantChangeReligiousBodyConsultationCommand command);
   Task SetProjectDates(int id, SetSignificantChangeProjectDatesCommand command);
   Task SetStakeholderObjections(int id, SetSignificantChangeStakeholderObjectionsCommand command);
   Task SetAdmissionVariationConsultation(int id, SetSignificantChangeAdmissionVariationConsultationCommand command);

}
