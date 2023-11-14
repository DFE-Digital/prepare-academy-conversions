using Dfe.Academies.Contracts.V4.Establishments;
using Dfe.Academies.Contracts.V4.Trusts;
using Dfe.PrepareConversions.Data.Models.SponsoredProject;
using System;

namespace Dfe.PrepareConversions.Mappings;

public static class CreateSponsoredProjectMapper
{
   public static CreateSponsoredProject MapToDto(EstablishmentDto establishment, TrustDto trust)
   {
      var partOfPfiScheme = !string.IsNullOrWhiteSpace(establishment?.Pfi)
                            && establishment?.Pfi.Equals("No", StringComparison.InvariantCultureIgnoreCase) == false;

      SponsoredProjectSchool createSchool = new(
         establishment.Name,
         establishment.Urn,
         partOfPfiScheme,
         establishment.LocalAuthorityName,
         establishment.Gor.Name);

      SponsoredProjectTrust createTrust = new(
         trust.Name,
         trust.ReferenceNumber);

      return new CreateSponsoredProject(createSchool, createTrust);
   }
}