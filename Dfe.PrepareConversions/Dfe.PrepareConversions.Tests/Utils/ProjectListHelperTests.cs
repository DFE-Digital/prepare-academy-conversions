﻿using Dfe.PrepareConversions.Tests.Pages;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Utils
{
   public class ProjectListHelperTests : BaseIntegrationTests
   {
      const string green = nameof(green);
      const string yellow = nameof(yellow);
      const string orange = nameof(orange);
      const string red = nameof(red);

      public ProjectListHelperTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
      {
      }

      [Fact]
      public void MapProjectString_Approved_ReturnsCorrectValues()
      {
         var actual = ProjectListHelper.MapProjectStatus("Approved");
         Assert.Equivalent(new ProjectStatus("APPROVED", green), actual);
      }

      [Fact]
      public void MapProjectString_Deferred_ReturnsCorrectValues()
      {
         var actual = ProjectListHelper.MapProjectStatus("Deferred");
         Assert.Equivalent(new ProjectStatus("DEFERRED", orange), actual);
      }

      [Fact]
      public void MapProjectString_Declined_ReturnsCorrectValues()
      {
         var actual = ProjectListHelper.MapProjectStatus("Declined");
         Assert.Equivalent(new ProjectStatus("DECLINED", red), actual);
      }

      [Fact]
      public void MapProjectString_OtherValue_ReturnsCorrectValues()
      {
         var actual = ProjectListHelper.MapProjectStatus("Hello!");
         Assert.Equivalent(new ProjectStatus("PRE ADVISORY BOARD", yellow), actual);
      }
   }
}