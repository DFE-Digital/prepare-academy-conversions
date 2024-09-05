using Dfe.Academisation.CorrelationIdMiddleware;
using Dfe.PrepareTransfers.Web.Services;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Dfe.PrepareConversions.Authorization;
using Dfe.PrepareConversions.Configuration;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.AzureAd;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Routing;
using Dfe.PrepareConversions.Security;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareTransfers.Web.BackgroundServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions;

public class Startup
{
   private readonly TimeSpan _authenticationExpiration;

   public Startup(IConfiguration configuration)
   {
      Configuration = configuration;

      _authenticationExpiration = TimeSpan.FromMinutes(int.Parse(Configuration["AuthenticationExpirationInMinutes"] ?? "60"));
   }

   private IConfiguration Configuration { get; }

   private IConfigurationSection GetConfigurationSectionFor<T>()
   {
      string sectionName = typeof(T).Name.Replace("Options", string.Empty);
      return Configuration.GetRequiredSection(sectionName);
   }

   private T GetTypedConfigurationFor<T>()
   {
      return GetConfigurationSectionFor<T>().Get<T>();
   }

   public void ConfigureServices(IServiceCollection services)
   {
      services.AddFeatureManagement();
      services.AddApplicationInsightsTelemetry();
      services.AddHealthChecks();
      services
         .AddRazorPages(options =>
         {
            options.Conventions.AuthorizeFolder("/");
            options.Conventions.AllowAnonymousToPage("/public/maintenance");
            options.Conventions.AllowAnonymousToPage("/public/accessibility");
            options.Conventions.AddAreaPageRoute("Transfers", "/Index", "/transfers");
         })
         .AddViewOptions(options =>
         {
            options.HtmlHelperOptions.ClientValidationEnabled = false;
         }).AddMvcOptions(options =>
         {
            options.MaxModelValidationErrors = 50;
            options.Filters.Add(new MaintenancePageFilter(Configuration));
         });

      services.AddControllersWithViews()
         .AddMicrosoftIdentityUI();

      // Enforce HTTPS in ASP.NET Core
      // @link https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?
      services.AddHsts(options =>
      {
         options.Preload = true;
         options.IncludeSubDomains = true;
         options.MaxAge = TimeSpan.FromDays(365);
      });

      services.AddScoped(sp => sp.GetService<IHttpContextAccessor>()?.HttpContext?.Session);
      services.AddSession(options =>
      {
         options.IdleTimeout = _authenticationExpiration;
         options.Cookie.Name = ".ManageAnAcademyConversion.Session";
         options.Cookie.IsEssential = true;
         options.Cookie.HttpOnly = true;

         if (string.IsNullOrWhiteSpace(Configuration["CI"]))
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
      });
      services.AddHttpContextAccessor();

      services.AddAuthorization(options => { options.DefaultPolicy = SetupAuthorizationPolicyBuilder().Build(); });

      services.AddMicrosoftIdentityWebAppAuthentication(Configuration);
      services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme,
         options =>
         {
            options.AccessDeniedPath = "/access-denied";
            options.Cookie.Name = ".ManageAnAcademyConversion.Login";
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.ExpireTimeSpan = _authenticationExpiration;
            options.SlidingExpiration = true;

            if (string.IsNullOrEmpty(Configuration["CI"]))
               options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
         });

      services.AddScoped<IApiClient, ApiClient>();
      services.AddSingleton<PathFor>();

      services.AddHttpClient(DfeHttpClientFactory.TramsClientName, (sp, client) =>
      {
         TramsApiOptions tramsApiOptions = GetTypedConfigurationFor<TramsApiOptions>();
         client.BaseAddress = new Uri(tramsApiOptions.Endpoint);
         client.DefaultRequestHeaders.Add("ApiKey", tramsApiOptions.ApiKey);

      });

      services.AddHttpClient(DfeHttpClientFactory.AcademisationClientName, (sp, client) =>
      {
         AcademisationApiOptions apiOptions = GetTypedConfigurationFor<AcademisationApiOptions>();
         client.BaseAddress = new Uri(apiOptions.BaseUrl);
         client.DefaultRequestHeaders.Add("x-api-key", apiOptions.ApiKey);
      });

      services.Configure<ServiceLinkOptions>(GetConfigurationSectionFor<ServiceLinkOptions>());
      services.Configure<AzureAdOptions>(GetConfigurationSectionFor<AzureAdOptions>());

