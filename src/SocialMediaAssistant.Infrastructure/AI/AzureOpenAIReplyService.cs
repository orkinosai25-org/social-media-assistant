using Azure.AI.OpenAI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using SocialMediaAssistant.Core.Entities;
using SocialMediaAssistant.Core.Interfaces;
using SocialMediaAssistant.Infrastructure.Configuration;
using SocialMediaAssistant.Shared.Constants;

namespace SocialMediaAssistant.Infrastructure.AI;

public class AzureOpenAIReplyService : IAIReplyService
{
    private readonly AzureOpenAIClient _client;
    private readonly AzureOpenAIOptions _options;
    private readonly ILogger<AzureOpenAIReplyService> _logger;

    public AzureOpenAIReplyService(
        AzureOpenAIClient client,
        IOptions<AzureOpenAIOptions> options,
        ILogger<AzureOpenAIReplyService> logger)
    {
        _client = client;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> GenerateReplyAsync(
        string customerMessage,
        IEnumerable<Message> conversationHistory,
        string stockContext,
        CancellationToken cancellationToken = default)
    {
        var chatClient = _client.GetChatClient(_options.DeploymentName);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(AiPrompts.SellerAssistantSystemPrompt),
            new SystemChatMessage($"Product catalog context:\n{stockContext}")
        };

        foreach (var message in conversationHistory)
        {
            if (message.Role == MessageRole.Customer)
            {
                messages.Add(new UserChatMessage(message.Content));
            }
            else if (message.Role == MessageRole.Seller)
            {
                messages.Add(new AssistantChatMessage(message.Content));
            }
        }

        messages.Add(new UserChatMessage(customerMessage));

        var response = await chatClient.CompleteChatAsync(messages, cancellationToken: cancellationToken);
        var reply = response.Value.Content.FirstOrDefault()?.Text ?? string.Empty;

        _logger.LogInformation("AI reply generated ({Length} chars)", reply.Length);
        return reply;
    }
}
