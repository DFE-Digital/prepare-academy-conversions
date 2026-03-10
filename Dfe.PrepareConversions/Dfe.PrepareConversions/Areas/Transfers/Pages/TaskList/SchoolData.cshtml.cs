using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Web.Dfe.PrepareTransfers.Helpers;
using Dfe.PrepareTransfers.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Web.Pages.TaskList
{
    public class SchoolData(IAcademies academies, IProjects projects, IEducationPerformance projectRepositoryEducationPerformance)
       : CommonPageModel
    {
       [BindProperty(SupportsGet = true)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string AcademyUkprn {get; set; }
        public bool HasKeyStage2PerformanceInformation { get; set; }
        public bool HasKeyStage4PerformanceInformation { get; set; }
        public bool HasKeyStage5PerformanceInformation { get; set; }
        public string AcademyName { get; set; }
        public string AcademyUrn { get; set; }


        public async Task<PageResult> OnGetAsync()
        {
            var project = await projects.GetByUrn(Urn); 
            var academy = await academies.GetAcademyByUkprn(AcademyUkprn);
            AcademyName = academy.Name;
            AcademyUrn = academy.Urn;
            ProjectReference = project.Result.Reference;
            var educationPerformance =
                projectRepositoryEducationPerformance.GetByAcademyUrn(project.Result.OutgoingAcademyUrn).Result;
            HasKeyStage2PerformanceInformation =
                PerformanceDataHelpers.HasKeyStage2PerformanceInformation(educationPerformance.Result
                    .KeyStage2Performance);
            HasKeyStage4PerformanceInformation =
                PerformanceDataHelpers.HasKeyStage4PerformanceInformation(educationPerformance.Result
                    .KeyStage4Performance);
            HasKeyStage5PerformanceInformation =
                PerformanceDataHelpers.HasKeyStage5PerformanceInformation(educationPerformance.Result
                    .KeyStage5Performance);
            return Page();
        }
    }
}