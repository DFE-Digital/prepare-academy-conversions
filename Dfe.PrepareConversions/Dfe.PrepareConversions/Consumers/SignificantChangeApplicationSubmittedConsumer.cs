using Dfe.Academisation.CorrelationIdMiddleware;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using GovUK.Dfe.FlexForms.Domain.Models.Messaging;
using MassTransit;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Consumers;

public class SignificantChangeApplicationSubmittedConsumer(
   ISignificantChangeProjectRepository significantChangeProjectRepository,
   ICorrelationContext correlationContext,
   ILogger<SignificantChangeApplicationSubmittedConsumer> logger) : IConsumer<SchemaEventEnvelope>
{
   private record PayloadValues(
      string ApplicationReference,
      string ApplicationId,
      int Urn,
      string TrustUkprn,
      byte Tier,
      string TypeOfSignificantChange);

   private const string ExpectedMessageType = "SignificantChangeApplicationSubmitted";
   private const string ExpectedTopicName = "significant-change-application-submitted";

   public async Task Consume(ConsumeContext<SchemaEventEnvelope> context)
   {
      SetCorrelationId(context, correlationContext);
      bool isInformationEnabled = logger.IsEnabled(LogLevel.Information);

      SchemaEventEnvelope message = context.Message;
      if (!ShouldProcess(message))
      {
         if (isInformationEnabled)
         {
            logger.LogInformation(
               "Skipping schema event with MessageType {MessageType} and TopicName {TopicName}",
               message?.MessageType,
               message?.TopicName);
         }

         return;
      }

      try
      {
         var resolvedPayload = ParsePayload(message.Payload);

         var command = new CreateSignificantProjectCommand(
            Urn: resolvedPayload.Urn,
            Tier: resolvedPayload.Tier,
            Route: resolvedPayload.TypeOfSignificantChange,
            TrustUkprn: resolvedPayload.TrustUkprn,
            ApplicationId: resolvedPayload.ApplicationId,
            ApplicationReference: resolvedPayload.ApplicationReference
         );

         await significantChangeProjectRepository.CreateProject(command);

         if (isInformationEnabled)
         {
            string applicationReference = message.Payload?.ApplicationReference ?? message.Metadata?.ApplicationReference;

            logger.LogInformation(
               "Created significant change project for application reference {ApplicationReference}, template {TemplateId}, urn {Urn}",
               applicationReference,
               message.Metadata?.TemplateId,
               resolvedPayload.Urn);
         }
      }
      catch (Exception ex)
      {
         logger.LogError(
            ex,
            "Error processing Significant Change Application Submitted schema event");
         throw new InvalidOperationException(
            $"Failed to process significant change schema event for application reference '{message?.Payload?.ApplicationReference ?? message?.Metadata?.ApplicationReference ?? "<unknown>"}'.",
            ex);
      }
   }

   private static PayloadValues ParsePayload(SchemaEventPayload payload)
   {
      if (payload == null) 
         throw new InvalidOperationException("Schema event payload is missing.");

      if (string.IsNullOrWhiteSpace(payload.ApplicationId))
         throw new InvalidOperationException("Schema event payload applicationId is missing.");
         
      if (string.IsNullOrWhiteSpace(payload.ApplicationReference))
         throw new InvalidOperationException("Schema event payload applicationReference is missing.");
      
      if (!int.TryParse(payload.Urn, out int urn))
         throw new InvalidOperationException("Schema event payload urn is missing or invalid.");

      if (string.IsNullOrWhiteSpace(payload.TrustUkprn))
         throw new InvalidOperationException("Schema event payload trustUkprn is missing.");
      
      if (!payload.Tier.HasValue)
         throw new InvalidOperationException("Schema event payload tier is missing.");
      
      if (payload.Tier.Value != 1 && payload.Tier.Value != 3)
         throw new InvalidOperationException("Schema event payload tier is invalid.");


      // Hardcode the type for now
      return new PayloadValues(
         ApplicationId: payload.ApplicationId,
         ApplicationReference: payload.ApplicationReference,
         Urn: urn,
         TrustUkprn: payload.TrustUkprn,
         Tier: payload.Tier.Value,
         TypeOfSignificantChange: "Hardcoded temporary value");
   }

   private static bool ShouldProcess(SchemaEventEnvelope message)
   {
      if (message is null)
      {
         return false;
      }

      bool messageTypeMatches = string.Equals(message.MessageType, ExpectedMessageType, StringComparison.OrdinalIgnoreCase);
      bool topicMatches = string.Equals(message.TopicName, ExpectedTopicName, StringComparison.OrdinalIgnoreCase);

      return messageTypeMatches || topicMatches;
   }

   private void SetCorrelationId(ConsumeContext context, ICorrelationContext correlationContext)
   {
      bool isInformationEnabled = logger.IsEnabled(LogLevel.Information);

      if (context.Headers.TryGetHeader("x-correlationId", out var headerValue) &&
          headerValue != null &&
          Guid.TryParse(headerValue.ToString(), out Guid correlationId))
      {
         if (isInformationEnabled)
         {
            logger.LogInformation(
               "Using correlation ID from message headers: {CorrelationId}",
               correlationId);
         }
      }
      else if (context.MessageId.HasValue)
      {
         correlationId = context.MessageId.Value;

         if (isInformationEnabled)
         {
            logger.LogInformation(
               "Using MassTransit MessageId as correlation ID: {CorrelationId}",
               correlationId);
         }
      }
      else
      {
         correlationId = Guid.NewGuid();
         logger.LogWarning(
            "No correlation ID found in message. Generated new correlation ID: {CorrelationId}",
            correlationId);
      }

      correlationContext.SetContext(correlationId);

      if (isInformationEnabled)
      {
         logger.LogInformation(
            "Correlation ID set for consumer processing: {CorrelationId}",
            correlationId);
      }
   }
}