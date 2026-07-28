using SocialMediaAssistant.Core.Entities;

namespace SocialMediaAssistant.Shared.DTOs;

public record ConversationDto(
    Guid Id,
    string CustomerName,
    MessageChannel Channel,
    ConversationStatus Status,
    DateTime UpdatedAt,
    int MessageCount);

public record MessageDto(
    Guid Id,
    string Content,
    MessageRole Role,
    bool IsAiGenerated,
    DateTime SentAt);

public record ConversationDetailDto(
    ConversationDto Conversation,
    IEnumerable<MessageDto> Messages);
