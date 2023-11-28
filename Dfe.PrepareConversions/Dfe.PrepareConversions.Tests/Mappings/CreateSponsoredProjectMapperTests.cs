using AutoFixture;
using Dfe.Academies.Contracts.V4.Establishments;
using Dfe.Academies.Contracts.V4.Trusts;
using Dfe.PrepareConversions.Mappings;
using FluentAssertions;
using FluentAssertions.Execution;
using System;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Mappings;

public class CreateSponsoredProjectMapperTests
{
   [Fact]
   public void Should_Map_Establishment_With_PfiScheme_And_Trust_Into_Dto_CreateSponsoredProject()
   {
      // Arrange
      var fixture = new AutoFixture.Fixture();

      EstablishmentDto establishment = new()
      {
         Name = fixture.Create<string>(),
         Urn = Guid.NewGuid().ToString(),
         Pfi = "some-text",
         LocalAuthorityName = fixture.Create<string>(),
         Gor = new NameAndCodeDto { Name = fixture.Create<string>() }
      };

      TrustDto trust = new()
      {
         Ukprn = fixture.Create<string>(),
         ReferenceNumber = fixture.Create<string>(),
         Name = fixture.Create<string>(),
         CompaniesHouseNumber = fixture.Create<string>()
      };

      // Act
      var result = CreateProjectMapper.MapToDto(establishment, trust);

      // Assert
      result.School.PartOfPfiScheme.Should().BeTrue();
      result.School.Region.Should().BeEquivalentTo(establishment.Gor.Name);
      result.Trust.Should().NotBeNull().And.BeEquivalentTo(trust, cfg => cfg.ExcludingMissingMembers());
      result.School.Should().NotBeNull().And.BeEquivalentTo(establishment, cfg => cfg.ExcludingMissingMembers());


   }

   [Theory]
   [InlineData(default)]
   [InlineData("")]
   [InlineData("No")]
   [InlineData("NO")]
   [InlineData("no")]
   [InlineData("nO")]
   public void Should_Map_Establishment_Without_PfiScheme_And_Trust_Into_Dto_CreateSponsoredProject(string pfiScheme)
   {
      // Arrange
      var fixture = new AutoFixture.Fixture();

      EstablishmentDto establishment = new()
      {
         Name = fixture.Create<string>(),
         Urn = Guid.NewGuid().ToString(),
         Pfi = pfiScheme,
         LocalAuthorityName = fixture.Create<string>(),
         Gor = new NameAndCodeDto() { Name = fixture.Create<string>() }
      };

      TrustDto trust = new()
      {
         Ukprn = fixture.Create<string>(),
         ReferenceNumber = fixture.Create<string>(),
         Name = fixture.Create<string>(),
         CompaniesHouseNumber = fixture.Create<string>()
      };

      // Act
      var result = CreateProjectMapper.MapToDto(establishment, trust);

      // Assert
      using (var scope = new AssertionScope())
      {
         scope.AddReportable("pfiScheme", $"Using PFI Scheme value: {pfiScheme ?? "null"}");
         result.School.PartOfPfiScheme.Should().BeFalse();
         result.School.Region.Should().BeEquivalentTo(establishment.Gor.Name);
         result.Trust.Should().NotBeNull().And.BeEquivalentTo(trust, cfg => cfg.ExcludingMissingMembers());
         result.School.Should().NotBeNull().And.BeEquivalentTo(establishment, cfg => cfg.ExcludingMissingMembers());
      }
   }
}