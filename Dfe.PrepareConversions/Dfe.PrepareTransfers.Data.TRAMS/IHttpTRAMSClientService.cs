using System.Net.Http;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Data.TRAMS;

public interface IHttpClientService
{
   Task<ApiResponse<TResponse>> Post<TRequest, TResponse>(HttpClient httpClient, string path, TRequest requestBody)
      where TResponse : class;

   Task<ApiResponse<TResponse>> Put<TRequest, TResponse>(HttpClient httpClient, string path, TRequest requestBody)
      where TResponse : class;

   Task<ApiResponse<TResponse>> Get<TResponse>(HttpClient httpClient, string path)
      where TResponse : class;

   Task<ApiResponse<TResponse>> Delete<TResponse>(HttpClient httpClient, string path)
      where TResponse : class;
}
