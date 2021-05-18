using ApplyToBecomeInternal.Models.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.ViewModels
{
	public class PreviewHTBTemplateViewModel
	{
		public PreviewHTBTemplateViewModel(ProjectViewModel project)
		{
			Project = project;
			Navigation = new NavigationViewModel(NavigationTarget.TaskList);
		}	
		public ProjectViewModel Project { get; set; }
		public NavigationViewModel Navigation { get; set; }

	}
}
