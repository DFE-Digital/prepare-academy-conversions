using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services.AzureAd
{
	public class GraphUserService : IGraphUserService
	{
		private readonly GraphServiceClient _client;
		private readonly AzureAdOptions _azureAdOptions;

		public GraphUserService(IGraphClientFactory graphClientFactory, IOptions<AzureAdOptions> azureAdOptions)
		{
			_client = graphClientFactory.Create();
			_azureAdOptions = azureAdOptions.Value;
		}

		public async Task<IEnumerable<Microsoft.Graph.User>> GetAllUsers()
		{
			var users = new List<Microsoft.Graph.User>();

			var queryOptions = new List<QueryOption>()
			{
				new QueryOption("$count", "true"),
				new QueryOption("$top", "999")
			};

			var members = await _client.Groups[_azureAdOptions.GroupId.ToString()].Members
				.Request(queryOptions)
				.Header("ConsistencyLevel", "eventual")
				.Select("givenName,surname,id,mail")
				.GetAsync();

			users.AddRange(members.Cast<Microsoft.Graph.User>().ToList());

			return users;
		}
	}
}