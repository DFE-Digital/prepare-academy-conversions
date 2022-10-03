using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Services.Interfaces;
using ApplyToBecome.Data.Tests.AutoFixture;
using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.Graph;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecome.Data.Tests.Services
{
	public class UserRepositoryTests
	{
		private readonly Fixture _fixture = new Fixture();

		[Theory, AutoMoqData]
		public async Task SearchUsers_MatchesGivenName_ReturnsUsers([Frozen] Mock<IGraphUserService> graphUserService,
			UserRepository sut)
		{			
			var users = GenerateUsers(20);

			users.First().GivenName = "Penelope";
			users.Last().GivenName = "Peter";

			graphUserService.Setup(m => m.GetAllUsers()).ReturnsAsync(users);

			var result = (await sut.SearchUsers("Pe")).ToList();

			Assert.Multiple(
				// first user
				() => Assert.Equal(result[0].FirstName, users[0].GivenName),
				() => Assert.Equal(result[0].LastName, users[0].Surname),
				() => Assert.Equal(result[0].EmailAddress, users[0].Mail),
				() => Assert.Equal(result[0].Id, users[0].Id),
				// 2nd user
				() => Assert.Equal(result[1].FirstName, users.Last().GivenName),
				() => Assert.Equal(result[1].LastName, users.Last().Surname),
				() => Assert.Equal(result[1].EmailAddress, users.Last().Mail),
				() => Assert.Equal(result[1].Id, users.Last().Id)
			);
		}		

		[Theory, AutoMoqData]
		public async Task SearchUsers_MatchesSurname_ReturnsUsers([Frozen] Mock<IGraphUserService> graphUserService,
			UserRepository sut)
		{
			var users = GenerateUsers(20);

			users.First().GivenName = "Smith";
			users.Last().GivenName = "Smithson";

			graphUserService.Setup(m => m.GetAllUsers()).ReturnsAsync(users);

			var result = (await sut.SearchUsers("Smi")).ToList();

			Assert.Multiple(
				// first user
				() => Assert.Equal(result[0].FirstName, users[0].GivenName),
				() => Assert.Equal(result[0].LastName, users[0].Surname),
				() => Assert.Equal(result[0].EmailAddress, users[0].Mail),
				() => Assert.Equal(result[0].Id, users[0].Id),
				// 2nd user
				() => Assert.Equal(result[1].FirstName, users.Last().GivenName),
				() => Assert.Equal(result[1].LastName, users.Last().Surname),
				() => Assert.Equal(result[1].EmailAddress, users.Last().Mail),
				() => Assert.Equal(result[1].Id, users.Last().Id)
			);
		}

		[Theory, AutoMoqData]
		public async Task SearchUsers_MatchesSurnameAndGivenName_ReturnsBothUsers([Frozen] Mock<IGraphUserService> graphUserService,
			UserRepository sut)
		{
			var users = GenerateUsers(20);

			users.First().GivenName = "Penelope";
			users.Last().GivenName = "Penn";

			graphUserService.Setup(m => m.GetAllUsers()).ReturnsAsync(users);

			var result = (await sut.SearchUsers("Pen")).ToList();

			Assert.Multiple(
				// first user
				() => Assert.Equal(result[0].FirstName, users[0].GivenName),
				() => Assert.Equal(result[0].LastName, users[0].Surname),
				() => Assert.Equal(result[0].EmailAddress, users[0].Mail),
				() => Assert.Equal(result[0].Id, users[0].Id),
				// 2nd user
				() => Assert.Equal(result[1].FirstName, users.Last().GivenName),
				() => Assert.Equal(result[1].LastName, users.Last().Surname),
				() => Assert.Equal(result[1].EmailAddress, users.Last().Mail),
				() => Assert.Equal(result[1].Id, users.Last().Id)
			);
		}

		[Theory, AutoMoqData]
		public async Task SearchUsers_MatchesWhenCaseDiffers_ReturnsUsers([Frozen] Mock<IGraphUserService> graphUserService,
			UserRepository sut)
		{
			var users = GenerateUsers(20);

			users.First().GivenName = "Smith";			

			graphUserService.Setup(m => m.GetAllUsers()).ReturnsAsync(users);

			var result = (await sut.SearchUsers("smi")).ToList();

			Assert.Multiple(				
				// first user
				() => Assert.Equal(result[0].FirstName, users[0].GivenName),
				() => Assert.Equal(result[0].LastName, users[0].Surname),
				() => Assert.Equal(result[0].EmailAddress, users[0].Mail),
				() => Assert.Equal(result[0].Id, users[0].Id)	
			);
		}

		private List<User>  GenerateUsers(int count)
		{
			var users = new List<User>();

			for (int i = 0; i < count; i++)
			{
				users.Add(
					new User { GivenName = _fixture.Create<string>(), Surname = _fixture.Create<string>(), Mail = _fixture.Create<string>(), Id = _fixture.Create<string>() }
					);
			}

			return users;
		}
	}
}
