using Dfe.PrepareTransfers.Data.Models;
using DfE.CoreLibs.Contracts.Academies.V4.Trusts;
using System.Collections.Generic;

namespace Dfe.PrepareTransfers.Data.TRAMS.Mappers.Response
{
   public class TramsTrustMapper : IMapper<TrustDto, Trust>
   {

      public Trust Map(TrustDto input)
      {
         var address = input.Address;
         return new Trust
         {
            Address = new List<string>
                {
                    input.Name,
                    address.Street,
                    address.Town,
                    $"{address.County}, {address.Postcode}"
                },
            EstablishmentType = "Not available", // Not sure why this even here, surely thsi should be the trust type
            CompaniesHouseNumber = input.CompaniesHouseNumber,
            GiasGroupId = input.ReferenceNumber,
            Name = input.Name,
            Ukprn = input.Ukprn
         };
      }
   }
}