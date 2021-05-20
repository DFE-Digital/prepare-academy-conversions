using System.Collections.Generic;
using ApplyToBecome.Data;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using ApplyToBecomeInternal.Models.Navigation;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplyToBecomeInternal.Pages.ApplicationForm
{
	public class IndexModel : PageModel
    {
		private readonly IApplications _applications;
		private readonly IProjects _projects;

		public IndexModel(IApplications applications, IProjects projects)
		{
			_applications = applications;
			_projects = projects;
		}

		public ProjectViewModel Project { get; set; }
		public SubMenuViewModel SubMenu { get; set; }
		public NavigationViewModel Navigation { get; set; }
		public IEnumerable<BaseFormSection> Sections { get; set; }

		public void OnGet(int id)
        {
			var project = _projects.GetProjectById(id);

			Project = new ProjectViewModel(project);
			SubMenu = new SubMenuViewModel(Project.Id, SubMenuPage.SchoolApplicationForm);
			Navigation = new NavigationViewModel(NavigationTarget.ProjectsList);

			var application = _applications.GetApplication(id.ToString());
			Sections = new BaseFormSection[]
			{
				new ApplicationFormSection(application),
				new AboutConversionSection(application),
				new FurtherInformationSection(application),
				new FinanceSection(application),
				new FuturePupilNumberSection(application),
				new LandAndBuildingsSection(application),
				new PreOpeningSupportGrantSection(application),
				new ConsultationSection(application),
				new DeclarationSection(application)
			};
		}
    }
}
