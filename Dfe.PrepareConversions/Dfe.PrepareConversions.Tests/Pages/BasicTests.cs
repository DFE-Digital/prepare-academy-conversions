﻿using FluentAssertions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages;

public class BasicTests : BaseIntegrationTests
{
   public BasicTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Theory]
   [InlineData("/project-list")]
   public async Task Should_be_success_result_on_get(string url)
   {
      AddGetStatuses();
      AddGetProjects();

      await OpenAndConfirmPathAsync(url);

      Document.StatusCode.Should().Be(HttpStatusCode.OK);
      Document.ContentType.Should().Be("text/html");
      Document.CharacterSet.Should().Be("utf-8");
   }
}
