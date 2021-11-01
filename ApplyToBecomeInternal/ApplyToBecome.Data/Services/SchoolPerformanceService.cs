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
				schoolPerformance.BehaviourAndAttitudes = establishment.MISEstablishment.BehaviourAndAttitudes;
				schoolPerformance.EarlyYearsProvision = establishment.MISEstablishment.EarlyYearsProvision;
				schoolPerformance.EffectivenessOfLeadershipAndManagement = establishment.MISEstablishment.EffectivenessOfLeadershipAndManagement;
				schoolPerformance.OverallEffectiveness = establishment.MISEstablishment.OverallEffectiveness;
				schoolPerformance.PersonalDevelopment = establishment.MISEstablishment.PersonalDevelopment;
				schoolPerformance.QualityOfEducation = establishment.MISEstablishment.QualityOfEducation;
				schoolPerformance.SixthFormProvision = establishment.MISEstablishment.SixthFormProvision;
			}

		if (DateTime.TryParse(establishment.OfstedLastInspection, new CultureInfo("en-GB"), DateTimeStyles.None, out var ofstedLastInspection))
				schoolPerformance.OfstedLastInspection = ofstedLastInspection;

			return schoolPerformance;
		}
	}
}