      services.AddScoped<ErrorService>();
      services.AddScoped<IGetEstablishment, EstablishmentService>();
      services.Decorate<IGetEstablishment, GetEstablishmentItemCacheDecorator>();
      services.AddScoped<SchoolPerformanceService>();
      services.AddScoped<SchoolOverviewService>();
      services.AddScoped<KeyStagePerformanceService>();
      services.AddScoped<ITrustsRepository, TrustsRepository>();
      services.AddScoped<IProjectGroupsRepository, ProjectGroupsRepository>();
      services.AddScoped<IAcademyConversionProjectRepository, AcademyConversionProjectRepository>();
      services.AddScoped<IAcademyConversionAdvisoryBoardDecisionRepository, AcademyConversionAdvisoryBoardDecisionRepository>();
      services.AddScoped<IHttpClientService, HttpClientService>();
      services.Decorate<IAcademyConversionProjectRepository, AcademyConversionProjectItemsCacheDecorator>();
      services.AddScoped<ApplicationRepository>();
      services.AddSingleton<IAuthorizationHandler, HeaderRequirementHandler>();
      services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IGraphClientFactory, GraphClientFactory>();
      services.AddScoped<IGraphUserService, GraphUserService>();
      services.AddScoped<IDfeHttpClientFactory, DfeHttpClientFactory>();
      services.AddScoped<ICorrelationContext, CorrelationContext>();
      services.AddTransient<ITaskListService, TaskListService>();
      services.AddSingleton<PerformanceDataChannel>();

      services.Configure<SharePointOptions>(Configuration.GetSection("Sharepoint"));
      var sharepointOptions = Configuration.GetSection("Sharepoint").Get<SharePointOptions>();

      Links.InializeProjectDocumentsEnabled(sharepointOptions.Enabled);

      // Initialize the TransfersUrl
      var serviceLinkOptions = Configuration.GetSection("ServiceLink").Get<ServiceLinkOptions>();

      Links.InitializeTransfersUrl(serviceLinkOptions.TransfersUrl);

      // use this to section off the transfers specific dependencies
      services.AddTransfersApplicationServices();

   }

   public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
   {
      // Ensure we do not lose X-Forwarded-* Headers when behind a Proxy
      var forwardOptions = new ForwardedHeadersOptions {
         ForwardedHeaders = ForwardedHeaders.All,
         RequireHeaderSymmetry = false
      };
      forwardOptions.KnownNetworks.Clear();
      forwardOptions.KnownProxies.Clear();
      app.UseForwardedHeaders(forwardOptions);

      if (env.IsDevelopment())
      {
         app.UseDeveloperExceptionPage();
      }
      else
      {
         app.UseExceptionHandler("/Errors");
      }

      app.UseSecurityHeaders(SecurityHeadersDefinitions.GetHeaderPolicyCollection(env.IsDevelopment()));
      app.UseHsts();

      app.UseCookiePolicy(new CookiePolicyOptions { Secure = CookieSecurePolicy.Always, HttpOnly = HttpOnlyPolicy.Always });

      app.UseStatusCodePagesWithReExecute("/Errors", "?statusCode={0}");

      app.UseHttpsRedirection();
      app.UseHealthChecks("/health");

      app.UseStaticFiles();
      app.UseRouting();
      app.UseSentryTracing();
      app.UseSession();
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseMiddleware<CorrelationIdMiddleware>();

      app.UseEndpoints(endpoints =>
      {
         endpoints.MapGet("/", context =>
         {
            context.Response.Redirect("project-list", false);
            return Task.CompletedTask;
         });
         endpoints.MapRazorPages();
         endpoints.MapControllerRoute("default", "{controller}/{action}/");
      });
   }

   /// <summary>
   ///    Builds Authorization policy
   ///    Ensure authenticated user and restrict roles if they are provided in configuration
   /// </summary>
   /// <returns>AuthorizationPolicyBuilder</returns>
   private AuthorizationPolicyBuilder SetupAuthorizationPolicyBuilder()
   {
      AuthorizationPolicyBuilder policyBuilder = new();
      policyBuilder.RequireAuthenticatedUser();

      string allowedRoles = Configuration.GetSection("AzureAd")["AllowedRoles"];
      if (string.IsNullOrWhiteSpace(allowedRoles) is false)
      {
         policyBuilder.RequireClaim(ClaimTypes.Role, allowedRoles.Split(','));
      }

      return policyBuilder;
   }
}
