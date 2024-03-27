using Dfe.PrepareConversions.Data.Models;
using System;

namespace Dfe.PrepareConversions.ViewModels;

public abstract class ProjectTypeBase
{
   protected abstract string TypeAndRouteValue { get; }

   public bool IsFormAMat { get; init; }

   public virtual bool IsExternalSchoolApplication { get; } = false;


   public bool IsSponsored => string.IsNullOrWhiteSpace(TypeAndRouteValue) is false &&
                              TypeAndRouteValue.Equals(AcademyTypeAndRoutes.Sponsored, StringComparison.InvariantCultureIgnoreCase);

   public bool IsVoluntary => string.IsNullOrWhiteSpace(TypeAndRouteValue) is false &&
                              TypeAndRouteValue.Equals(AcademyTypeAndRoutes.Voluntary, StringComparison.InvariantCultureIgnoreCase);
}
