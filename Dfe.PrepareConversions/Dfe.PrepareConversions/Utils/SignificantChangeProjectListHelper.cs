using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.ViewModels;
using System;

namespace Dfe.PrepareConversions.Utils;

public static class SignificantChangeProjectListHelper
{
   public static SignificantChangeProjectViewBaseModel Build(SignificantChangeProjectResponse significantChangeProject)
   {
      return new SignificantChangeProjectViewBaseModel
      {
         Id = significantChangeProject.Id,
         Urn = significantChangeProject.Urn,
         SchoolName = significantChangeProject.SchoolName,
         Tier = significantChangeProject.Tier,
         TrustName = significantChangeProject.TrustName,
         TrustUkprn = significantChangeProject.TrustUkprn,
         AssignedUser = significantChangeProject.AssignedUser,
         TypeOfSignificantChange = significantChangeProject.TypeOfSignificantChange,
         Status = MapProjectStatus(significantChangeProject.Status),
         StatusColour = MapProjectStatusColour(significantChangeProject.Status)
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

   public static string MapProjectStatusColour(string status)
   {
      const string green = nameof(green);
      const string yellow = nameof(yellow);
      const string orange = nameof(orange);
      const string red = nameof(red);
      const string purple = nameof(purple);

      if (string.IsNullOrWhiteSpace(status)) return yellow;

      return status.Trim().ToLowerInvariant() switch
      {
         "approved" => green,
         "approved with conditions" => green,
         "deferred" => orange,
         "declined" => red,
         "daorevoked" => red,
         "dao revoked" => red,
         "withdrawn" => purple,
         _ => yellow
      };
   }
}