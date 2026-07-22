using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.ViewModels;
using System;

namespace Dfe.PrepareConversions.Utils;

public static class SignificantChangeProjectListHelper
{
   public static SignificantChangeProjectListViewModel Build(SignificantChangeProjectResponse significantChangeProject)
   {
      return new SignificantChangeProjectListViewModel
      {
         Id = significantChangeProject.Id,
         Urn = significantChangeProject.Urn,
         Tier = significantChangeProject.Tier,
         TrustName = significantChangeProject.TrustName,
         TrustUkprn = significantChangeProject.TrustUkprn,
         TypeOfSignificantChange = significantChangeProject.TypeOfSignificantChange,
         Status = MapProjectStatus(significantChangeProject.Status),
      };
   }

   public static string MapProjectStatus(string status)
   {
      if (string.IsNullOrWhiteSpace(status)) return "Pre decision";

      return status.Trim().ToLowerInvariant() switch
      {
         "approved" => "Approved",
         "deferred" => "Deferred",
         "declined" => "Declined",
         "daorevoked" => "DAO revoked",
         "dao revoked" => "DAO revoked",
         "withdrawn" => "Withdrawn",
         "approved with conditions" => "Approved with conditions",
         _ => "Pre decision"
      };
   }
}