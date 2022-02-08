using System;
using System.Collections.Generic;

namespace ApplyToBecome.Data.Models.Application
{
	public class Application
	{
		public string ApplicationId { get; set; }
		public string TrustName { get; set; } // CML - application to join (trust name) - hopefully API will use application.account.accountId to get the name
											  //		public string FormTrustProposedNameOfTrust { get; set; } // CML - looks like this is for FAMs
		public string ApplicationLeadAuthorName { get; set; }
		public Link TrustConsentEvidenceDocument { get; set; }
		public bool ChangesToTrust { get; set; }
		public string ChangesToTrustExplained { get; set; }
		public bool ChangesToLaGovernance { get; set; }
		public string ChangesToLAGovernanceExplained { get; set; }
		public ApplyingSchool SchoolApplication { get; set; } // CML change to use a List now to allow for FAMs which will have >1 school?
	}
}