﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Models.Benefits;
using Dfe.PrepareTransfers.Web.Models.Forms;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.PrepareTransfers.Web.Pages.Projects.BenefitsAndRisks
{
    public class EqualitiesImpactAssessment : CommonPageModel
    {
        private readonly IProjects _projects;

        public EqualitiesImpactAssessment(IProjects projects)
        {
            _projects = projects;
        }

        [BindProperty] public EqualitiesImpactAssessmentViewModel EqualitiesImpactAssessmentViewModel { get; set; } = new EqualitiesImpactAssessmentViewModel();

        public IList<RadioButtonViewModel> RadioButtonsYesNo { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var project = await _projects.GetByUrn(Urn);
            IncomingTrustName = project.Result.IncomingTrustName;

            RadioButtonsYesNo = GetRadioButtons(project.Result.Benefits.EqualitiesImpactAssessmentConsidered);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var project = await _projects.GetByUrn(Urn);

            if (!ModelState.IsValid)
            {
                RadioButtonsYesNo = GetRadioButtons(project.Result.Benefits.EqualitiesImpactAssessmentConsidered);
                return Page();
            }

            project.Result.Benefits.EqualitiesImpactAssessmentConsidered = EqualitiesImpactAssessmentViewModel.EqualitiesImpactAssessmentConsidered;
            await _projects.UpdateBenefits(project.Result);

         if (ReturnToPreview)
         {
            return RedirectToPage(Links.HeadteacherBoard.Preview.PageName, new { Urn });
         }

         return RedirectToPage("/Projects/BenefitsAndRisks/Index", new {Urn});
        }

        private IList<RadioButtonViewModel> GetRadioButtons(bool? valueSelected)
        {
            var list = new List<RadioButtonViewModel>
            {
                new RadioButtonViewModel
                {
                    DisplayName = "Yes",
                    Name = $"{nameof(EqualitiesImpactAssessmentViewModel.EqualitiesImpactAssessmentConsidered)}",
                    Value = "true",
                    Checked = valueSelected is true
                },
                new RadioButtonViewModel
                {
                    DisplayName = "No",
                    Name = $"{nameof(EqualitiesImpactAssessmentViewModel.EqualitiesImpactAssessmentConsidered)}",
                    Value = "false",
                    Checked = valueSelected is false
                }
            };

            var selectedRadio =
                list.FirstOrDefault(c => c.Value == EqualitiesImpactAssessmentViewModel.EqualitiesImpactAssessmentConsidered.ToString());
            if (selectedRadio != null)
            {
                selectedRadio.Checked = true;
            }

            return list;
        }
    }
}