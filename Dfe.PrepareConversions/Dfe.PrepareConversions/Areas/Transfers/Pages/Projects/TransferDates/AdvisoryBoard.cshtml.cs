using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Web.Models.TransferDates;
using Dfe.PrepareTransfers.Web.Transfers.Validators.TransferDates;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Dfe.PrepareTransfers.Web.Pages.Projects.TransferDates
{
   public class AdvisoryBoard(IProjects projectsRepository) : CommonPageModel
   {
      [BindProperty] public AdvisoryBoardViewModel AdvisoryBoardViewModel { get; set; }
      public string TrustName { get; set; }
      public string Returns { get; set; }

      public async Task<IActionResult> OnGetAsync(string returns = null)
      {
         var project = await projectsRepository.GetByUrn(Urn);
         Returns = returns ?? "/Projects/TransferDates/Index";
         var projectResult = project.Result;
         TrustName = projectResult.OutgoingTrustName;
         AdvisoryBoardViewModel = new AdvisoryBoardViewModel
         {
            AdvisoryBoardDate = new DateViewModel
            {
               Date = DateViewModel.SplitDateIntoDayMonthYear(projectResult.Dates.Htb),
               UnknownDate = projectResult.Dates.HasHtbDate is false
            }
         };
         IncomingTrustName = projectResult.IncomingTrustName;
         return Page();
      }

      public async Task<IActionResult> OnPostAsync(string returns = null)
      {
         var project = await projectsRepository.GetByUrn(Urn);

         if (!ModelState.IsValid)
         {
            return Page();
         }

         var projectResult = project.Result;

         var validationContext = new ValidationContext<AdvisoryBoardViewModel>(AdvisoryBoardViewModel)
         {
            RootContextData =
                {
                    ["TargetDate"] = projectResult.Dates.Target
                }
         };
         var validator = new AdvisoryBoardDateValidator();
         var validationResult = await validator.ValidateAsync(validationContext);

         if (!validationResult.IsValid)
         {
            validationResult.AddToModelState(ModelState, nameof(AdvisoryBoardViewModel));
            return Page();
         }

         projectResult.Dates.Htb = AdvisoryBoardViewModel.AdvisoryBoardDate.DateInputAsString();
         projectResult.Dates.HasHtbDate = !AdvisoryBoardViewModel.AdvisoryBoardDate.UnknownDate;

         await projectsRepository.UpdateDates(projectResult);

         if (ReturnToPreview)
         {
            return RedirectToPage(Links.HeadteacherBoard.Preview.PageName, new { Urn });
         }

         Returns = returns ?? "/Projects/TransferDates/Index";

         return RedirectToPage(Returns, new { Urn });
      }

      private (string, string) GetReturnPageAndFragment()
      {
         Request.Query.TryGetValue("return", out StringValues returnQuery);
         Request.Query.TryGetValue("fragment", out StringValues fragmentQuery);
         return (returnQuery, fragmentQuery);
      }
   }
}