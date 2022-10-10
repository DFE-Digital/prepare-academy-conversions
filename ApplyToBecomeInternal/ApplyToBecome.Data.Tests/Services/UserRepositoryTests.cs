using ApplyToBecome.Data.Extensions;
using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Services.Interfaces;
using ApplyToBecome.Data.Tests.AutoFixture;
using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.Graph;
using Moq;
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
		public async Task GetAllUsers_ReturnsUsers([Frozen] Mock<IGraphUserService> graphUserService,
			UserRepository sut)
		{
			var users = GenerateUsers(20);

			graphUserService.Setup(m => m.GetAllUsers()).ReturnsAsync(users);

			var result = (await sut.GetAllUsers()).ToList();

			Assert.Equivalent(users.Select(u => new Data.Models.User(u.Id, u.Mail, $"{u.GivenName} {u.Surname.ToFirstUpper()}")), result);
		}

		[Theory, AutoMoqData]
		public async Task SearchUsers_EmptySearch_ReturnsEmpty(UserRepository sut)
		{
			var result = (await sut.GetAllUsers());

			Assert.Empty(result);
		}

		private List<User> GenerateUsers(int count)
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
