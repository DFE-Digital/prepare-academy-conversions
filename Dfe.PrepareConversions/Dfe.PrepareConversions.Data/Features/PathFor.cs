using Microsoft.FeatureManagement;

namespace Dfe.PrepareConversions.Data.Features;

public class PathFor
{
   private readonly bool _useAcademisationApplication;

   public PathFor(IFeatureManager features)
   {
      _useAcademisationApplication = features.IsEnabledAsync(FeatureFlags.UseAcademisationApplication).Result;
   }

   public static string GetSelectedRegions => "/establishment/regions?{0}";
   public static string GetProjectNotes => "/project-notes/{0}";
   public string GetApplicationByReference => _useAcademisationApplication ? "/application/{0}/applicationReference" : "/v2/apply-to-become/application/{0}";
   public string GetAllProjects => "/legacy/projects";
   public string GetProjectById => "/legacy/project/{0}";
   public string UpdateProject => "/legacy/project/{0}";
   public string GetFilterParameters => "/legacy/projects/status";
   public string AddProjectNote => "/legacy/project/{0}/notes";
}
