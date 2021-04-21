using ApplyToBecome.Data.Models.Application;

namespace ApplyToBecome.Data
{
	public interface IApplications
	{
		Application GetApplication(string id);
	}
}