using ApplyToBecome.Data.Models;

namespace ApplyToBecome.Data.Mock
{
	public class MockApplications:IApplications
	{
		public Application GetApplication(string id)
		{
			return new Application();
		}
	}
}