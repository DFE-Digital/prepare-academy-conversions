using System.Collections.Generic;
using System.Threading.Tasks;
using ApplyToBecome.Data;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models.ApplicationForm;
using ApplyToBecomeInternal.Models.ApplicationForm.Sections;
using ApplyToBecomeInternal.ViewModels;

namespace ApplyToBecomeInternal.Pages.ApplicationForm
{
	public class IndexModel : BaseProjectPageModel
	{
		private readonly IApplications _applications;

		public IndexModel(IApplications applications, AcademyConversionProjectRepository repository) : base(repository)
		{
			_applications = applications;
		}

		public SubMenuViewModel SubMenu { get; set; }
		public IEnumerable<BaseFormSection> Sections { get; set; }

		public override async Task OnGetAsync(int id)
        {
			await base.OnGetAsync(id);

			SubMenu = new SubMenuViewModel(Project.Id, SubMenuPage.SchoolApplicationForm);

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
