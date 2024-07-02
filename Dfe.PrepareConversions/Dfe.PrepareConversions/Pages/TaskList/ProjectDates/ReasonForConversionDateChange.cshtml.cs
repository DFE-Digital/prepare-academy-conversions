using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AcademyConversion;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.ProjectDates;

public class ReasonForConversionDateChangePageModel : BaseAcademyConversionProjectPageModel
{
   private readonly ErrorService _errorService;

   public IEnumerable<ConversionDateReasonForChangePast> PastReasonForChangeOptions { get; set; }
   public IEnumerable<ConversionDateReasonForChangeFuture> FutureReasonForChangeOptions { get; set; }

   [BindProperty]
   public IEnumerable<string> ChangedReasons { get; set; }

   [BindProperty]
   public string ChangeIncomingTrustReason { get; set; }

   [BindProperty]
   public string ChangeSchoolReason { get; set; }
   
   [BindProperty]
   public string ChangeLAReason { get; set; }
   
   [BindProperty]
   public string ChangeDioceseReason { get; set; }
   
   [BindProperty]
   public string ChangeTuPEReason { get; set; }
   
   [BindProperty]
   public string ChangePensionsReason { get; set; }
   
   [BindProperty]
   public string ChangeUnionReason { get; set; }
   
   [BindProperty]
   public string ChangeNegativePressCoverageReason { get; set; }
   
   [BindProperty]
   public string ChangeGovernanceReason { get; set; }
   
   [BindProperty]
   public string ChangeNotAppliFinancecableReason { get; set; }
   
   [BindProperty]
   public string ChangeFinanceReason { get; set; }

   [BindProperty]
   public string ChangeViabilityReason { get; set; }

   [BindProperty]
   public string ChangeLandReason { get; set; }
   
   [BindProperty]
   public string ChangeBuildingsReason { get; set; }
   
   [BindProperty]
   public string ChangeLegalDocumentsReason { get; set; }
   
   [BindProperty]
   public string ChangeCorrectingAnErrorReason { get; set; }
   
   [BindProperty]
   public string ChangeVoluntaryDeferralReason { get; set; }
   
   [BindProperty]
   public string ChangeInAFederationReason { get; set; }

   [BindProperty]
   public string ChangeProgressingFasterReason { get; set; }

   public UIHelpers UI => new(this);

   public bool ShowError => _errorService.HasErrors();
   public string ErrorMessage => _errorService.GetErrors().ToList().Last().Message;

   public DateTime ConversionDate { get; set; }
   public bool NewDateIsInThePast { get; set; }


   public ReasonForConversionDateChangePageModel(IAcademyConversionProjectRepository repository,
                       ErrorService errorService)
      : base(repository)
   {
      _errorService = errorService;
      PastReasonForChangeOptions = Enum.GetValues(typeof(ConversionDateReasonForChangePast)).Cast<ConversionDateReasonForChangePast>();
      FutureReasonForChangeOptions = Enum.GetValues(typeof(ConversionDateReasonForChangeFuture)).Cast<ConversionDateReasonForChangeFuture>();
   }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await base.OnGetAsync(id);

      ConversionDate = DateTime.Parse(Request.Query["conversionDate"]);

      var previousConversionDate = Project.ProposedConversionDate;

      if (previousConversionDate > ConversionDate)
      {
         NewDateIsInThePast = true;
      }

