using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Extensions;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services;

public class UserRepository : IUserRepository
{
   private readonly IGraphUserService _graphUserService;

   public UserRepository(IGraphUserService graphUserService)
   {
      _graphUserService = graphUserService;
   }

   public async Task<IEnumerable<User>> GetAllUsers()
   {
      IEnumerable<Microsoft.Graph.User> users = await _graphUserService.GetAllUsers();

      return users
         .Select(u => new User(u.Id, u.Mail, $"{u.GivenName} {u.Surname.ToFirstUpper()}"));
   }
}
