﻿using Microsoft.FeatureManagement;

namespace ApplyToBecome.Data.Features;

public class PathFor
{
   private readonly bool _useAcademisation;

   public PathFor(IFeatureManager features)
   {
      _useAcademisation = features.IsEnabledAsync(FeatureFlags.UseAcademisation).Result;
   }

   public static string GetSelectedRegions => "/establishment/regions?{0}";

   public string GetAllProjects => _useAcademisation ? "/legacy/projects" : "/v2/conversion-projects";
   public string GetProjectById => _useAcademisation ? "/legacy/project/{0}" : "/conversion-projects/{0}";
   public string UpdateProject => _useAcademisation ? "/legacy/project/{0}" : "/conversion-projects/{0}";
   public string GetFilterParameters => _useAcademisation ? "/legacy/projects/status" : "/v2/conversion-projects/parameters";
   public string GetApplicationByReference => _useAcademisation ? "/application/{0}" : "/v2/apply-to-become/application/{0}";
}