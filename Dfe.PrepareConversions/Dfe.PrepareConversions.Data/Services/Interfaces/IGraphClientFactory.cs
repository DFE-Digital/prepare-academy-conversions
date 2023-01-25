using Microsoft.Graph;

namespace Dfe.PrepareConversions.Data.Services.Interfaces
{
	public interface IGraphClientFactory
	{
		public GraphServiceClient Create();
	}
}