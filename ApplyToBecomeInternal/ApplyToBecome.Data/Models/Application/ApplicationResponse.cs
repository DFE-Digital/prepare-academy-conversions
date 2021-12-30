using System;
using System.Collections.Generic;
using System.Text;

namespace ApplyToBecome.Data.Models.Application
{
	//
	// Parts of the A2BExternal Application response
	//
	public class ApplicationResponse
	{
		public string FormTrustProposedNameOfTrust { get; set; }
		public string ApplicationLeadAuthorName { get; set; }
		public int? ChangesToTrust { get; set; }
//		public string ChangesToTrustExplained { get; set; } // needed CML?
		public int? ChangesToLaGovernance { get; set; }
	}
}
