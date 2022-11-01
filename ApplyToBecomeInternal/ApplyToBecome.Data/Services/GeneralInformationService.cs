using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.Establishment;
using System;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	public class GeneralInformationService
	{
		private readonly IGetEstablishment _getEstablishment;

		public GeneralInformationService(IGetEstablishment getEstablishment)
		{
			_getEstablishment = getEstablishment;
		}

		public async Task<GeneralInformation> GetGeneralInformationByUrn(string urn)
		{
			var establishment = await _getEstablishment.GetEstablishmentByUrn(urn);
			var generalInformation = new GeneralInformation
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

			return generalInformation;
		}

		private string IsPartOfADiocesanTrust(NameAndCodeResponse nameAndCode)
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

		private int? ToInt(string value)
		{
			if (int.TryParse(value, out var result))
			{
				return result;
			}
			return null;
		}
	}
}
