using System.Net;

namespace ApplyToBecome.Data
{
	public class ApiResponse<TBody>
	{
		public ApiResponse(HttpStatusCode statusCode, TBody body)
		{
			Success = (int)statusCode >= 200 && (int)statusCode < 300;
			Body = body;
		}

		public bool Success { get; }
		public TBody Body { get; }
	}
}
