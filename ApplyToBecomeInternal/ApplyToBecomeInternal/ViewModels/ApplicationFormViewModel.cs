using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using ApplyToBecomeInternal.Models.Navigation;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.ViewModels
{
	public class ApplicationFormViewModel
	{
		public ApplicationFormViewModel(Application application, ProjectViewModel project)
		{
			Project = project;
			SubMenu = new SubMenuViewModel(project.Id, SubMenuPage.ApplicationForm);
			Navigation = new NavigationViewModel(NavigationTarget.ProjectsList);
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

		public ProjectViewModel Project { get; }
		public SubMenuViewModel SubMenu { get; }
		public NavigationViewModel Navigation { get; }
		public IEnumerable<BaseFormSection> Sections { get; }
	}
}
