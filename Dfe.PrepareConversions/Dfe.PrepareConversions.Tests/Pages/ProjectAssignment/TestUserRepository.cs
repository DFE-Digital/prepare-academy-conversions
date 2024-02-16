using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Tests.Pages.ProjectAssignment;

public class TestUserRepository : IUserRepository
{
   private List<User> users = new();
   
   public Task<IEnumerable<User>> GetAllUsers()
   {
      if (users.Count == 0)
      {
         for (int i = 0; i < 30; i++)
            users.Add(new User(Guid.NewGuid().ToString(), $"bob.{i}@education.gov.uk", $"Bob {i}"));
      }
      return Task.FromResult(users.AsEnumerable());
   }
}
