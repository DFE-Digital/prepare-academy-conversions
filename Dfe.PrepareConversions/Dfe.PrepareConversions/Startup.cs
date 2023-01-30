using Dfe.PrepareConversions.Authorization;
using Dfe.PrepareConversions.Configuration;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.AzureAd;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Security;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

      services
         .AddRazorPages(options =>
         {
            options.Conventions.AuthorizeFolder("/");
         })
         .AddViewOptions(options =>
         {
            options.HtmlHelperOptions.ClientValidationEnabled = false;
         });

      services.AddControllersWithViews()
         .AddMicrosoftIdentityUI();

      services.AddScoped(sp => sp.GetService<IHttpContextAccessor>()?.HttpContext?.Session);
      services.AddSession(options =>
      {
         options.IdleTimeout = _authenticationExpiration;
         options.Cookie.Name = ".ManageAnAcademyConversion.Session";
         options.Cookie.IsEssential = true;
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
            {
               options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            }
         });

      services.AddScoped<IApiClient, ApiClient>();
      services.AddSingleton<PathFor>();

      services.AddHttpClient("TramsClient", (_, client) =>
      {
         TramsApiOptions tramsApiOptions = GetTypedConfigurationFor<TramsApiOptions>();
         client.BaseAddress = new Uri(tramsApiOptions.Endpoint);
         client.DefaultRequestHeaders.Add("ApiKey", tramsApiOptions.ApiKey);
      });

      services.AddHttpClient("AcademisationClient", (_, client) =>
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
      services.AddScoped<GeneralInformationService>();
      services.AddScoped<KeyStagePerformanceService>();
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
   }

   public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
   {
      logger.LogInformation("Feature Flag - Use Academisation API: {usingAcademisationApi}", IsFeatureEnabled(FeatureFlags.UseAcademisation));

      if (env.IsDevelopment())
      {
         app.UseDeveloperExceptionPage();
      }
      else
      {
         app.UseExceptionHandler("/Errors");
         // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
         app.UseHsts();
      }

      app.UseSecurityHeaders(
         SecurityHeadersDefinitions.GetHeaderPolicyCollection(env.IsDevelopment())
            .AddXssProtectionDisabled()
      );

      app.UseStatusCodePagesWithReExecute("/Errors", "?statusCode={0}");

      app.UseHttpsRedirection();

      //For Azure AD redirect uri to remain https
      ForwardedHeadersOptions forwardOptions = new() { ForwardedHeaders = ForwardedHeaders.All, RequireHeaderSymmetry = false };
      forwardOptions.KnownNetworks.Clear();
      forwardOptions.KnownProxies.Clear();
      app.UseForwardedHeaders(forwardOptions);

      app.UseStaticFiles();
      app.UseRouting();
      app.UseSentryTracing();
      app.UseSession();
      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
         endpoints.MapGet("/", context =>
         {
            context.Response.Redirect("project-type", false);
            return Task.CompletedTask;
         });
         endpoints.MapRazorPages();
         endpoints.MapControllerRoute("default", "{controller}/{action}/");
      });

      bool IsFeatureEnabled(string flag)
      {
         return (app.ApplicationServices.GetService(typeof(IFeatureManager)) as IFeatureManager)?.IsEnabledAsync(flag).Result ?? false;
      }
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
