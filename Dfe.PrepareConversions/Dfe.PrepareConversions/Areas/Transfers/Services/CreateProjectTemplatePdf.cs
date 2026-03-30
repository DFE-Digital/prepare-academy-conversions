using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Dfe.PrepareTransfers.Web.Services.Responses;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Web.Services
{
   public class CreateProjectTemplatePdf(IGetProjectTemplateModel getProjectTemplateModel) : ICreateProjectTemplatePdf
   {
      public async Task<CreateProjectTemplateResponse> Execute(string projectUrn)
      {
         var getHtbDocumentForProject = await getProjectTemplateModel.Execute(projectUrn);
         if (!getHtbDocumentForProject.IsValid)
         {
            return CreateErrorResponse(getHtbDocumentForProject.ResponseError);
         }

         var generator = new PdfDocumentGenerator();
         var document = generator.GenerateDocument(getHtbDocumentForProject.ProjectTemplateModel);

         return new CreateProjectTemplateResponse
         {
            Document = document
         };
      }

      private static CreateProjectTemplateResponse CreateErrorResponse(
          ServiceResponseError serviceResponseError)
      {
         if (serviceResponseError.ErrorCode == ErrorCode.NotFound)
         {
            return new CreateProjectTemplateResponse
            {
               ResponseError = new ServiceResponseError
               {
                  ErrorCode = ErrorCode.NotFound,
                  ErrorMessage = "Not found"
               }
            };
         }

         return new CreateProjectTemplateResponse
         {
            ResponseError = new ServiceResponseError
            {
               ErrorCode = ErrorCode.ApiError,
               ErrorMessage = "API has encountered an error"
            }
         };
      }
   }
}
