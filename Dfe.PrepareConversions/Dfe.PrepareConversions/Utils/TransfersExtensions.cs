using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Data.Models.KeyStagePerformance;
using Dfe.PrepareTransfers.Data.Services.Interfaces;
using Dfe.PrepareTransfers.Data.TRAMS;
using Dfe.PrepareTransfers.Data.TRAMS.Mappers.Request;
using Dfe.PrepareTransfers.Data.TRAMS.Mappers.Response;
using Dfe.PrepareTransfers.Data.TRAMS.Models;
using Dfe.PrepareTransfers.Data.TRAMS.Models.EducationPerformance;
using Dfe.PrepareTransfers.Helpers;
using Dfe.PrepareTransfers.Web.Services;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Trusts;
using Microsoft.Extensions.DependencyInjection;

namespace Dfe.PrepareConversions.Utils;

internal static class TransfersExtensions
{
   public static void AddTransfersApplicationServices(this IServiceCollection services)
   {
      services.AddScoped<IReferenceNumberService, ReferenceNumberService>();

      services.AddTransient<IMapper<TrustDto, Trust>, TramsTrustMapper>();
      services.AddTransient<IMapper<EstablishmentDto, Academy>, AcademiesEstablishmentMapper>();
      services.AddTransient<IMapper<TramsProjectSummary, ProjectSearchResult>, TramsProjectSummariesMapper>();
      services.AddTransient<IMapper<AcademisationProject, Project>, AcademisationProjectMapper>();
      services.AddTransient<IMapper<TramsEducationPerformance, EducationPerformance>, TramsEducationPerformanceMapper>();
      services.AddTransient<IMapper<Project, TramsProjectUpdate>, InternalProjectToUpdateMapper>();
      services.AddTransient<ITrusts, TramsTrustsRepository>();
      services.AddTransient<IAcademies, TramsEstablishmentRepository>();
      services.AddTransient<IEducationPerformance, TramsEducationPerformanceRepository>();
      services.AddTransient<IProjects, TramsProjectsRepository>();
      services.AddTransient<ICreateProjectTemplate, CreateProjectTemplate>();
      services.AddTransient<IGetInformationForProject, GetInformationForProject>();
      services.AddTransient<IGetProjectTemplateModel, GetProjectTemplateModel>();

      services.AddScoped<IAcademyTransfersAdvisoryBoardDecisionRepository, AcademyTransfersAdvisoryBoardDecisionRepository>();
      services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
   }
}