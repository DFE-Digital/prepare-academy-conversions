using Dfe.PrepareConversions.ViewModels;

namespace Dfe.PrepareConversions.Extensions;

public static class ProjectListRowViewModelHelpers
{
   public static ProjectListRowViewModel Row(this ProjectListViewModel project, int index)
   {
      return new ProjectListRowViewModel { Item = project, Index = index };
   }

   public static FormAMatProjectListRowViewModel Row(this FormAMatProjectListViewModel project, int index)
   {
      return new FormAMatProjectListRowViewModel { Item = project, Index = index };
   }

   public static ProjectGroupListRowViewModel Row(this ProjectGroupListViewModel project, int index)
   {
      return new ProjectGroupListRowViewModel { Item = project, Index = index };
   }
}
