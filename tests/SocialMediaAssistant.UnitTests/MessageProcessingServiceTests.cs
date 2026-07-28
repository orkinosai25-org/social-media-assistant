using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SocialMediaAssistant.Core.Entities;
using SocialMediaAssistant.Core.Events;
using SocialMediaAssistant.Core.Interfaces;
using SocialMediaAssistant.Core.Services;

namespace SocialMediaAssistant.UnitTests;

public class MessageProcessingServiceTests
{
    private readonly Mock<IConversationRepository> _conversationRepoMock = new();
    private readonly Mock<ISellerRepository> _sellerRepoMock = new();
    private readonly Mock<IStockService> _stockServiceMock = new();
    private readonly Mock<IAIReplyService> _aiReplyServiceMock = new();
    private readonly Mock<IMessageSender> _messageSenderMock = new();
    private readonly MessageProcessingService _sut;

    public MessageProcessingServiceTests()
    {
        _messageSenderMock.Setup(s => s.Channel).Returns(MessageChannel.Instagram);
        _sut = new MessageProcessingService(
            _conversationRepoMock.Object,
            _sellerRepoMock.Object,
            _stockServiceMock.Object,
            _aiReplyServiceMock.Object,
            new[] { _messageSenderMock.Object },
            NullLogger<MessageProcessingService>.Instance);
    }

    [Fact]
    public async Task ProcessAsync_WhenSellerNotFound_DoesNotProcessMessage()
    {
        _sellerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Seller?)null);

        var @event = CreateTestEvent();
        await _sut.ProcessAsync(@event, Guid.NewGuid());

        _aiReplyServiceMock.Verify(s => s.GenerateReplyAsync(
            It.IsAny<string>(),
            It.IsAny<IEnumerable<Message>>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ProcessAsync_WhenSellerInactive_DoesNotProcessMessage()
    {
        _sellerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Seller { IsActive = false });

        var @event = CreateTestEvent();
        await _sut.ProcessAsync(@event, Guid.NewGuid());

        _aiReplyServiceMock.Verify(s => s.GenerateReplyAsync(
            It.IsAny<string>(),
            It.IsAny<IEnumerable<Message>>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ProcessAsync_WhenAiFails_SetsConversationToNeedsReview()
    {
        var sellerId = Guid.NewGuid();
        var conversation = new Conversation { Id = Guid.NewGuid(), SellerId = sellerId };

        _sellerRepoMock.Setup(r => r.GetByIdAsync(sellerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Seller { Id = sellerId, IsActive = true });
        _conversationRepoMock.Setup(r => r.GetByExternalIdAsync(It.IsAny<string>(), It.IsAny<MessageChannel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(conversation);
        _conversationRepoMock.Setup(r => r.AddMessageAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _conversationRepoMock.Setup(r => r.GetRecentMessagesAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<Message>());
        _stockServiceMock.Setup(s => s.GetStockSummaryAsync(sellerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync("test stock");
        _aiReplyServiceMock.Setup(s => s.GenerateReplyAsync(It.IsAny<string>(), It.IsAny<IEnumerable<Message>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("AI failure"));
        _conversationRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Conversation>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Conversation c, CancellationToken _) => c);

        var @event = CreateTestEvent();
        await _sut.ProcessAsync(@event, sellerId);

        _conversationRepoMock.Verify(r => r.UpdateAsync(
            It.Is<Conversation>(c => c.Status == ConversationStatus.NeedsReview),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WhenSuccessful_SendsAndPersistsReply()
    {
        var sellerId = Guid.NewGuid();
        var conversation = new Conversation { Id = Guid.NewGuid(), SellerId = sellerId };
        var socialAccount = new SocialAccount { SellerId = sellerId, Channel = MessageChannel.Instagram, AccessToken = "token", IsActive = true };

        _sellerRepoMock.Setup(r => r.GetByIdAsync(sellerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Seller { Id = sellerId, IsActive = true });
        _sellerRepoMock.Setup(r => r.GetSocialAccountAsync(sellerId, MessageChannel.Instagram, It.IsAny<CancellationToken>()))
            .ReturnsAsync(socialAccount);
        _conversationRepoMock.Setup(r => r.GetByExternalIdAsync(It.IsAny<string>(), It.IsAny<MessageChannel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(conversation);
        _conversationRepoMock.Setup(r => r.AddMessageAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _conversationRepoMock.Setup(r => r.GetRecentMessagesAsync(conversation.Id, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<Message>());
        _stockServiceMock.Setup(s => s.GetStockSummaryAsync(sellerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync("test stock");
        _aiReplyServiceMock.Setup(s => s.GenerateReplyAsync(It.IsAny<string>(), It.IsAny<IEnumerable<Message>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("AI reply");
        _conversationRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Conversation>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Conversation c, CancellationToken _) => c);

        var @event = CreateTestEvent();
        await _sut.ProcessAsync(@event, sellerId);

        _messageSenderMock.Verify(s => s.SendMessageAsync(@event.CustomerExternalId, "AI reply", socialAccount.AccessToken, It.IsAny<CancellationToken>()), Times.Once);
        _conversationRepoMock.Verify(r => r.AddMessageAsync(It.Is<Message>(m => m.IsAiGenerated && m.Content == "AI reply"), It.IsAny<CancellationToken>()), Times.Once);
    }

    private static IncomingMessageEvent CreateTestEvent() => new(
        ExternalConversationId: "ext-conv-123",
        CustomerExternalId: "customer-456",
        CustomerName: "Test Customer",
        MessageText: "Do you have this in black?",
        ExternalMessageId: "msg-789",
        Channel: MessageChannel.Instagram,
        SenderAccountId: "page-101",
        ReceivedAt: DateTime.UtcNow);
}
