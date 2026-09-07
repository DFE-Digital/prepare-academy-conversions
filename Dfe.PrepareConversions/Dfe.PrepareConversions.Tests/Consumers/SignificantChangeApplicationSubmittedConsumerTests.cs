using Dfe.Academisation.CorrelationIdMiddleware;
using Dfe.PrepareConversions.Consumers;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using GovUK.Dfe.FlexForms.Domain.Models.Messaging;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Consumers;

public class SignificantChangeApplicationSubmittedConsumerTests
{
   [Fact]
   public async Task Consume_creates_significant_change_project_from_payload()
   {
      var repository = new Mock<ISignificantChangeProjectRepository>();
      repository
         .Setup(x => x.CreateProject(It.IsAny<CreateSignificantProjectCommand>()))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.Created, BuildProjectResponse()));

      var correlationContext = new Mock<ICorrelationContext>();
      var logger = new Mock<ILogger<SignificantChangeApplicationSubmittedConsumer>>();

      var sut = new SignificantChangeApplicationSubmittedConsumer(repository.Object, correlationContext.Object, logger.Object);
      var context = BuildContext(new SchemaEventEnvelope
      {
         MessageType = "SignificantChangeApplicationSubmitted",
         TopicName = "significant-change-application-submitted",
         Payload = new SchemaEventPayload
         {
            ApplicationReference = "SC123",
            Urn = "123456",
            TrustUkprn = "10001234",
            Tier = 2
         },
         Metadata = new SchemaEventMetadata
         {
            ApplicationReference = "SC123",
            TemplateId = "template-a"
         }
      });

      await sut.Consume(context.Object);

      repository.Verify(x => x.CreateProject(
         It.Is<CreateSignificantProjectCommand>(command =>
            command.Urn == 123456 &&
            command.TrustUkprn == "10001234" &&
            command.Tier == 2 &&
            command.TypeOfSignificantChange == "Hardcoded temporary value")), Times.Once);
      correlationContext.Verify(x => x.SetContext(It.IsAny<Guid>()), Times.Once);
   }

   [Fact]
   public async Task Consume_uses_payload_tier_and_route_when_present()
   {
      var repository = new Mock<ISignificantChangeProjectRepository>();
      repository
         .Setup(x => x.CreateProject(It.IsAny<CreateSignificantProjectCommand>()))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.Created, BuildProjectResponse()));

      var correlationContext = new Mock<ICorrelationContext>();
      var logger = new Mock<ILogger<SignificantChangeApplicationSubmittedConsumer>>();

      var sut = new SignificantChangeApplicationSubmittedConsumer(repository.Object, correlationContext.Object, logger.Object);
      var context = BuildContext(new SchemaEventEnvelope
      {
         MessageType = "SignificantChangeApplicationSubmitted",
         TopicName = "significant-change-application-submitted",
         Payload = new SchemaEventPayload
         {
            ApplicationReference = "SC124",
            Urn = "654321",
            TrustUkprn = "10009999",
            Tier = 1,
            TypeOfSignificantChange = "TypeOfSignificantChange A"
         },
         Metadata = new SchemaEventMetadata
         {
            TemplateId = "unknown-template"
         }
      });

      await sut.Consume(context.Object);

      repository.Verify(x => x.CreateProject(
         It.Is<CreateSignificantProjectCommand>(command =>
            command.Urn == 654321 &&
            command.TrustUkprn == "10009999" &&
            command.Tier == 1 &&
            command.TypeOfSignificantChange == "Hardcoded temporary value")), Times.Once);
   }

   [Fact]
   public async Task Consume_throws_when_tier_missing()
   {
      var repository = new Mock<ISignificantChangeProjectRepository>();
      repository
         .Setup(x => x.CreateProject(It.IsAny<CreateSignificantProjectCommand>()))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(HttpStatusCode.Created, BuildProjectResponse()));

      var correlationContext = new Mock<ICorrelationContext>();
      var logger = new Mock<ILogger<SignificantChangeApplicationSubmittedConsumer>>();

      var sut = new SignificantChangeApplicationSubmittedConsumer(repository.Object, correlationContext.Object, logger.Object);
      var context = BuildContext(new SchemaEventEnvelope
      {
         MessageType = "SignificantChangeApplicationSubmitted",
         TopicName = "significant-change-application-submitted",
         Payload = new SchemaEventPayload
         {
            ApplicationReference = "SIG-20260916-047",
            Urn = "100010",
            TrustUkprn = "10061000"
         },
         Metadata = new SchemaEventMetadata
         {
            TemplateId = "f12e2d96-c4bf-4135-919c-f04f4f1a7449"
         }
      });

      await Assert.ThrowsAsync<InvalidOperationException>(() => sut.Consume(context.Object));

      repository.Verify(x => x.CreateProject(It.IsAny<CreateSignificantProjectCommand>()), Times.Never);
   }

   [Fact]
   public async Task Consume_does_nothing_for_unrelated_message_type()
   {
      var repository = new Mock<ISignificantChangeProjectRepository>();
      var correlationContext = new Mock<ICorrelationContext>();
      var logger = new Mock<ILogger<SignificantChangeApplicationSubmittedConsumer>>();

      var sut = new SignificantChangeApplicationSubmittedConsumer(repository.Object, correlationContext.Object, logger.Object);
      var context = BuildContext(new SchemaEventEnvelope
      {
         MessageType = "SomethingElse",
         TopicName = "another-topic",
         Payload = new SchemaEventPayload(),
         Metadata = new SchemaEventMetadata()
      });

      await sut.Consume(context.Object);

      repository.Verify(x => x.CreateProject(It.IsAny<CreateSignificantProjectCommand>()), Times.Never);
   }

   private static SignificantChangeProjectResponse BuildProjectResponse()
   {
      return new SignificantChangeProjectResponse
      {
         Id = 1,
         Urn = 123456,
         Tier = 1,
         SchoolName = "School",
         TrustName = "Trust",
         TrustUkprn = "10001234",
         TypeOfSignificantChange = "TypeOfSignificantChange A",
         Status = "Pre decision"
      };
   }

   private static Mock<ConsumeContext<SchemaEventEnvelope>> BuildContext(SchemaEventEnvelope message)
   {
      var headers = new Mock<Headers>();
      object emptyValue = null;
      headers
         .Setup(x => x.TryGetHeader(It.IsAny<string>(), out emptyValue))
         .Returns(false);

      var context = new Mock<ConsumeContext<SchemaEventEnvelope>>();
      context.SetupGet(x => x.Message).Returns(message);
      context.SetupGet(x => x.Headers).Returns(headers.Object);
      context.SetupGet(x => x.MessageId).Returns(Guid.NewGuid());

      return context;
   }
}
