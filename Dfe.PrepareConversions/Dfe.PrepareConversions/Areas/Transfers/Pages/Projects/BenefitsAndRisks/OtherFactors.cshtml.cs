﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models.Projects;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Models.Benefits;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable UnusedMember.Global - properties bound using [BindProperty]

namespace Dfe.PrepareTransfers.Web.Pages.Projects.BenefitsAndRisks
{
    public class OtherFactors : CommonPageModel
    {
        private readonly IProjects _projects;

        public OtherFactors(IProjects projects)
        {
            _projects = projects;
        }

        [BindProperty] public OtherFactorsViewModel OtherFactorsViewModel { get; set; } = new OtherFactorsViewModel();

        public async Task<IActionResult> OnGetAsync()
        {
            var project = await _projects.GetByUrn(Urn);

            var projectResult = project.Result;
            IncomingTrustName = projectResult.IncomingTrustName;
            OtherFactorsViewModel.OtherFactorsVm = BuildOtherFactorsItemViewModel(projectResult.Benefits.OtherFactors);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var project = await _projects.GetByUrn(Urn);
            var projectResult = project.Result;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            projectResult.Benefits.OtherFactors = OtherFactorsViewModel.OtherFactorsVm
                .Where(of => of.Checked)
                .ToDictionary(d => d.OtherFactor, x => x.Description);
            await _projects.UpdateBenefits(projectResult);

            var available = new List<TransferBenefits.OtherFactor>

            {
                TransferBenefits.OtherFactor.HighProfile,
                TransferBenefits.OtherFactor.ComplexLandAndBuildingIssues,
                TransferBenefits.OtherFactor.FinanceAndDebtConcerns,
                TransferBenefits.OtherFactor.OtherRisks
            };
            return RedirectToPage(GetPage(available, projectResult.Benefits.OtherFactors), new {Urn, ReturnToPreview});
        }

        /// <summary>
        /// Get next or previous page for other factors dynamic navigation
        /// </summary>
        /// <param name="available">List of other factors to navigate to</param>
        /// <param name="otherFactors">Other factors currently selected</param>
        /// <param name="backLink">true if navigating backwards, back link</param>
        /// <returns>Page name</returns>
        public static string GetPage(List<TransferBenefits.OtherFactor> available,
            Dictionary<TransferBenefits.OtherFactor, string> otherFactors, bool backLink = false)
        {
            var foundOtherFactor =
                available.FirstOrDefault(otherFactor => otherFactors.Select(o => o.Key).Contains(otherFactor));

            string backLinkContent =
               backLink ? "/Projects/BenefitsAndRisks/OtherFactors" : "/Projects/BenefitsAndRisks/Index";

            var pageUrl = GetPageUrlFromOtherFactor(foundOtherFactor);

            return string.IsNullOrEmpty(pageUrl) ? backLinkContent : pageUrl;
        }

        public static string GetPageUrlFromOtherFactor(TransferBenefits.OtherFactor foundOtherFactor)
        {
            switch (foundOtherFactor)
            {
                case TransferBenefits.OtherFactor.HighProfile:
                {
                    return "/Projects/BenefitsAndRisks/HighProfileTransfer";
                }
                case TransferBenefits.OtherFactor.ComplexLandAndBuildingIssues:
                {
                    return "/Projects/BenefitsAndRisks/ComplexLandAndBuilding";
                }
                case TransferBenefits.OtherFactor.FinanceAndDebtConcerns:
                {
                    return "/Projects/BenefitsAndRisks/FinanceAndDebt";
                }
                case TransferBenefits.OtherFactor.OtherRisks:
                {
                    return "/Projects/BenefitsAndRisks/OtherRisks";
                }
            }

            return null;
        }

        public static List<OtherFactorsItemViewModel> BuildOtherFactorsItemViewModel(
            Dictionary<TransferBenefits.OtherFactor, string> otherFactorsToSet)
        {
            List<OtherFactorsItemViewModel> items = new List<OtherFactorsItemViewModel>();
            foreach (TransferBenefits.OtherFactor otherFactor in Enum.GetValues(typeof(TransferBenefits.OtherFactor)))
            {
                if (otherFactor != TransferBenefits.OtherFactor.Empty)
                {
                    var isChecked = otherFactorsToSet.ContainsKey(otherFactor);

                    items.Add(new OtherFactorsItemViewModel
                    {
                        Description = isChecked ? otherFactorsToSet[otherFactor] : null,
                        OtherFactor = otherFactor,
                        Checked = isChecked
                    });
                }
            }

            return items;
        }
    }
}