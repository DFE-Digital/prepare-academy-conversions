using ApplyToBecome.Data.Models;

namespace ApplyToBecome.Data.Mock
{
	public class MockApplications:IApplications
	{
		public Application GetApplication(string id)
		{
			return new Application{
				School = new School{
					Name = "St Wilfrid's Primary School"
				},
				Trust = new Trust{
					Name = "Dynamics Trust",
				},
				LeadApplicant = "Garth Brown",
				Details = new ApplicationDetails{
					EvidenceDocument = new Link{
						Name = "consent_dynamics.docx",
						Url = "#"
					},
					ChangesToGovernance = false,
					ChangesAtLocalLevel = true
				}
			};
		}
	}
}