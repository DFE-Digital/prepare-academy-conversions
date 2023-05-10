using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.ViewModels;
using FluentAssertions;
using System;
using System.Globalization;
using Xunit;

namespace Dfe.PrepareConversions.Tests.ViewModels;

public class ProjectListViewModelTests
{
   [Fact]
   public void Should_show_head_teacher_board_date_when_the_value_is_present()
   {
      new ProjectListViewModel { HeadTeacherBoardDate = DateTime.Today.ToString(CultureInfo.InvariantCulture) }
         .ShowHtbDate.Should().BeTrue();

      new ProjectListViewModel { HeadTeacherBoardDate = default }
         .ShowHtbDate.Should().BeFalse();
   }

   [Fact]
   public void Should_show_the_proposed_board_date_when_the_value_is_present()
   {
      new ProjectListViewModel { ProposedAcademyOpeningDate = DateTime.Today.ToString(CultureInfo.InvariantCulture) }
         .ShowProposedOpeningDate.Should().BeTrue();

      new ProjectListViewModel { ProposedAcademyOpeningDate = default }
         .ShowProposedOpeningDate.Should().BeFalse();
   }

   [Fact]
   public void Should_be_considered_sponsored_if_the_route_is_sponsored()
   {
      new ProjectListViewModel { TypeAndRoute = AcademyTypeAndRoutes.Sponsored }
         .IsSponsored.Should().BeTrue();

      new ProjectListViewModel { ApplicationReceivedDate = DateTime.Today.ToString(CultureInfo.InvariantCulture) }
         .IsSponsored.Should().BeFalse();
   }

   [Fact]
   public void Should_be_considered_a_form_a_mat_project_if_the_route_is_form_a_met()
   {
      new ProjectListViewModel { TypeAndRoute = AcademyTypeAndRoutes.FormAMat }
         .IsFormAMat.Should().BeTrue();

      new ProjectListViewModel { TypeAndRoute = "Anything else" }
         .IsFormAMat.Should().BeFalse();
   }

   [Fact]
   public void Should_be_considered_a_voluntary_conversion_if_the_project_is_neither_sponsored_nor_form_a_mat()
   {
      new ProjectListViewModel { TypeAndRoute = AcademyTypeAndRoutes.Voluntary, ApplicationReceivedDate = DateTime.Today.ToString(CultureInfo.InvariantCulture) }
         .IsVoluntary.Should().BeTrue();
   }
}
