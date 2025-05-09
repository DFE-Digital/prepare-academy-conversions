﻿using System.Linq;
using System.Threading.Tasks;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.PrepareTransfers.Web.Pages.TaskList.HtbDocument
{
   public class Download(ICreateProjectTemplate createProjectTemplate,
       IGetInformationForProject getInformationForProject) : CommonPageModel
   {
      public string FileName { get; private set; }

      public async Task<IActionResult> OnGetAsync()
      {
         var project = await GetProject();

         ProjectReference = project.Reference;
         FileName = GenerateFormattedFileName(project);

         return Page();
      }

      public async Task<IActionResult> OnGetGenerateDocumentAsync()
      {
         var project = await GetProject();
         FileName = GenerateFormattedFileName(project);

         var document = await createProjectTemplate.Execute(Urn);

         return File(document.Document.ToArray(),
             "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
             $"{FileName}.docx");
      }

      private async Task<Project> GetProject()
      {
         var projectInformation = await getInformationForProject.Execute(Urn);
         return projectInformation.Project;
      }

      private static string GenerateFormattedFileName(Project project)
      {
         var formattedOutgoingTrustName = FormatTrustName(project.OutgoingTrustName);
         var formattedIncomingTrustName = FormatTrustName(project.IncomingTrustName);

         return $"{project.Reference}_{formattedOutgoingTrustName}_{formattedIncomingTrustName}_project-template";
      }

      private static string FormatTrustName(string trustName)
      {
         return trustName?.ToTitleCase().ToHyphenated().RemoveNonAlphanumericOrWhiteSpace();
      }
   }
}