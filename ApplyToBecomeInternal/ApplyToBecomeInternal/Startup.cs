using ApplyToBecome.Data.Features;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Services.AzureAd;
using ApplyToBecome.Data.Services.Interfaces;
using ApplyToBecomeInternal.Authorization;
using ApplyToBecomeInternal.Configuration;
using ApplyToBecomeInternal.Options;
using ApplyToBecomeInternal.Security;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal;

public class Startup
{
   public Startup(IConfiguration configuration)
   {
      Configuration = configuration;
   }

   public IConfiguration Configuration { get; }

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

      services.AddScoped(sp => sp.GetService<IHttpContextAccessor>()!.HttpContext.Session);
      services.AddSession(options =>
      {
         options.IdleTimeout =
            TimeSpan.FromMinutes(int.Parse(Configuration["AuthenticationExpirationInMinutes"]));
         options.Cookie.Name = ".ManageAnAcademyConversion.Session";
         options.Cookie.IsEssential = true;
      });
      services.AddHttpContextAccessor();
      ConfigureRedisConnection(services);

      services.AddAuthorization(options => { options.DefaultPolicy = SetupAuthorizationPolicyBuilder().Build(); });

      services.AddMicrosoftIdentityWebAppAuthentication(Configuration);
      services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme,
         options =>
         {
            options.AccessDeniedPath = "/access-denied";
            options.Cookie.Name = ".ManageAnAcademyConversion.Login";
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.ExpireTimeSpan =
               TimeSpan.FromMinutes(int.Parse(Configuration["AuthenticationExpirationInMinutes"]));
            options.SlidingExpiration = true;
            if (string.IsNullOrEmpty(Configuration["CI"]))
            {
               options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            }
         });

      services.AddScoped<IApiClient, ApiClient>();
      services.AddSingleton<PathFor>();

      services.AddHttpClient("TramsClient", (sp, client) =>
      {
         IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
         TramsApiOptions tramsApiOptions = configuration.GetSection(TramsApiOptions.Name).Get<TramsApiOptions>();
         client.BaseAddress = new Uri(tramsApiOptions.Endpoint);
         client.DefaultRequestHeaders.Add("ApiKey", tramsApiOptions.ApiKey);
      });

      services.AddHttpClient("AcademisationClient", (sp, client) =>
      {
         IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
         AcademisationApiOptions apiOptions = configuration.GetSection(AcademisationApiOptions.Name).Get<AcademisationApiOptions>();
         client.BaseAddress = new Uri(apiOptions.BaseUrl);
         client.DefaultRequestHeaders.Add("x-api-key", apiOptions.ApiKey);
      });

      services.Configure<ServiceLinkOptions>(Configuration.GetSection(ServiceLinkOptions.Name));
      services.Configure<AzureAdOptions>(Configuration.GetSection(AzureAdOptions.Name));

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
      services.AddScoped<IProjectNotesRepository, ProjectNotesRepository>();
      services.AddScoped<ApplicationRepository>();
      services.AddSingleton<IAuthorizationHandler, HeaderRequirementHandler>();
      services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IGraphClientFactory, GraphClientFactory>();
      services.AddScoped<IGraphUserService, GraphUserService>();
   }

   public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
   {
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
      ForwardedHeadersOptions forwardOptions = new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All, RequireHeaderSymmetry = false };
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
   }

   /// <summary>
   ///    Builds Authorization policy
   ///    Ensure authenticated user and restrict roles if they are provided in configuration
   /// </summary>
   /// <returns>AuthorizationPolicyBuilder</returns>
   private AuthorizationPolicyBuilder SetupAuthorizationPolicyBuilder()
   {
      AuthorizationPolicyBuilder policyBuilder = new AuthorizationPolicyBuilder();
      string allowedRoles = Configuration.GetSection("AzureAd")["AllowedRoles"];
      policyBuilder.RequireAuthenticatedUser();
      if (!string.IsNullOrWhiteSpace(allowedRoles))
      {
         policyBuilder.RequireClaim(ClaimTypes.Role, allowedRoles.Split(','));
      }

      return policyBuilder;
   }

   private void ConfigureRedisConnection(IServiceCollection services)
   {
      bool vcapServicesDefined = !string.IsNullOrEmpty(Configuration["VCAP_SERVICES"]);
      bool redisUrlDefined = !string.IsNullOrEmpty(Configuration["REDIS_URL"]);

      if (!vcapServicesDefined && !redisUrlDefined)
      {
         return;
      }

      string redisPass;
      string redisHost;
      string redisPort;
      bool redisTls = false;


      if (vcapServicesDefined)
      {
         JObject vcapConfiguration = JObject.Parse(Configuration["VCAP_SERVICES"]);
         JToken redisCredentials = vcapConfiguration["redis"]?[0]?["credentials"];
         redisPass = (string)redisCredentials?["password"];
         redisHost = (string)redisCredentials?["host"];
         redisPort = (string)redisCredentials?["port"];
         redisTls = (bool)redisCredentials?["tls_enabled"];
      }
      else
      {
         Uri redisUri = new Uri(Configuration["REDIS_URL"]);
         redisPass = redisUri.UserInfo.Split(":")[1];
         redisHost = redisUri.Host;
         redisPort = redisUri.Port.ToString();
      }

      ConfigurationOptions redisConfigurationOptions = new ConfigurationOptions { Password = redisPass, EndPoints = { $"{redisHost}:{redisPort}" }, Ssl = redisTls };

      ConnectionMultiplexer redisConnection = ConnectionMultiplexer.Connect(redisConfigurationOptions);

      services.AddStackExchangeRedisCache(
         options => { options.ConfigurationOptions = redisConfigurationOptions; });
      services.AddDataProtection().PersistKeysToStackExchangeRedis(redisConnection, "DataProtectionKeys");
   }
}
