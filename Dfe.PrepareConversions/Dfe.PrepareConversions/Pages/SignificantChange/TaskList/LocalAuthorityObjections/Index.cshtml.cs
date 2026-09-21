using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.LocalAuthorityObjections;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
{
   private readonly ISignificantChangeProjectRepository _repository = repository;
   private readonly ErrorService _errorService = errorService;

   [BindProperty]
   public bool? LocalAuthorityRaisedObjections { get; set; }

   [BindProperty]
   public string LocalAuthorityObjectionsFurtherInformation { get; set; }

   [BindProperty]
   public string LocalAuthorityObjectionsSupportingEvidenceLink { get; set; }

   protected override string TaskTitle => "Local authority objections";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      LocalAuthorityRaisedObjections = Project.LocalAuthorityRaisedObjections;
      LocalAuthorityObjectionsFurtherInformation = Project.LocalAuthorityObjectionsFurtherInformation;
      LocalAuthorityObjectionsSupportingEvidenceLink = Project.LocalAuthorityObjectionsSupportingEvidenceLink;

      return Page();
   }

   public async Task<IActionResult> OnPostAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      Validate();

      if (!ModelState.IsValid)
      {
         _errorService.AddErrors([nameof(LocalAuthorityRaisedObjections)], ModelState);
         return Page();
      }

      SetSignificantChangeLocalAuthorityObjectionsCommand command = new(
         LocalAuthorityRaisedObjections,
         LocalAuthorityRaisedObjections == true ? LocalAuthorityObjectionsFurtherInformation : null,
         string.IsNullOrWhiteSpace(LocalAuthorityObjectionsSupportingEvidenceLink)
            ? null
            : LocalAuthorityObjectionsSupportingEvidenceLink);

      await _repository.SetLocalAuthorityObjections(id, command);

      return RedirectToTaskList(id);
   }

   private void Validate()
   {
      if (!LocalAuthorityRaisedObjections.HasValue)
      {
         ModelState.AddModelError(nameof(LocalAuthorityRaisedObjections), "Select an option");
      }
   }
}
