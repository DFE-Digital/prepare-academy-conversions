using GovUK.Dfe.CoreLibs.Contracts.ExternalApplications.Models.Response;
using GovUK.Dfe.PersonsApi.Client.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services.Person
{
    public class PersonApiEstablishmentsService : IPersonApiEstablishmentsService
    {
      private readonly IEstablishmentsClient _apiClient;
      private readonly ILogger<PersonApiEstablishmentsService> _logger;

      public PersonApiEstablishmentsService(IEstablishmentsClient apiClient, ILogger<PersonApiEstablishmentsService> logger)
      {
         _apiClient = apiClient;
         _logger = logger;
      }

      public async Task<Result<MemberOfParliament>> GetMemberOfParliamentBySchoolUrnAsync(int urn)
      {
         try
         {
            var result = await _apiClient.GetMemberOfParliamentBySchoolUrnAsync(urn, CancellationToken.None)
                .ConfigureAwait(false);

            return Result<MemberOfParliament>.Success(result);
         }
         catch (PersonsApiException ex)
         {
            var errorMessage = $"An error occurred with the Persons API client. Response: {ex.Message}";
            _logger.LogError(ex, "An error occurred with the Persons API client. Response: {Message}", ex.Message);
            return Result<MemberOfParliament>.Failure(errorMessage);
         }
         catch (AggregateException ex)
         {
            var errorMessage = "An error occurred.";
            _logger.LogError(ex, "An error occurred.");
            return Result<MemberOfParliament>.Failure(errorMessage);
         }
         catch (Exception ex) when (ex is not OperationCanceledException)
         {
            var errorMessage = $"An unexpected error occurred. Response: {ex.Message}";
            _logger.LogError(ex, "An unexpected error occurred. Response: {Message}", ex.Message);
            return Result<MemberOfParliament>.Failure(errorMessage);
         }
      }
   }
}
