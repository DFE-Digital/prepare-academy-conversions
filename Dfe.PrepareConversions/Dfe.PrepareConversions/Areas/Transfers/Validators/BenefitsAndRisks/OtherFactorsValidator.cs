﻿using System.Linq;
using FluentValidation;
using Dfe.PrepareTransfers.Web.Models.Benefits;

namespace Dfe.PrepareTransfers.Web.Transfers.Validators.BenefitsAndRisks
{
    public class OtherFactorsValidator : AbstractValidator<OtherFactorsViewModel>
    {
        public OtherFactorsValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.OtherFactorsVm)
                .Custom((list, context) =>
                {
                    if (!list.Any(o => o.Checked))
                    {
                        context.AddFailure("Select the risks with this transfer");
                    }
                });
        }
    }
}