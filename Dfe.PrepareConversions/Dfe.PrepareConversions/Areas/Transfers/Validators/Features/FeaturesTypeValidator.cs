﻿using Dfe.PrepareTransfers.Web.Models.Features;
using Dfe.PrepareTransfers.Data.Models.Projects;
using FluentValidation;


namespace Dfe.PrepareTransfers.Web.Transfers.Validators.Features
{
    public class FeaturesTypeValidator : AbstractValidator<FeaturesTypeViewModel>
    {
        public FeaturesTypeValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            
            RuleFor(x => x.TypeOfTransfer)
                          .NotEqual(TransferFeatures.TransferTypes.Empty)
                          .WithMessage("Select the type of transfer");

            RuleFor(x => x.OtherType)
                .NotEmpty()
                .When(x => x.TypeOfTransfer == TransferFeatures.TransferTypes.Other)
                .WithMessage("Enter the type of transfer");
        }
    }
}