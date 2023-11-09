using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Models.SponsoredProject;
using Dfe.PrepareConversions.Data.Models.Trust;
using System;

namespace Dfe.PrepareConversions.Mappings;

public static class CreateSponsoredProjectMapper
{
   public static CreateSponsoredProject MapToDto(EstablishmentResponse establishment, TrustDetail trust)
   {
      var partOfPfiScheme = !string.IsNullOrWhiteSpace(establishment.ViewAcademyConversion?.Pfi)
                            && establishment.ViewAcademyConversion?.Pfi.Equals("No", StringComparison.InvariantCultureIgnoreCase) == false;
      
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