      return Page();
   }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      await OnGetAsync(id);

      if (!ChangedReasons.Any())
      {
         ModelState.AddModelError("ChangedReasonSet", "Select at least one reason");
      }

      EnsureExplanationIsProvidedFor(ConversionDateReasonForChangeFuture.IncomingTrust, ChangeIncomingTrustReason);
      EnsureExplanationIsProvidedFor(ConversionDateReasonForChangeFuture.School, ChangeSchoolReason);
      EnsureExplanationIsProvidedFor(ConversionDateReasonForChangeFuture.LA, ChangeLAReason);
      EnsureExplanationIsProvidedFor(ConversionDateReasonForChangeFuture.Diocese, ChangeDioceseReason);
      EnsureExplanationIsProvidedFor(ConversionDateReasonForChangeFuture.TuPE, ChangeTuPEReason);
      EnsureExplanationIsProvidedFor(ConversionDateReasonForChangeFuture.Pensions, ChangePensionsReason);
      EnsureExplanationIsProvidedFor(ConversionDateReasonForChangeFuture.Union, ChangeUnionReason);
      EnsureExplanationIsProvidedFor(ConversionDateReasonForChangeFuture.NegativePressCoverage, ChangeNegativePressCoverageReason);
      EnsureExplanationIsProvidedFor(ConversionDateReasonForChangeFuture.Governance, ChangeGovernanceReason);
      EnsureExplanationIsProvidedFor(ConversionDateReasonForChangeFuture.Finance, ChangeFinanceReason);
      EnsureExplanationIsProvidedFor(ConversionDateReasonForChangeFuture.Viability, ChangeViabilityReason);
      EnsureExplanationIsProvidedFor(ConversionDateReasonForChangeFuture.Land, ChangeLandReason);
      EnsureExplanationIsProvidedFor(ConversionDateReasonForChangeFuture.Buildings, ChangeBuildingsReason);
      EnsureExplanationIsProvidedFor(ConversionDateReasonForChangeFuture.LegalDocuments, ChangeLegalDocumentsReason);
      EnsureExplanationIsProvidedFor(ConversionDateReasonForChangeFuture.CorrectingAnError, ChangeCorrectingAnErrorReason);
      EnsureExplanationIsProvidedFor(ConversionDateReasonForChangeFuture.VoluntaryDeferral, ChangeVoluntaryDeferralReason);
      EnsureExplanationIsProvidedFor(ConversionDateReasonForChangeFuture.InAFederation, ChangeInAFederationReason);

      EnsureExplanationIsProvidedFor(ConversionDateReasonForChangePast.ProgressingFaster, ChangeProgressingFasterReason);

      if (_errorService.HasErrors())
      {
         return await OnGetAsync(id);
      }

      var projectDatesModel = new SetProjectDatesModel(id, Project.HeadTeacherBoardDate, Project.PreviousHeadTeacherBoardDate, ConversionDate, Project.ProjectDatesSectionComplete, User.Identity.Name, MapSelectedReasons());

      await _repository.SetProjectDates(id, projectDatesModel);

      return RedirectToPage(Links.ProjectDates.ConfirmProjectDates.Page, new { id });
   }

   private void EnsureExplanationIsProvidedFor(Enum reason, string explanation)
   {
      string reasonName = reason.ToString();

      if (ChangedReasons.Contains(reasonName) && string.IsNullOrWhiteSpace(explanation))
         ModelState.AddModelError(UI.ReasonFieldFor(reason), $"Enter a reason for selecting {reason.ToDescription()}");
   }

   private IEnumerable<ReasonChange> MapSelectedReasons()
   {
      if (NewDateIsInThePast)
      {
         return ChangedReasons
         .Select(reasonText => Enum.Parse<ConversionDateReasonForChangePast>(reasonText, true))
         .Select(reason => reason switch
         {
            ConversionDateReasonForChangePast.ProgressingFaster => new ReasonChange(reason.ToDescription(), ChangeProgressingFasterReason),
            ConversionDateReasonForChangePast.CorrectingAnError => new ReasonChange(reason.ToDescription(), ChangeCorrectingAnErrorReason),
            _ => throw new ArgumentOutOfRangeException(nameof(reason), reason, "Unexpected value for ReasonChange")
         });
      }
      else
      {
         return ChangedReasons
         .Select(reasonText => Enum.Parse<ConversionDateReasonForChangeFuture>(reasonText, true))
         .Select(reason => reason switch
         {
            ConversionDateReasonForChangeFuture.IncomingTrust => new ReasonChange(reason.ToDescription(), ChangeIncomingTrustReason),
            ConversionDateReasonForChangeFuture.School => new ReasonChange(reason.ToDescription(), ChangeSchoolReason),
            ConversionDateReasonForChangeFuture.LA => new ReasonChange(reason.ToDescription(), ChangeLAReason),
            ConversionDateReasonForChangeFuture.Diocese => new ReasonChange(reason.ToDescription(), ChangeDioceseReason),
            ConversionDateReasonForChangeFuture.TuPE => new ReasonChange(reason.ToDescription(), ChangeTuPEReason),
            ConversionDateReasonForChangeFuture.Pensions => new ReasonChange(reason.ToDescription(), ChangePensionsReason),
            ConversionDateReasonForChangeFuture.Union => new ReasonChange(reason.ToDescription(), ChangeUnionReason),
            ConversionDateReasonForChangeFuture.NegativePressCoverage => new ReasonChange(reason.ToDescription(), ChangeNegativePressCoverageReason),
            ConversionDateReasonForChangeFuture.Finance => new ReasonChange(reason.ToDescription(), ChangeNegativePressCoverageReason),
            ConversionDateReasonForChangeFuture.Governance => new ReasonChange(reason.ToDescription(), ChangeFinanceReason),
            ConversionDateReasonForChangeFuture.Viability => new ReasonChange(reason.ToDescription(), ChangeViabilityReason),
            ConversionDateReasonForChangeFuture.Land => new ReasonChange(reason.ToDescription(), ChangeLandReason),
            ConversionDateReasonForChangeFuture.Buildings => new ReasonChange(reason.ToDescription(), ChangeBuildingsReason),
            ConversionDateReasonForChangeFuture.LegalDocuments => new ReasonChange(reason.ToDescription(), ChangeLegalDocumentsReason),
            ConversionDateReasonForChangeFuture.CorrectingAnError => new ReasonChange(reason.ToDescription(), ChangeCorrectingAnErrorReason),
            ConversionDateReasonForChangeFuture.VoluntaryDeferral => new ReasonChange(reason.ToDescription(), ChangeVoluntaryDeferralReason),
            ConversionDateReasonForChangeFuture.InAFederation => new ReasonChange(reason.ToDescription(), ChangeInAFederationReason),
            _ => throw new ArgumentOutOfRangeException(nameof(reason), reason, "Unexpected value for ReasonChange")
         });
      }

   }

   public class UIHelpers
   {
      private readonly ReasonForConversionDateChangePageModel _model;

      public UIHelpers(ReasonForConversionDateChangePageModel model)
      {
         _model = model;
      }

      public string IdFor(string prefix, object item)
      {
         string connector = prefix.EndsWith('-') ? string.Empty : "-";
         string suffix = item?.ToString()?.ToLowerInvariant();

         return $"{prefix}{connector}{suffix}";
      }

      public string ValueFor(object item)
      {
         return item.ToString();
      }

      public string ReasonFieldFor(object item)
      {
         return $"Change{item}Reason";
      }
   }
}