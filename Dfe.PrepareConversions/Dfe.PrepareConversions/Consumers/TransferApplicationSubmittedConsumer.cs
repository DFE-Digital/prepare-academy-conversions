using Dfe.Academisation.CorrelationIdMiddleware;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using GovUK.Dfe.CoreLibs.Messaging.Contracts.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;
using TransferringAcademiesPrepare = Dfe.PrepareTransfers.Data.Models.Projects.TransferringAcademy;
namespace Dfe.PrepareConversions.Consumers
{
   public class TransferApplicationSubmittedConsumer(
      IProjects projectsRepository,
      ICorrelationContext correlationContext,
      ILogger<TransferApplicationSubmittedConsumer> logger) : IConsumer<TransferApplicationSubmittedEvent>
   {
      public async Task Consume(ConsumeContext<TransferApplicationSubmittedEvent> context)
      {
         SetCorrelationId(context, correlationContext);

         var applicationSubmittedEvent = context.Message;
         try
         {
            var project = new Project
            {
               Reference = applicationSubmittedEvent.ApplicationReference,
               OutgoingTrustUkprn = applicationSubmittedEvent.OutgoingTrustUkprn,
               OutgoingTrustName = applicationSubmittedEvent.OutgoingTrustName,
               TransferringAcademies = applicationSubmittedEvent.TransferringAcademies.Select(x => new TransferringAcademiesPrepare
               {
                  OutgoingAcademyUkprn = x.OutgoingAcademyUkprn,
                  IncomingTrustUkprn = x.IncomingTrustUkprn,
                  IncomingTrustName = x.IncomingTrustName,
                  Region = x.Region,
                  LocalAuthority = x.LocalAuthority,
                  OutgoingAcademyName = x.OutgoingAcademyName,
               }).ToList(),
               IsFormAMat = false
            };

            _ = await projectsRepository.Create(project);

         }
         catch (Exception ex)
         {
            logger.LogError(
               ex,
               "Error processing Application Submitted Event from EAT Transfers");
            throw; // Let MassTransit handle retries
         }
      }

      private void SetCorrelationId(ConsumeContext context, ICorrelationContext correlationContext)
      {
         Guid correlationId;

         if (context.Headers.TryGetHeader("x-correlationId", out var headerValue) &&
             headerValue != null &&
             Guid.TryParse(headerValue.ToString(), out correlationId))
         {
            logger.LogInformation(
               "Using correlation ID from message headers: {CorrelationId}",
               correlationId);
         }
         else if (context.MessageId.HasValue)
         {
            correlationId = context.MessageId.Value;
            logger.LogInformation(
               "Using MassTransit MessageId as correlation ID: {CorrelationId}",
               correlationId);
         }
         else
         {
            correlationId = Guid.NewGuid();
            logger.LogWarning(
               "No correlation ID found in message. Generated new correlation ID: {CorrelationId}",
               correlationId);
         }

         correlationContext.SetContext(correlationId);

         logger.LogInformation(
            "Correlation ID set for consumer processing: {CorrelationId}",
            correlationId);
      }
   }
}