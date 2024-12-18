using Dfe.PrepareTransfers.Web.Models.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Dfe.PrepareTransfers.Data.Models.Projects;
using Dfe.PrepareTransfers.Web.Models.Forms;
using Dfe.PrepareTransfers.Helpers;
using System;

namespace Dfe.PrepareTransfers.Web.Models.Benefits
{
    public class BenefitsSummaryViewModel : CommonViewModel
    {
        private readonly IList<TransferBenefits.IntendedBenefit> _intendedBenefits;
        private readonly string _otherIntendedBenefit;
        public readonly IReadOnlyList<OtherFactorsItemViewModel> OtherFactorsItems;
        public readonly string OutgoingAcademyUrn;
        public readonly bool? AnyRisks;
        public readonly bool? EqualitiesImpactAssessmentConsidered;
        public readonly bool? IsReadOnly;
        public readonly DateTime? ProjectSentToCompleteDate;
        

        public BenefitsSummaryViewModel(IList<TransferBenefits.IntendedBenefit> intendedBenefits, 
            string otherIntendedBenefit, 
            IList<OtherFactorsItemViewModel> otherFactorsItems,
            string projectUrn,
            string outgoingAcademyUrn,
            bool? anyRisks = null,
            bool? equalitiesImpactAssessmentConsidered = null,
            bool? isReadOnly = null,
            DateTime? projectSentToCompleteDate = null)
        {
            _intendedBenefits = intendedBenefits;
            _otherIntendedBenefit = otherIntendedBenefit;
            OtherFactorsItems =
               new ReadOnlyCollection<OtherFactorsItemViewModel>(otherFactorsItems ??
                                                                 new List<OtherFactorsItemViewModel>());
            Urn = projectUrn;
            OutgoingAcademyUrn = outgoingAcademyUrn;
            AnyRisks = anyRisks;
            EqualitiesImpactAssessmentConsidered = equalitiesImpactAssessmentConsidered;
            IsReadOnly = isReadOnly;
            ProjectSentToCompleteDate = projectSentToCompleteDate;
        }

        public List<string> IntendedBenefitsSummary()
        {
           List<string> summary = _intendedBenefits.ToList()
              .FindAll(EnumHelpers<TransferBenefits.IntendedBenefit>.HasDisplayValue)
              .Select(EnumHelpers<TransferBenefits.IntendedBenefit>.GetDisplayValue)
              .ToList();

            if (_intendedBenefits.Contains(TransferBenefits.IntendedBenefit.Other))
            {
                summary.Add($"Other: {_otherIntendedBenefit}");
            }

            return summary;
        }
    }
}