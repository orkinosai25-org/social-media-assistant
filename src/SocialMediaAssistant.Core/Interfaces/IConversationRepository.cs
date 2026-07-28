using SocialMediaAssistant.Core.Entities;

namespace SocialMediaAssistant.Core.Interfaces;

/// <summary>Repository interface for conversation persistence.</summary>
public interface IConversationRepository
{
    Task<Conversation?> GetByExternalIdAsync(string externalId, MessageChannel channel, CancellationToken cancellationToken = default);
    Task<Conversation> CreateAsync(Conversation conversation, CancellationToken cancellationToken = default);
    Task<Conversation> UpdateAsync(Conversation conversation, CancellationToken cancellationToken = default);
    Task<IEnumerable<Conversation>> GetBySellerIdAsync(Guid sellerId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
    Task AddMessageAsync(Message message, CancellationToken cancellationToken = default);
    Task<IEnumerable<Message>> GetRecentMessagesAsync(Guid conversationId, int count = 10, CancellationToken cancellationToken = default);
    Task<Conversation?> GetByIdAsync(Guid conversationId, CancellationToken cancellationToken = default);
}
