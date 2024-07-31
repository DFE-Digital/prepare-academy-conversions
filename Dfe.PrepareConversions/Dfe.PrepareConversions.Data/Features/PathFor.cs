using Microsoft.FeatureManagement;

namespace Dfe.PrepareConversions.Data.Features;

public class PathFor
{
   private readonly bool _useAcademisationApplication;

   public PathFor(IFeatureManager features)
   {
      _useAcademisationApplication = features.IsEnabledAsync(FeatureFlags.UseAcademisationApplication).Result;
   }
   public string GetApplicationByReference => _useAcademisationApplication ? "/application/{0}/applicationReference" : "/v2/apply-to-become/application/{0}";
   public static string GetAllProjects => "/legacy/projects";
   public static string DownloadProjectExport => "/export/export-projects";
   public static string GetProjectById => "/legacy/project/{0}";
   public static string GetFormAMatProjectById => "/conversion-project/formamatproject/{0}";
   public static string UpdateProject => "/legacy/project/{0}";

   public static string GetProjectsForGroup => "/conversion-project/projects-for-group/{0}";
   public static string GetFilterParameters => "/legacy/projects/status";
   public static string AddProjectNote => "/legacy/project/{0}/notes";
   public static string AddSchoolImprovementPlan => "/conversion-project/{0}/school-improvement-plans";
   public static string UpdateSchoolImprovementPlan => "/conversion-project/{0}/school-improvement-plans/{1}";
   public static string GetSchoolImprovementPlans => "/conversion-project/{0}/school-improvement-plans";
   public static string SetExternalApplicationForm => "/conversion-project/{0}/setExternalApplicationForm";
   public static string SetPerformanceData => "/conversion-project/{0}/SetPerformanceData";
   public static string SetIncomingTrust => "/conversion-project/{0}/SetIncomingTrust";
   public static string SetSchoolOverview => "/conversion-project/{0}/SetSchoolOverview";
   public static string SetAssignedUser => "/conversion-project/{0}/SetAssignedUser";
   public static string SetFormAMatAssignedUser => "/conversion-project/{0}/SetFormAMatAssignedUser";

   public static string GetAllProjectsV2 => "/conversion-project/projects";
   public static string GetFormAMatProjects => "/conversion-project/FormAMatProjects";
   public static string SearchFormAMatProjects => "/conversion-project/search-formamatprojects";
   public static string SetFormAMatProjectReference => "/conversion-project/{0}/SetFormAMatProjectReference";
   public static string SetProjectDates => "/conversion-project/{0}/SetProjectDates";
   public static string GetOpeningDateHistoryForConversionProject => "/conversion-project/{0}/conversion-date-history";
   public static string CreateNewProjectGroup => "/project-group/create-project-group";
   public static string GetProjectGroups => "/project-group/get-project-groups";
}
