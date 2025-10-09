using AutoFixture;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Pages;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Utils;

public class ProjectListHelperTests : BaseIntegrationTests
{
   private const string green = nameof(green);
   private const string yellow = nameof(yellow);
   private const string orange = nameof(orange);
   private const string red = nameof(red);

   public ProjectListHelperTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   [Fact]
   public void MapProjectString_Approved_ReturnsCorrectValues()
   {
      ProjectStatus actual = ProjectListHelper.MapProjectStatus("Approved");
      Assert.Equivalent(new ProjectStatus("Approved", green), actual);
   }

   [Fact]
   public void MapProjectString_Deferred_ReturnsCorrectValues()
   {
      ProjectStatus actual = ProjectListHelper.MapProjectStatus("Deferred");
      Assert.Equivalent(new ProjectStatus("Deferred", orange), actual);
   }

   [Fact]
   public void MapProjectString_Declined_ReturnsCorrectValues()
   {
      ProjectStatus actual = ProjectListHelper.MapProjectStatus("Declined");
      Assert.Equivalent(new ProjectStatus("Declined", red), actual);
   }

   [Fact]
   public void MapProjectString_OtherValue_ReturnsCorrectValues()
   {
      ProjectStatus actual = ProjectListHelper.MapProjectStatus("Hello!");
      Assert.Equivalent(new ProjectStatus("Pre advisory board", yellow), actual);
   }

   [Theory]
   [InlineData("APPROVED WITH CONDITIONS", "Approved with conditions", green)]
   [InlineData("approved with conditions", "Approved with conditions", green)]
   [InlineData("Approved with Conditions", "Approved with conditions", green)]
   public void MapProjectString_ApprovedWithConditions_ReturnsCorrectValues(string inputStatus, string expectedStatus, string expectedColour)
   {
      ProjectStatus actual = ProjectListHelper.MapProjectStatus(inputStatus);
      Assert.Equivalent(new ProjectStatus(expectedStatus, expectedColour), actual);
   }

   [Fact]
   public void ConvertToFirstLast_Returns_Empty_String_When_Name_Is_Empty()
   {
      string actual = ProjectListHelper.ConvertToFirstLast("");
      Assert.Equivalent(string.Empty, actual);
   }

   [Fact]
   public void ConvertToFirstLast_Returns_Name_When_Name_Is_One_Word()
   {
      string name = "Joe";
      string actual = ProjectListHelper.ConvertToFirstLast(name);
      Assert.Equivalent(name, actual);
   }

   [Fact]
   public void ConvertToFirstLast_Returns_Name_With_Space_When_Name_Is_Two_Words()
   {
      string name = "Bloggs,Joe";
      string actual = ProjectListHelper.ConvertToFirstLast(name);
      Assert.Equivalent("Joe Bloggs", actual);
   }

   [Fact]
   public void Build_FormAMatProjectListViewModel_Returns_FormAMatProjectListViewModel()
   {
      Fixture fixture = new();
      FormAMatProject formAMATProject = fixture.Create<FormAMatProject>();

      FormAMatProjectListViewModel actual = ProjectListHelper.Build(formAMATProject);

      AcademyConversionProject firstProject = formAMATProject.Projects.First();

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
      Fixture fixture = new();
      ProjectGroup projectGroup = fixture.Create<ProjectGroup>();

      ProjectGroupListViewModel actual = ProjectListHelper.Build(projectGroup);

      AcademyConversionProject firstProject = projectGroup.Projects.First();

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

   [Fact]
   public void MapPerformanceDataHint_With_School_Type_Pupil_Referral()
   {
      string actual = ProjectListHelper.MapPerformanceDataHint("pupil referral unit", false);

      Assert.Equal(
         "Your document will automatically include some Ofsted inspection data. Educational performance data isn't published for pupil referral units.\r\n\r\nAsk the pupil referral unit to share their educational performance and absence data with you. You can add that to the document once you have created it.",
         actual);
   }

   [Fact]
   public void MapPerformanceDataHint_With_School_Absence()
   {
      string actual = ProjectListHelper.MapPerformanceDataHint("", true);

      Assert.Equal("Only educational attendance information will be added to your project template.", actual);
   }

   [Fact]
   public void MapPerformanceDataHint_With_No_School_Absence()
   {
      string actual = ProjectListHelper.MapPerformanceDataHint("", false);

      Assert.Equal("This information will not be added to your project.", actual);
   }
}