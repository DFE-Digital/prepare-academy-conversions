using ApplyToBecome.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services.Interfaces
{
	public interface IUserRepository
	{
		Task<IEnumerable<User>> GetAllUsers();
	}
}
