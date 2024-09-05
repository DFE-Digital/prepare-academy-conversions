using Dfe.Academisation.CorrelationIdMiddleware; 
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Extensions; 
using Microsoft.AspNetCore.Http; 
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks; 

namespace Dfe.PrepareConversions.Middleware
{
   public class CapabilitiyMiddleware(RequestDelegate next, ILogger<CapabilitiyMiddleware> logger)
   {
      public const string SESSION_KEY = "RoleCapabilities"; 
      private readonly ILogger<CapabilitiyMiddleware> _logger = logger ?? throw new ArgumentNullException("logger");

      public async Task Invoke(HttpContext httpContext, ICorrelationContext correlationContext, ISession session, IUserRoleRepository userRoleRepository)
      {
         if (httpContext.User.Identity.IsAuthenticated && !httpContext.User.Identity.Name.IsNullOrEmpty())
         {
            var sessionKey = $"{SESSION_KEY}_{httpContext.User.Identity.Name}";
            if (session.Get<string>(sessionKey).IsNullOrEmpty())
            {
               SetCorrelationId(httpContext, correlationContext);

               var roleCapabilitiesModel = await userRoleRepository.GetUserRoleCapabilities(httpContext.User.Identity.Name);
               httpContext.Session.Set(sessionKey, string.Join(",", roleCapabilitiesModel.Body.Capabilities));
            }
         }

         await next(httpContext);
      }

      private void SetCorrelationId(HttpContext httpContext, ICorrelationContext correlationContext)
      {
         Guid correlationId;
         if (httpContext.Request.Headers.TryGetValue("x-correlationId", out StringValues value) && !string.IsNullOrWhiteSpace(value))
         {
            if (!Guid.TryParse(value, out correlationId))
            {
               correlationId = Guid.NewGuid();
               _logger.LogWarning("Detected header x-correlationId, but value cannot be parsed to a GUID. Other values are not supported. Generated a new one: {correlationId}", correlationId);
            }
            else
            {
               _logger.LogInformation("CorrelationIdMiddleware:Invoke - x-correlationId detected in request headers: {correlationId}", correlationId);
            }
         }
         else
         {
            correlationId = Guid.NewGuid();
            _logger.LogWarning("CorrelationIdMiddleware:Invoke - x-correlationId not detected in request headers. Generated a new one: {correlationId}", correlationId);
         }
         httpContext.Request.Headers["x-correlationId"] = correlationId.ToString();
         correlationContext.SetContext(correlationId);
      }
   }
}
