using Microsoft.Extensions.Logging;
using SocialMediaAssistant.Core.Entities;
using SocialMediaAssistant.Core.Events;
using SocialMediaAssistant.Core.Interfaces;

namespace SocialMediaAssistant.Core.Services;

/// <summary>
/// Orchestrates the full message processing pipeline:
/// receive → load context → AI reply → send → persist.
/// </summary>
public class MessageProcessingService
{
    private readonly IConversationRepository _conversationRepository;
    private readonly ISellerRepository _sellerRepository;
    private readonly IStockService _stockService;
    private readonly IAIReplyService _aiReplyService;
    private readonly IEnumerable<IMessageSender> _messageSenders;
    private readonly ILogger<MessageProcessingService> _logger;

    public MessageProcessingService(
        IConversationRepository conversationRepository,
        ISellerRepository sellerRepository,
        IStockService stockService,
        IAIReplyService aiReplyService,
        IEnumerable<IMessageSender> messageSenders,
        ILogger<MessageProcessingService> logger)
    {
        _conversationRepository = conversationRepository;
        _sellerRepository = sellerRepository;
        _stockService = stockService;
        _aiReplyService = aiReplyService;
        _messageSenders = messageSenders;
        _logger = logger;
    }

    public async Task ProcessAsync(IncomingMessageEvent @event, Guid sellerId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing message from {Customer} on {Channel}", @event.CustomerName, @event.Channel);

        var seller = await _sellerRepository.GetByIdAsync(sellerId, cancellationToken);
        if (seller is null || !seller.IsActive)
        {
            _logger.LogWarning("Seller {SellerId} not found or inactive", sellerId);
            return;
        }

        var conversation = await _conversationRepository.GetByExternalIdAsync(
            @event.ExternalConversationId,
            @event.Channel,
            cancellationToken);

        if (conversation is null)
        {
            conversation = await _conversationRepository.CreateAsync(new Conversation
            {
                SellerId = sellerId,
                ExternalConversationId = @event.ExternalConversationId,
                CustomerName = @event.CustomerName,
                CustomerExternalId = @event.CustomerExternalId,
                Channel = @event.Channel
            }, cancellationToken);
        }

        var incomingMessage = new Message
        {
            ConversationId = conversation.Id,
            Content = @event.MessageText,
            Role = MessageRole.Customer,
            IsAiGenerated = false,
            SentAt = @event.ReceivedAt,
            ExternalMessageId = @event.ExternalMessageId
        };
        await _conversationRepository.AddMessageAsync(incomingMessage, cancellationToken);

        var history = await _conversationRepository.GetRecentMessagesAsync(conversation.Id, 10, cancellationToken);
        var stockContext = await _stockService.GetStockSummaryAsync(sellerId, cancellationToken);

        string aiReply;
        try
        {
            aiReply = await _aiReplyService.GenerateReplyAsync(
                @event.MessageText,
                history,
                stockContext,
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI reply generation failed for conversation {ConversationId}", conversation.Id);
            conversation.Status = ConversationStatus.NeedsReview;
            conversation.UpdatedAt = DateTime.UtcNow;
            await _conversationRepository.UpdateAsync(conversation, cancellationToken);
            return;
        }

        var sender = _messageSenders.FirstOrDefault(s => s.Channel == @event.Channel);
        if (sender is null)
        {
            _logger.LogWarning("No message sender configured for channel {Channel}", @event.Channel);
            return;
        }

        var socialAccount = await _sellerRepository.GetSocialAccountAsync(sellerId, @event.Channel, cancellationToken);
        if (socialAccount is null)
        {
            _logger.LogWarning("No social account found for seller {SellerId} on {Channel}", sellerId, @event.Channel);
            return;
        }

        await sender.SendMessageAsync(@event.CustomerExternalId, aiReply, socialAccount.AccessToken, cancellationToken);

        var replyMessage = new Message
        {
            ConversationId = conversation.Id,
            Content = aiReply,
            Role = MessageRole.Seller,
            IsAiGenerated = true,
            SentAt = DateTime.UtcNow
        };
        await _conversationRepository.AddMessageAsync(replyMessage, cancellationToken);

        conversation.Status = ConversationStatus.AiReplied;
        conversation.UpdatedAt = DateTime.UtcNow;
        await _conversationRepository.UpdateAsync(conversation, cancellationToken);

        _logger.LogInformation("Successfully processed and replied to message in conversation {ConversationId}", conversation.Id);
    }
}
