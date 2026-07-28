using SocialMediaAssistant.Core.Entities;

namespace SocialMediaAssistant.Core.Interfaces;

/// <summary>Sends messages back to customers on a specific social media channel.</summary>
public interface IMessageSender
{
    /// <summary>The channel this sender handles.</summary>
    MessageChannel Channel { get; }

    /// <summary>Sends a text message to a customer.</summary>
    Task SendMessageAsync(string recipientId, string message, string accessToken, CancellationToken cancellationToken = default);
}
