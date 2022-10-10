using Microsoft.Graph;

namespace ApplyToBecome.Data.Services.Interfaces
{
	public interface IGraphClientFactory
	{
		public GraphServiceClient Create();
	}
}