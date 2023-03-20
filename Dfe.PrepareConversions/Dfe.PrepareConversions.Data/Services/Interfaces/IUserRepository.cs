using Dfe.PrepareConversions.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services.Interfaces;

public interface IUserRepository
{
   Task<IEnumerable<User>> GetAllUsers();
}
