using Dfe.Academies.Contracts.V4.Establishments;
using Dfe.Academies.Contracts.V4.Trusts;
using Dfe.PrepareConversions.Data.Models.NewProject;
using System;

namespace Dfe.PrepareConversions.Mappings;

public static class CreateProjectMapper
{
   public static CreateNewProject MapToDto(EstablishmentDto establishment, TrustDto trust, string HasSchoolApplied, string HasPreferredTrust)
   {
      if (establishment == null)
      {
         throw new ArgumentNullException(nameof(establishment));
      }

      var partOfPfiScheme = !string.IsNullOrWhiteSpace(establishment?.Pfi)
                      && !establishment.Pfi.Equals("No", StringComparison.InvariantCultureIgnoreCase);


      NewProjectSchool createSchool = new(
         establishment.Name,
         establishment.Urn,
         partOfPfiScheme,
         establishment.LocalAuthorityName,
         establishment.Gor.Name);

      NewProjectTrust createTrust = null;
      if (trust != null)
      {
         createTrust = new(
            trust.Name,
            trust.ReferenceNumber);
      }

      return new CreateNewProject(createSchool, createTrust, HasSchoolApplied, HasPreferredTrust);
   }
}