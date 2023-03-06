using Dfe.PrepareConversions.ViewModels;

namespace Dfe.PrepareConversions.Extensions;

public static class ProjectListRowViewModelHelpers
{
   public static ProjectListRowViewModel Row(this ProjectListViewModel project, int index)
   {
      return new ProjectListRowViewModel { Item = project, Index = index };
   }
}