using System.Threading.Tasks;
using ApplyToBecome.Data;
using ApplyToBecome.Data.Services;
using SchoolPerformanceModel = ApplyToBecome.Data.Models.SchoolPerformance;

namespace ApplyToBecomeInternal.Pages.SchoolPerformance
{
	public class IndexModel : BaseProjectPageModel
	{
		private readonly SchoolPerformanceService _schoolPerformanceService;

		public IndexModel(SchoolPerformanceService schoolPerformanceService, IProjects projects) : base(projects)
		{
			_schoolPerformanceService = schoolPerformanceService;
		}

		public SchoolPerformanceModel SchoolPerformance { get; private set; }

		public override async Task OnGetAsync(int id)
		{
			await base.OnGetAsync(id);

			SchoolPerformance = await _schoolPerformanceService.GetSchoolPerformanceByUrn(Project.SchoolURN);
		}
	}
}
