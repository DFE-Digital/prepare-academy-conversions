using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Tests.Pages.ProjectAssignment
{
	public class TestUserRepository : IUserRepository
	{
		public Task<IEnumerable<User>> GetAllUsers()
		{
			var toReturn = new List<User>();			
			for (int i = 0; i < 30; i++)
				toReturn.Add(new User(Guid.NewGuid().ToString(), $"bob.{i}@education.gov.uk", $"Bob {i}"));

			return Task.FromResult(toReturn.AsEnumerable());
		}
	}
}
