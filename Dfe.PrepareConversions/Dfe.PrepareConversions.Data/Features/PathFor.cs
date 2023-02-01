using Microsoft.FeatureManagement;

namespace Dfe.PrepareConversions.Data.Features;

public class PathFor : ISetAcademisationFlag
{
   private bool _useAcademisation;

   public PathFor(IFeatureManager features)
   {
      _useAcademisation = features.IsEnabledAsync(FeatureFlags.UseAcademisation).Result;
   }
   public PathFor()
   {
   }

   public static string GetSelectedRegions => "/establishment/regions?{0}";
   public static string GetProjectNotes => "/project-notes/{0}";
   public string GetApplicationByReference => _useAcademisation ? "/application/{0}/applicationReference" :"/v2/apply-to-become/application/{0}";
   public string GetAllProjects => _useAcademisation ? "/legacy/projects" : "/v2/conversion-projects";
   public string GetProjectById => _useAcademisation ? "/legacy/project/{0}" : "/conversion-projects/{0}";
   public string UpdateProject => _useAcademisation ? "/legacy/project/{0}" : "/conversion-projects/{0}";
   public string GetFilterParameters => _useAcademisation ? "/legacy/projects/status" : "/v2/conversion-projects/parameters";
   public string AddProjectNote => _useAcademisation ? "/legacy/project/{0}/notes" : "/project-notes/{0}";
   bool ISetAcademisationFlag.UseAcademisation
   {
      get => _useAcademisation;
      set => _useAcademisation = value;
   }
}