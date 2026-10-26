using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.ViewModels;

namespace Dfe.PrepareConversions.Utils;

public static class SignificantChangeProjectListHelper
{
   public static SignificantChangeProjectViewBaseModel Build(SignificantChangeProjectSearchResponse significantChangeProject)
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
         LocalAuthorityName = significantChangeProject.LocalAuthorityName,
         RegionName = significantChangeProject.RegionName,
         Status = SignificantChangeProjectStatusHelper.MapProjectStatus(significantChangeProject.Status),
         StatusColour = SignificantChangeProjectStatusHelper.MapProjectStatusColour(significantChangeProject.Status),
      };
   }
}