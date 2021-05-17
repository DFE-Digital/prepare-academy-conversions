using ApplyToBecomeInternal.Models.Navigation;

namespace ApplyToBecomeInternal.ViewModels
{
	public class GenerateHTBTemplateViewModel
	{
		public GenerateHTBTemplateViewModel(ProjectViewModel project)
		{
			Project = project;
			Navigation = new NavigationViewModel(NavigationTarget.PreviewHTBTemplate);
		}
		public ProjectViewModel Project { get; set; }
		public NavigationViewModel Navigation { get; set; }
	}
}
