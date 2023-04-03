using System;

namespace Dfe.PrepareConversions.ViewModels;

public abstract class ProjectTypeBase
{
   protected abstract string TypeAndRouteValue { get; }

   public bool IsSponsored => string.IsNullOrWhiteSpace(TypeAndRouteValue) is false &&
                              TypeAndRouteValue.Equals("Sponsored", StringComparison.InvariantCultureIgnoreCase);

   public bool IsVoluntary => string.IsNullOrWhiteSpace(TypeAndRouteValue) is false &&
                              TypeAndRouteValue.Equals("Converter", StringComparison.InvariantCultureIgnoreCase);

   public bool IsFormAMat => string.IsNullOrWhiteSpace(TypeAndRouteValue) is false &&
                             TypeAndRouteValue.Equals("Form a MAT", StringComparison.InvariantCultureIgnoreCase);
}
