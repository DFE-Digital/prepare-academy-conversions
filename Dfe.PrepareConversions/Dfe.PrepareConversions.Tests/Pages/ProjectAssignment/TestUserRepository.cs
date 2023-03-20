using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Tests.Pages.ProjectAssignment;

public class TestUserRepository : IUserRepository
{
   public Task<IEnumerable<User>> GetAllUsers()
   {
      List<User> toReturn = new();
      for (int i = 0; i < 30; i++)
         toReturn.Add(new User(Guid.NewGuid().ToString(), $"bob.{i}@education.gov.uk", $"Bob {i}"));

      return Task.FromResult(toReturn.AsEnumerable());
   }
}
