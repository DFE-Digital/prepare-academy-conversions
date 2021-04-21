using ApplyToBecome.Data.Models;

namespace ApplyToBecome.Data
{
	public interface IApplications
	{
		Application GetApplication(string id);
	}
}