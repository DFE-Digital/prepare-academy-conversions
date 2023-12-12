using Dfe.Academies.Contracts.V4.Establishments;
using Dfe.PrepareConversions.Data.Models;
using System;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services;

public class SchoolOverviewService
{
   private readonly IGetEstablishment _getEstablishment;

   public SchoolOverviewService(IGetEstablishment getEstablishment)
   {
      _getEstablishment = getEstablishment;
   }

   public async Task<SchoolOverview> GetSchoolOverviewByUrn(string urn)
   {
      // TODO: Technical Debt - enrich
      EstablishmentDto establishment = await _getEstablishment.GetEstablishmentByUrn(urn);
      SchoolOverview schoolOverview = new()
      {
         SchoolPostcode = establishment.Address?.Postcode,
         SchoolPhase = establishment.PhaseOfEducation?.Name,
         AgeRangeLower = establishment.StatutoryLowAge,
         AgeRangeUpper = establishment.StatutoryHighAge,
         SchoolType = establishment.EstablishmentType?.Name,
         NumberOnRoll = ToInt(establishment.Census?.NumberOfPupils),
         SchoolCapacity = ToInt(establishment.SchoolCapacity),
         PercentageFreeSchoolMeals = establishment.Census?.PercentageFsm,
         IsSchoolLinkedToADiocese = IsPartOfADiocesanTrust(establishment.Diocese),
         ParliamentaryConstituency = establishment.ParliamentaryConstituency?.Name
      };

      return schoolOverview;
   }

   private static string IsPartOfADiocesanTrust(NameAndCodeDto nameAndCode)
   {
      if (nameAndCode == null)
      {
         return null;
      }

      if (nameAndCode.Code == "0"
          || nameAndCode.Code == "0000"
          || nameAndCode.Name == null
          || nameAndCode.Name.Equals("Not applicable", StringComparison.OrdinalIgnoreCase))
      {
         return "No";
      }

      return $"Yes, {nameAndCode.Name}";
   }

   private static int? ToInt(string value)
   {
      if (int.TryParse(value, out int result))
      {
         return result;
      }

      return null;
   }
}
