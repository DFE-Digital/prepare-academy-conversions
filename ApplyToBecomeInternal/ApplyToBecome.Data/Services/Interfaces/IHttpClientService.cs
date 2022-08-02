using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services.Interfaces
{
	public interface IHttpClientService
	{
		Task<ApiResponse<TResponse>> Post<TRequest, TResponse>(string path, TRequest requestBody)
			where TResponse : class;
	   Task<ApiResponse<TResponse>> Get<TResponse>(string path)
		   where TResponse : class;
	}
}