using System.Net.Http;

namespace Dfe.PrepareConversions.Data.Services;

public interface IDfeHttpClientFactory
{
   /// <summary>
   /// Creates an http client pointing to the trams/academies api, with correlation context headers configured
   /// </summary>
   /// <returns></returns>
   HttpClient CreateTramsClient();

   /// <summary>
   /// Creates an http client pointing to the academisation api, with correlation context headers configured
   /// </summary>
   /// <returns></returns>
   HttpClient CreateAcademisationClient();
}
