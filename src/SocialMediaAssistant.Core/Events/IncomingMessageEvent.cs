using SocialMediaAssistant.Core.Entities;

namespace SocialMediaAssistant.Core.Events;

/// <summary>Domain event raised when a new customer message arrives via webhook.</summary>
public record IncomingMessageEvent(
    string ExternalConversationId,
    string CustomerExternalId,
    string CustomerName,
    string MessageText,
    string ExternalMessageId,
    MessageChannel Channel,
    string SenderAccountId,
    DateTime ReceivedAt);
