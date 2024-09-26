using Dfe.Academisation.CorrelationIdMiddleware;
using System.Net.Http;

namespace Dfe.Prepare.Data;

/// <summary>
/// Creates an http client, with correlation context headers configured. You MUST register this as a scoped dependency
/// </summary>
public class DfeHttpClientFactory : IDfeHttpClientFactory
{
   private readonly IHttpClientFactory _httpClientFactory;
   private readonly ICorrelationContext _correlationContext;

   public const string TramsClientName = "TramsClient";
   public const string AcademisationClientName = "AcademisationClient";

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
      return CreateClient(TramsClientName);
   }

   /// <summary>
   /// Creates an http client pointing to the academisation api, with correlation context headers configured
   /// </summary>
   /// <returns></returns>
   public HttpClient CreateAcademisationClient()
   {
      return CreateClient(AcademisationClientName);
   }

   /// <summary>
   /// Creates an http client, with correlation context headers configured
   /// </summary>
   /// <param name="name">The name.</param>
   /// <returns>A HttpClient.</returns>
   public HttpClient CreateClient(string name)
   {
      var httpClient = _httpClientFactory.CreateClient(name);
      httpClient.DefaultRequestHeaders.Add(Dfe.Academisation.CorrelationIdMiddleware.Keys.HeaderKey, _correlationContext.CorrelationId.ToString());
      return httpClient;
   }
}