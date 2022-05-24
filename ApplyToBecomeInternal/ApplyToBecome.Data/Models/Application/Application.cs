using System;
using System.Collections.Generic;

namespace ApplyToBecome.Data.Models.Application
{
	public class Application
	{
		public string ApplicationId { get; set; }
		public string ApplicationType { get; set; }
		public string Name { get; set; }
		public string ApplicationLeadAuthorName { get; set; }
		public string TrustConsentEvidenceDocumentLink { get; set; }
		public bool? ChangesToTrust { get; set; }
		public string ChangesToTrustExplained { get; set; }
		public bool? ChangesToLaGovernance { get; set; }
		public string ChangesToLaGovernanceExplained { get; set; }
		public ICollection<ApplyingSchool> ApplyingSchools { get; set; }
	}
}