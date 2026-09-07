using System.Text.Json.Serialization;

namespace GovUK.Dfe.FlexForms.Domain.Models.Messaging;

public class SchemaEventEnvelope
{
   [JsonPropertyName("messageType")]
   public string MessageType { get; set; }

   [JsonPropertyName("version")]
   public string Version { get; set; }

   [JsonPropertyName("topicName")]
   public string TopicName { get; set; }

   [JsonPropertyName("payload")]
   public SchemaEventPayload Payload { get; set; } = new();

   [JsonPropertyName("metadata")]
   public SchemaEventMetadata Metadata { get; set; } = new();
}

public class SchemaEventPayload
{
   [JsonPropertyName("urn")]
   public string Urn { get; set; }

   [JsonPropertyName("applicationId")]
   public string ApplicationId { get; set; }

   [JsonPropertyName("applicationReference")]
   public string ApplicationReference { get; set; }

   [JsonPropertyName("tier")]
   public byte? Tier { get; set; }

   [JsonPropertyName("typeOfSignificantChange")]
   public string TypeOfSignificantChange { get; set; }

   [JsonPropertyName("trustUkprn")]
   public string TrustUkprn { get; set; }
}

public class SchemaEventMetadata
{
   [JsonPropertyName("applicationId")]
   public string ApplicationId { get; set; }

   [JsonPropertyName("applicationReference")]
   public string ApplicationReference { get; set; }

   [JsonPropertyName("templateId")]
   public string TemplateId { get; set; }
}