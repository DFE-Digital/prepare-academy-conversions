using ApplyToBecome.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	public class SchoolPerformanceService : EstablishmentServiceBase
	{
		public SchoolPerformanceService(IHttpClientFactory httpClientFactory, ILogger<SchoolPerformanceService> logger) : base(httpClientFactory, logger) { }

		public async Task<SchoolPerformance> GetSchoolPerformanceByUrn(string urn)
		{
			var establishment = await GetEstablishmentByUrn(urn);
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

			if (DateTime.TryParse(establishment.OfstedLastInspection, out var ofstedLastInspection))
				schoolPerformance.OfstedLastInspection = ofstedLastInspection;

			return schoolPerformance;
		}
	}
}