using ApplyToBecome.Data.Models;

namespace ApplyToBecome.Data
{
	public interface ITrusts
	{
		Trust FindTrustByName(string searchInput);
	}
}