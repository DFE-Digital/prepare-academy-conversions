using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Pages;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Xunit;
using AutoFixture;
using System.Linq;
using Dfe.Academisation.ExtensionMethods;

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

      [Theory]
      [InlineData("APPROVED WITH CONDITIONS", "Approved with Conditions", green)]
      [InlineData("approved with conditions", "Approved with Conditions", green)]
      [InlineData("Approved with Conditions", "Approved with Conditions", green)]
      public void MapProjectString_ApprovedWithConditions_ReturnsCorrectValues(string inputStatus, string expectedStatus, string expectedColour)
      {
         var actual = ProjectListHelper.MapProjectStatus(inputStatus);
         Assert.Equivalent(new ProjectStatus(expectedStatus, expectedColour), actual);
      }

      [Fact]
      public void ConvertToFirstLast_Returns_Empty_String_When_Name_Is_Empty()
      {
         var actual = ProjectListHelper.ConvertToFirstLast("");
         Assert.Equivalent(string.Empty, actual);
      }

      [Fact]
      public void ConvertToFirstLast_Returns_Name_When_Name_Is_One_Word()
      {
         var name = "Joe";
         var actual = ProjectListHelper.ConvertToFirstLast(name);
         Assert.Equivalent(name, actual);
      }

      [Fact]
      public void ConvertToFirstLast_Returns_Name_With_Space_When_Name_Is_Two_Words()
      {
         var name = "Bloggs,Joe";
         var actual = ProjectListHelper.ConvertToFirstLast(name);
         Assert.Equivalent("Joe Bloggs", actual);
      }

      [Fact]
      public void Build_FormAMatProjectListViewModel_Returns_FormAMatProjectListViewModel()
      {
         var fixture = new Fixture();
         FormAMatProject formAMATProject = fixture.Create<FormAMatProject>();

         FormAMatProjectListViewModel actual = ProjectListHelper.Build(formAMATProject);

         var firstProject = formAMATProject.Projects.First();

         Assert.Equivalent(formAMATProject.Id.ToString(), actual.Id);
         Assert.Equal(formAMATProject.ProposedTrustName, actual.TrustName);
         Assert.Equal(formAMATProject.ApplicationReference, actual.ApplciationReference);
         Assert.Equal(firstProject.Id, actual.FirstProjectId);
         Assert.Equal(firstProject.AssignedUser.FullName, actual.AssignedTo);
         Assert.Equal(string.Join(", ", formAMATProject.Projects.Select(x => x.LocalAuthority).Distinct()), actual.LocalAuthorities);
         Assert.Equal(firstProject.HeadTeacherBoardDate.ToDateString(), actual.AdvisoryBoardDate);
         Assert.Equivalent(formAMATProject.Projects.Select(x => x.ProjectStatus).Select(ProjectListHelper.MapProjectStatus).ToList(), actual.Status);
         Assert.Equal(string.Join(", ", formAMATProject.Projects.Select(x => x.SchoolName).Distinct()), actual.SchoolNames);
         Assert.Equal(string.Join(", ", formAMATProject.Projects.Select(x => x.Region).Distinct()), actual.Regions);
      }

      [Fact]
      public void Build_ProjectGroupListViewModel_Returns_ProjectGroupListViewModel()
      {
         var fixture = new Fixture();
         ProjectGroup projectGroup = fixture.Create<ProjectGroup>();

         ProjectGroupListViewModel actual = ProjectListHelper.Build(projectGroup);

         var firstProject = projectGroup.Projects.First();

         Assert.Equivalent(projectGroup.Id.ToString(), actual.Id);
         Assert.Equal(projectGroup.TrustName, actual.TrustName);
         Assert.Equal(projectGroup.TrustReferenceNumber, actual.TrustReference);
         Assert.Equal(projectGroup.TrustUkprn, actual.TrustUkprn);
         Assert.Equal(projectGroup.ReferenceNumber, actual.GroupReference);
         Assert.Equal(firstProject.AssignedUser?.FullName, actual.AssignedTo);
         Assert.Equal(string.Join(", ", projectGroup.Projects.Select(x => x.LocalAuthority).Distinct()), actual.LocalAuthorities);
         Assert.Equal(string.Join(", ", projectGroup.Projects.Select(x => x.SchoolName).Distinct()), actual.SchoolNames);
         Assert.Equal(string.Join(", ", projectGroup.Projects.Select(x => x.Region).Distinct()), actual.Regions);
         Assert.Equivalent(projectGroup.Projects.Select(x => x.ProjectStatus).Select(ProjectListHelper.MapProjectStatus).ToList(), actual.Status);
      }
   }
}
