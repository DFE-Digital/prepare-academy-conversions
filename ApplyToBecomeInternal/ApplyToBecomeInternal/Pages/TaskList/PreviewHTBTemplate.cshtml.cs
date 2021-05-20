using ApplyToBecome.Data;
using ApplyToBecomeInternal.Models.Navigation;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Pages.TaskList
{
	public class PreviewHTBTemplateModel : PageModel
    {
		private readonly IProjects _projects;

		public PreviewHTBTemplateModel(IProjects projects)
		{
			_projects = projects;
		}

		public ProjectViewModel Project { get; set; }
		public NavigationViewModel Navigation { get; set; }

		public void OnGet(int id)
        {
			var project = _projects.GetProjectById(id);
			Project = new ProjectViewModel(project);
			var templateData = new[] { new KeyValuePair<string, string>("id", Project.Id) };
			Navigation = new NavigationViewModel(NavigationTarget.TaskList, templateData);
		}
    }
}
