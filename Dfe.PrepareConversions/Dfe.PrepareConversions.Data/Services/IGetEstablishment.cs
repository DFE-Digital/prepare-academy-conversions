using Dfe.PrepareConversions.Data.Models.Establishment;
using DfE.CoreLibs.Contracts.Academies.V4.Establishments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services;

public interface IGetEstablishment
{
   Task<EstablishmentDto> GetEstablishmentByUrn(string urn);
   Task<IEnumerable<EstablishmentSearchResponse>> SearchEstablishments(string searchQuery);
}
