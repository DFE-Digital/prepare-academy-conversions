using ApplyToBecome.Data.Models;

namespace ApplyToBecome.Data.Mock
{
	public class MockTrusts:ITrusts
	{
		public Trust FindTrustByName(string searchInput)
		{
			return new Trust {Name = searchInput};
		}
	}
}