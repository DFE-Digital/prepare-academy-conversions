using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.ConfirmProjectDates;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository), IDateValidationMessageProvider
{
   private readonly ISignificantChangeProjectRepository _repository = repository;

   [BindProperty(Name = "proposed-decision-date", BinderType = typeof(DateInputModelBinder))]
   public DateTime? ProposedDecisionDate { get; set; }

   [BindProperty(Name = "proposed-change-date", BinderType = typeof(DateInputModelBinder))]
   public DateTime? ProposedChangeDate { get; set; }

   protected override string TaskTitle => "Confirm project dates";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      ProposedDecisionDate = Project.ProposedDecisionDate;
      ProposedChangeDate = Project.ProposedChangeDate;

      return Page();
   }

   public async Task<IActionResult> OnPostAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      if (!ModelState.IsValid)
      {
         errorService.AddErrors(ModelState.Keys, ModelState);
         return Page();
      }

      SetSignificantChangeProjectDatesCommand command = new(
         ProposedDecisionDate,
         ProposedChangeDate);

      await _repository.SetProjectDates(id, command);

      return RedirectToTaskList(id);
   }
   
   string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
   {
      return $"{displayName} must include a {string.Join(" and ", missingParts)}";
   }

   string IDateValidationMessageProvider.AllMissing(string displayName)
   {
      return $"Enter the {displayName}";
   }
}

