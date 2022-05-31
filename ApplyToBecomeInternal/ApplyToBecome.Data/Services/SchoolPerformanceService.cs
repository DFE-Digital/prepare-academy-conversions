using ApplyToBecome.Data.Models;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	public class SchoolPerformanceService
	{
		private readonly IGetEstablishment _getEstablishment;

		public SchoolPerformanceService(IGetEstablishment getEstablishment)
		{
			_getEstablishment = getEstablishment;
		}

		public async Task<SchoolPerformance> GetSchoolPerformanceByUrn(string urn)
		{
			var establishment = await _getEstablishment.GetEstablishmentByUrn(urn);
			var schoolPerformance = new SchoolPerformance();

			if (establishment.MISEstablishment != null)
			{
				schoolPerformance.OverallEffectiveness = establishment.MISEstablishment.OverallEffectiveness;
				schoolPerformance.QualityOfEducation = establishment.MISEstablishment.QualityOfEducation;
				schoolPerformance.BehaviourAndAttitudes = establishment.MISEstablishment.BehaviourAndAttitudes;
				schoolPerformance.PersonalDevelopment = establishment.MISEstablishment.PersonalDevelopment;
				schoolPerformance.EffectivenessOfLeadershipAndManagement = establishment.MISEstablishment.EffectivenessOfLeadershipAndManagement;
				schoolPerformance.SixthFormProvision = establishment.MISEstablishment.SixthFormProvision;
				schoolPerformance.EarlyYearsProvision = establishment.MISEstablishment.EarlyYearsProvision;
				schoolPerformance.InspectionEndDate = TryParseDate(establishment.MISEstablishment.InspectionEndDate);
				schoolPerformance.DateOfLatestSection8Inspection = TryParseDate(establishment.MISEstablishment.DateOfLatestSection8Inspection);
				schoolPerformance.OfstedReport = establishment.MISEstablishment.Weblink;
			}

			return schoolPerformance;
		}

		private static DateTime? TryParseDate(string date)
		{
			if (DateTime.TryParse(date, new CultureInfo("en-GB"), DateTimeStyles.None, out var result))
			{
				return result;
			}

			return null;
		}
	}
}