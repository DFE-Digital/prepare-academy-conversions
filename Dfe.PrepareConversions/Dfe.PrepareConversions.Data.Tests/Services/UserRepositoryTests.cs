using AutoFixture;
using AutoFixture.Xunit2;
using Dfe.PrepareConversions.Data.Extensions;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Data.Tests.AutoFixture;
using Microsoft.Graph;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Data.Tests.Services;

public class UserRepositoryTests
{
   private readonly Fixture _fixture = new();

   [Theory]
   [AutoMoqData]
   public async Task GetAllUsers_ReturnsUsers([Frozen] Mock<IGraphUserService> graphUserService,
                                              UserRepository sut)
   {
      List<User> users = GenerateUsers(20);

      graphUserService.Setup(m => m.GetAllUsers()).ReturnsAsync(users);

      List<Data.Models.User> result = (await sut.GetAllUsers()).ToList();

      Assert.Equivalent(users.Select(u => new Data.Models.User(u.Id, u.Mail, $"{u.GivenName} {u.Surname.ToFirstUpper()}")), result);
   }

   [Theory]
   [AutoMoqData]
   public async Task SearchUsers_EmptySearch_ReturnsEmpty(UserRepository sut)
   {
      IEnumerable<Data.Models.User> result = await sut.GetAllUsers();

      Assert.Empty(result);
   }

   private List<User> GenerateUsers(int count)
   {
      List<User> users = new();

      for (int i = 0; i < count; i++)
      {
         users.Add(
            new User { GivenName = _fixture.Create<string>(), Surname = _fixture.Create<string>(), Mail = _fixture.Create<string>(), Id = _fixture.Create<string>() }
         );
      }

      return users;
   }
}
