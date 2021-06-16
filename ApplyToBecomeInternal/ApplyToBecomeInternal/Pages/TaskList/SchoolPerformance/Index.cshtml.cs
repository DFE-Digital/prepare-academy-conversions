using System.Threading.Tasks;
using ApplyToBecome.Data.Services;
using Microsoft.AspNetCore.Mvc;
using SchoolPerformanceModel = ApplyToBecome.Data.Models.SchoolPerformance;

namespace ApplyToBecomeInternal.Pages.SchoolPerformance
{
	public class IndexModel : BaseAcademyConversionProjectPageModel
	{
		private readonly SchoolPerformanceService _schoolPerformanceService;

		public IndexModel(SchoolPerformanceService schoolPerformanceService, AcademyConversionProjectRepository repository) : base(repository)
		{
			_schoolPerformanceService = schoolPerformanceService;
		}

		public SchoolPerformanceModel SchoolPerformance { get; private set; }

		public override async Task<IActionResult> OnGetAsync(int id)
		{
			var result = await base.OnGetAsync(id);

			SchoolPerformance = await _schoolPerformanceService.GetSchoolPerformanceByUrn(Project.SchoolURN);

			return result;
		}
	}
}
