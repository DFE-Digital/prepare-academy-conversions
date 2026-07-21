using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.ViewModels;

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
         Status = significantChangeProject.Status,
      };
   }
}