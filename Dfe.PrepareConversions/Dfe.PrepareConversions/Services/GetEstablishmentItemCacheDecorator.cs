using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using EstablishmentDto = Dfe.Academies.Contracts.V4.Establishments.EstablishmentDto;

namespace Dfe.PrepareConversions.Services;

public class GetEstablishmentItemCacheDecorator : IGetEstablishment
{
   private readonly IGetEstablishment _getEstablishment;
   private readonly HttpContext _httpContext;

   public GetEstablishmentItemCacheDecorator(IGetEstablishment getEstablishment, IHttpContextAccessor httpContextAccessor)
   {
      _getEstablishment = getEstablishment;
      _httpContext = httpContextAccessor.HttpContext;
   }

   public async Task<EstablishmentDto> GetEstablishmentByUrn(string urn)
   {
      string key = $"establishment-{urn}";
      if (_httpContext.Items.ContainsKey(key) && _httpContext.Items[key] is EstablishmentDto cached)
      {
         return cached;
      }

      EstablishmentDto establishment = await _getEstablishment.GetEstablishmentByUrn(urn);

      _httpContext.Items[key] = establishment;

      return establishment;
   }

   public Task<IEnumerable<EstablishmentSearchResponse>> SearchEstablishments(string searchQuery)
   {
      string key = $"establishments-{searchQuery}";
      if (_httpContext.Items.ContainsKey(key) && _httpContext.Items[key] is IEnumerable<EstablishmentSearchResponse> cached)
      {
         return Task.FromResult(cached);
      }

      Task<IEnumerable<EstablishmentSearchResponse>> establishments = _getEstablishment.SearchEstablishments(searchQuery);

      _httpContext.Items[key] = establishments;

      return establishments;
   }
}
