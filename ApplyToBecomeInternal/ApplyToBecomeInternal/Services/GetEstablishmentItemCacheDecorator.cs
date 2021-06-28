using ApplyToBecome.Data.Models.Establishment;
using ApplyToBecome.Data.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Services
{
	public class GetEstablishmentItemCacheDecorator : IGetEstablishment
	{
		private readonly IGetEstablishment _getEstablishment;
		private readonly HttpContext _httpContext;

		public GetEstablishmentItemCacheDecorator(IGetEstablishment getEstablishment, IHttpContextAccessor httpContextAccessor)
		{
			_getEstablishment = getEstablishment;
			_httpContext = httpContextAccessor.HttpContext;
		}

		public async Task<EstablishmentResponse> GetEstablishmentByUrn(string urn)
		{
			var key = $"establishment-{urn}";
			if (_httpContext.Items.ContainsKey(key) && _httpContext.Items[key] is EstablishmentResponse cached)
			{
				return cached;
			}

			var establishment = await _getEstablishment.GetEstablishmentByUrn(urn);

			_httpContext.Items[key] = establishment;

			return establishment;
		}
	}
}
