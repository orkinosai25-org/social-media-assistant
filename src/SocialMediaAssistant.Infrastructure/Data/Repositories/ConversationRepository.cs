using Microsoft.EntityFrameworkCore;
using SocialMediaAssistant.Core.Entities;
using SocialMediaAssistant.Core.Interfaces;

namespace SocialMediaAssistant.Infrastructure.Data.Repositories;

public class ConversationRepository : IConversationRepository
{
    private readonly AppDbContext _context;

    public ConversationRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Conversation?> GetByExternalIdAsync(string externalId, MessageChannel channel, CancellationToken cancellationToken = default)
        => _context.Conversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.ExternalConversationId == externalId && c.Channel == channel, cancellationToken);

    public Task<Conversation?> GetByIdAsync(Guid conversationId, CancellationToken cancellationToken = default)
        => _context.Conversations
            .Include(c => c.Messages.OrderBy(m => m.SentAt))
            .FirstOrDefaultAsync(c => c.Id == conversationId, cancellationToken);

    public async Task<Conversation> CreateAsync(Conversation conversation, CancellationToken cancellationToken = default)
    {
        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync(cancellationToken);
        return conversation;
    }

    public async Task<Conversation> UpdateAsync(Conversation conversation, CancellationToken cancellationToken = default)
    {
        _context.Conversations.Update(conversation);
        await _context.SaveChangesAsync(cancellationToken);
        return conversation;
    }

    public async Task<IEnumerable<Conversation>> GetBySellerIdAsync(Guid sellerId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        => await _context.Conversations
            .Include(c => c.Messages)
            .Where(c => c.SellerId == sellerId)
            .OrderByDescending(c => c.UpdatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public async Task AddMessageAsync(Message message, CancellationToken cancellationToken = default)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Message>> GetRecentMessagesAsync(Guid conversationId, int count = 10, CancellationToken cancellationToken = default)
    {
        var messages = await _context.Messages
            .Where(m => m.ConversationId == conversationId)
            .OrderByDescending(m => m.SentAt)
            .Take(count)
            .ToListAsync(cancellationToken);

        return messages.OrderBy(m => m.SentAt);
    }
}
