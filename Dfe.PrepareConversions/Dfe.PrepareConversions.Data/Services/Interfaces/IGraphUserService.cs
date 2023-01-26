using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services.Interfaces
{
	public interface IGraphUserService
	{
		Task<IEnumerable<User>> GetAllUsers();
	}
}