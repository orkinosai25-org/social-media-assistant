using SocialMediaAssistant.Core.Entities;

namespace SocialMediaAssistant.Core.Interfaces;

/// <summary>Provides AI-powered reply generation for customer messages.</summary>
public interface IAIReplyService
{
    /// <summary>
    /// Generates an AI reply for a customer message given conversation history and product context.
    /// </summary>
    Task<string> GenerateReplyAsync(
        string customerMessage,
        IEnumerable<Message> conversationHistory,
        string stockContext,
        CancellationToken cancellationToken = default);
}
