using Dfe.Academies.Contracts.V4.Establishments;
using Dfe.Academies.Contracts.V4.Trusts;
using Dfe.Academisation.CorrelationIdMiddleware;
using Dfe.PrepareConversions.Authorization;
using Dfe.PrepareConversions.Data.Services.AzureAd;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareTransfers.Data.Models.KeyStagePerformance;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Data.Services.Interfaces;
using Dfe.PrepareTransfers.Data.TRAMS;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Dfe.PrepareTransfers.Data.TRAMS.Mappers.Response;
using Dfe.PrepareTransfers.Data.TRAMS.Models;
using Dfe.PrepareTransfers.Data.TRAMS.Models.EducationPerformance;
using Dfe.PrepareTransfers.Data.TRAMS.Mappers.Request;

namespace Dfe.PrepareConversions.Utils
{
   internal static class TransfersExtensions
   {
      public static void AddTransfersApplicationServices(this IServiceCollection services)
      {
         //services.AddScoped<IReferenceNumberService, ReferenceNumberService>();
         //services.AddScoped<ErrorService>();



         services.AddTransient<IMapper<TrustDto, Trust>, TramsTrustMapper>();
         services.AddTransient<IMapper<EstablishmentDto, Academy>, AcademiesEstablishmentMapper>();
         services.AddTransient<IMapper<TramsProjectSummary, ProjectSearchResult>, TramsProjectSummariesMapper>();
         services.AddTransient<IMapper<AcademisationProject, Project>, AcademisationProjectMapper>();
         //services.AddTransient<IMapper<TramsEducationPerformance, EducationPerformance>, TramsEducationPerformanceMapper>();
         services.AddTransient<IMapper<Project, TramsProjectUpdate>, InternalProjectToUpdateMapper>();
         services.AddTransient<ITrusts, TramsTrustsRepository>();
         services.AddTransient<IAcademies, TramsEstablishmentRepository>();
         //services.AddTransient<IEducationPerformance, TramsEducationPerformanceRepository>();
         services.AddTransient<IProjects, TramsProjectsRepository>();
         //services.AddTransient<ICreateProjectTemplate, CreateProjectTemplate>();
         //services.AddTransient<IGetInformationForProject, GetInformationForProject>();
         //services.AddTransient<IGetProjectTemplateModel, GetProjectTemplateModel>();
         //services.AddTransient<ITaskListService, TaskListService>();

         //services.AddTransient<IUserRepository, UserRepository>();
         //services.AddTransient<IGraphClientFactory, GraphClientFactory>();
         //services.AddTransient<IGraphUserService, GraphUserService>();

         //services.AddScoped<IAcademyTransfersAdvisoryBoardDecisionRepository, AcademyTransfersAdvisoryBoardDecisionRepository>();

         //services.AddSingleton<PerformanceDataChannel>();
         services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
         //services.AddSingleton<IAuthorizationHandler, HeaderRequirementHandler>();
         //services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();
         //services.AddScoped<ICorrelationContext, CorrelationContext>();

         //services.AddHostedService<PerformanceDataProcessingService>();
      }
   }
}
