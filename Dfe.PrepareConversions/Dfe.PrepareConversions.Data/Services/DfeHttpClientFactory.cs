using Dfe.Academisation.CorrelationIdMiddleware;
using System.Net.Http;

namespace Dfe.PrepareConversions.Data.Services;

/// <summary>
/// Creates an http client, with correlation context headers configured. You MUST register this as a scoped dependency
/// </summary>
public class DfeHttpClientFactory : IDfeHttpClientFactory
{
   private readonly IHttpClientFactory _httpClientFactory;
   private readonly ICorrelationContext _correlationContext;

   public DfeHttpClientFactory(IHttpClientFactory httpClientFactory, ICorrelationContext correlationContext)
   {
      _httpClientFactory = httpClientFactory;
      _correlationContext = correlationContext;
   }

   /// <summary>
   /// Creates an http client pointing to the trams/academies api, with correlation context headers configured
   /// </summary>
   /// <returns></returns>
   public HttpClient CreateTramsClient()
   {
      var httpClient = _httpClientFactory.CreateClient("TramsClient");
      httpClient.DefaultRequestHeaders.Add(Dfe.Academisation.CorrelationIdMiddleware.Keys.HeaderKey, _correlationContext.CorrelationId.ToString());
      return httpClient;
   }

   /// <summary>
   /// Creates an http client pointing to the academisation api, with correlation context headers configured
   /// </summary>
   /// <returns></returns>
   public HttpClient CreateAcademisationClient()
   {
      var httpClient = _httpClientFactory.CreateClient("AcademisationClient");
      httpClient.DefaultRequestHeaders.Add(Dfe.Academisation.CorrelationIdMiddleware.Keys.HeaderKey, _correlationContext.CorrelationId.ToString());
      return httpClient;
   }
}