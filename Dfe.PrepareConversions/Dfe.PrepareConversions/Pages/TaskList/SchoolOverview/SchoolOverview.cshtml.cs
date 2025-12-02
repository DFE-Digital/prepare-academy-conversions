using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Person;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.SchoolOverview
{
    public class SchoolOverviewModel : UpdateAcademyConversionProjectPageModel
    {
      protected readonly IPersonApiEstablishmentsService _establishmentsService;

      public SchoolOverviewModel(
         IAcademyConversionProjectRepository repository,
         IPersonApiEstablishmentsService establishmentsService,
         ErrorService errorService
      ) : base(repository, errorService)
      {
         _establishmentsService = establishmentsService;
      }

      public override async Task<IActionResult> OnGetAsync(int id)
      {
         await base.OnGetAsync(id);

         var status = Project.ProjectStatus;

         if (status == "Pre advisory board" || status == "Deferred")
         {
            var result = await _establishmentsService.GetMemberOfParliamentBySchoolUrnAsync(Int32.Parse(Project.SchoolURN));

            if (result.IsSuccess)
            {
               UpdateAcademyConversionProject updatedProject = new()
               {
                  MemberOfParliamentNameAndParty = result.Value.DisplayName
               };

               await _repository.UpdateProject(id, updatedProject);

               return Page();
            }

            SetError("member-of-parliament-name-and-party", "The Member of Parliement name and Political party could not be retrieved");
         }

         return Page();
      }
   }
}
