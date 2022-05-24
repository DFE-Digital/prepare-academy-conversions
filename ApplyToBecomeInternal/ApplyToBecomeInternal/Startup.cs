using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Configuration;
using ApplyToBecomeInternal.Security;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Security.Claims;

namespace ApplyToBecomeInternal
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			_env = env;
		}

		public IConfiguration Configuration { get; }
		private readonly IWebHostEnvironment _env;

		public void ConfigureServices(IServiceCollection services)
		{
			var razorPages = services
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
			
			if (_env.IsDevelopment())
			{
				razorPages.AddRazorRuntimeCompilation();
			}

			services.AddHttpContextAccessor();

			ConfigureRedisConnection(services);
			
			services.AddAuthorization(options => { options.DefaultPolicy = SetupAuthorizationPolicyBuilder().Build(); });
			
			services.AddMicrosoftIdentityWebAppAuthentication(Configuration);
			services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme,
				options =>
				{
					options.AccessDeniedPath = "/access-denied";
					options.LogoutPath = "/signed-out";
					options.Cookie.Name = "ManageAnAcademyTransfer.Login";
					options.Cookie.HttpOnly = true;
					options.Cookie.IsEssential = true;
					options.ExpireTimeSpan =
						TimeSpan.FromMinutes(Int32.Parse(Configuration["AuthenticationExpirationInMinutes"]));
					options.SlidingExpiration = true;
					if (string.IsNullOrEmpty(Configuration["CI"]))
					{
						options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
					}
				});

			services.AddHttpClient("TramsClient", (sp, client) =>
			{
				var configuration = sp.GetRequiredService<IConfiguration>();
				var tramsApiOptions = configuration.GetSection(TramsApiOptions.Name).Get<TramsApiOptions>();
				client.BaseAddress = new Uri(tramsApiOptions.Endpoint);
				client.DefaultRequestHeaders.Add("ApiKey", tramsApiOptions.ApiKey);
			});

			services.AddScoped<ErrorService>();
			services.AddScoped<IGetEstablishment, EstablishmentService>();
			services.Decorate<IGetEstablishment, GetEstablishmentItemCacheDecorator>();
			services.AddScoped<SchoolPerformanceService>();
			services.AddScoped<GeneralInformationService>();
			services.AddScoped<KeyStagePerformanceService>();
			services.AddScoped<IAcademyConversionProjectRepository, AcademyConversionProjectRepository>();
			services.Decorate<IAcademyConversionProjectRepository, AcademyConversionProjectItemsCacheDecorator>();
			services.AddScoped<IProjectNotesRepository, ProjectNotesRepository>();
			services.AddScoped<ApplicationRepository>();
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
				SecureHeadersDefinitions.SecurityHeadersDefinitions.GetHeaderPolicyCollection(env.IsDevelopment()));

			app.UseStatusCodePagesWithReExecute("/Errors", "?statusCode={0}");

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseSentryTracing();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllerRoute("default", "{controller}/{action}/");
			});
		}
		
		/// <summary>
		/// Builds Authorization policy
		/// Ensure authenticated user and restrict roles if they are provided in configuration
		/// </summary>
		/// <returns>AuthorizationPolicyBuilder</returns>
		private AuthorizationPolicyBuilder SetupAuthorizationPolicyBuilder()
		{
			var policyBuilder = new AuthorizationPolicyBuilder();
			var allowedRoles = Configuration.GetSection("AzureAd")["AllowedRoles"];
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
			var redisTls = false;


			if (vcapServicesDefined)
			{
				var vcapConfiguration = JObject.Parse(Configuration["VCAP_SERVICES"]);
				var redisCredentials = vcapConfiguration["redis"]?[0]?["credentials"];
				redisPass = (string)redisCredentials?["password"];
				redisHost = (string)redisCredentials?["host"];
				redisPort = (string)redisCredentials?["port"];
				redisTls = (bool)redisCredentials?["tls_enabled"];
			}
			else
			{
				var redisUri = new Uri(Configuration["REDIS_URL"]);
				redisPass = redisUri.UserInfo.Split(":")[1];
				redisHost = redisUri.Host;
				redisPort = redisUri.Port.ToString();
			}

			var redisConfigurationOptions = new ConfigurationOptions { Password = redisPass, EndPoints = { $"{redisHost}:{redisPort}" }, Ssl = redisTls };

			var redisConnection = ConnectionMultiplexer.Connect(redisConfigurationOptions);

			services.AddStackExchangeRedisCache(
				options => { options.ConfigurationOptions = redisConfigurationOptions; });
			services.AddDataProtection().PersistKeysToStackExchangeRedis(redisConnection, "DataProtectionKeys");
		}
	}
}