using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	public class UserRepository : IUserRepository
	{
		private readonly IGraphUserService _graphUserService;

		public UserRepository(IGraphUserService graphUserService)
		{
			_graphUserService = graphUserService;
		}
		
		public async Task<IEnumerable<User>> SearchUsers(string searchString)
		{
			var users = await _graphUserService.GetAllUsers();
			
			searchString = searchString.ToLower().Trim();

			return users
				.Where(u => u.GivenName.ToLower().Contains(searchString) || u.Surname.ToLower().Contains(searchString))
				.Select(u => new User(u.Id, u.Mail, u.GivenName, u.Surname));			
		}		
	}
}
