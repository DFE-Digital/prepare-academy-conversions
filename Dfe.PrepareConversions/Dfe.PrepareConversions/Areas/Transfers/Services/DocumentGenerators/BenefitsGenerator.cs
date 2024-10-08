using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareTransfers.Data.Models.Projects;
using Dfe.PrepareConversions.DocumentGeneration;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using Dfe.PrepareTransfers.Helpers;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Models.ProjectTemplate;
using Dfe.PrepareTransfers.Web.Pages.Projects.BenefitsAndRisks;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace Dfe.PrepareTransfers.Web.Services.DocumentGenerators
{
    public static class BenefitsGenerator
    {
        public static void AddBenefits(DocumentBuilder builder, ProjectTemplateModel projectTemplateModel)
        {
            builder.ReplacePlaceholderWithContent("BenefitsInformation", build =>
            {
                build.AddHeading("Benefits", HeadingLevel.One);

                if (projectTemplateModel.TransferBenefits.IsNullOrEmpty())
                {
                    build.AddTable(new List<TextElement[]>
                    {
                        new[] { new TextElement { Value = "N/A" } }
                    });
                }

                else
                {
                    foreach (var item in projectTemplateModel.ListOfTransferBenefits)
                    {

                       var displayedOption = item == TransferBenefits.IntendedBenefit.Other ? $"Other: {projectTemplateModel.OtherIntendedBenefit}" : EnumHelpers<TransferBenefits.IntendedBenefit>.GetDisplayValue(item);

                        build.AddTable(new List<TextElement[]>
                        {
                          new[] { new TextElement { Value = displayedOption } }
                        });

                    }
                }
            });
        }
    }
}