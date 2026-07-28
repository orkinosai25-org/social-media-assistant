using SocialMediaAssistant.Core.Entities;
using SocialMediaAssistant.Core.Interfaces;

namespace SocialMediaAssistant.Infrastructure.AI;

public class DisabledAIReplyService : IAIReplyService
{
    public Task<string> GenerateReplyAsync(
        string customerMessage,
        IEnumerable<Message> conversationHistory,
        string stockContext,
        CancellationToken cancellationToken = default)
        => throw new InvalidOperationException("Azure OpenAI is not configured. Set AzureOpenAI settings before processing messages.");
}
