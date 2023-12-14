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
   public static string UpdateProject => "/legacy/project/{0}";
   public static string GetFilterParameters => "/legacy/projects/status";
   public static string AddProjectNote => "/legacy/project/{0}/notes";
   public static string SetExternalApplicationForm => "/conversion-project/{0}/setExternalApplicationForm";
}
