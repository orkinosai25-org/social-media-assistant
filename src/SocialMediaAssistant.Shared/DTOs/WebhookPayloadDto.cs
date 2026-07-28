using System.Text.Json.Serialization;

namespace SocialMediaAssistant.Shared.DTOs;

public class MetaWebhookPayload
{
    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;

    [JsonPropertyName("entry")]
    public List<WebhookEntry> Entry { get; set; } = new();
}

public class WebhookEntry
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("time")]
    public long Time { get; set; }

    [JsonPropertyName("messaging")]
    public List<WebhookMessaging>? Messaging { get; set; }

    [JsonPropertyName("changes")]
    public List<WebhookChange>? Changes { get; set; }
}

public class WebhookMessaging
{
    [JsonPropertyName("sender")]
    public WebhookParticipant Sender { get; set; } = new();

    [JsonPropertyName("recipient")]
    public WebhookParticipant Recipient { get; set; } = new();

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }

    [JsonPropertyName("message")]
    public WebhookMessage? Message { get; set; }
}

public class WebhookParticipant
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}

public class WebhookMessage
{
    [JsonPropertyName("mid")]
    public string Mid { get; set; } = string.Empty;

    [JsonPropertyName("text")]
    public string? Text { get; set; }
}

public class WebhookChange
{
    [JsonPropertyName("value")]
    public WebhookChangeValue? Value { get; set; }

    [JsonPropertyName("field")]
    public string Field { get; set; } = string.Empty;
}

public class WebhookChangeValue
{
    [JsonPropertyName("messaging_product")]
    public string? MessagingProduct { get; set; }

    [JsonPropertyName("metadata")]
    public WhatsAppMetadata? Metadata { get; set; }

    [JsonPropertyName("messages")]
    public List<WhatsAppMessage>? Messages { get; set; }

    [JsonPropertyName("contacts")]
    public List<WhatsAppContact>? Contacts { get; set; }
}

public class WhatsAppMetadata
{
    [JsonPropertyName("display_phone_number")]
    public string? DisplayPhoneNumber { get; set; }

    [JsonPropertyName("phone_number_id")]
    public string? PhoneNumberId { get; set; }
}

public class WhatsAppMessage
{
    [JsonPropertyName("from")]
    public string From { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = string.Empty;

    [JsonPropertyName("text")]
    public WhatsAppText? Text { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
}

public class WhatsAppText
{
    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;
}

public class WhatsAppContact
{
    [JsonPropertyName("profile")]
    public WhatsAppProfile? Profile { get; set; }

    [JsonPropertyName("wa_id")]
    public string WaId { get; set; } = string.Empty;
}

public class WhatsAppProfile
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
