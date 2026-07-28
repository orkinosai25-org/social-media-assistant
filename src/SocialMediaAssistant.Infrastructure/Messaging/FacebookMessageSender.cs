using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SocialMediaAssistant.Core.Entities;
using SocialMediaAssistant.Core.Interfaces;
using SocialMediaAssistant.Infrastructure.Configuration;
using System.Net.Http.Json;

namespace SocialMediaAssistant.Infrastructure.Messaging;

public class FacebookMessageSender : IMessageSender
{
    private readonly HttpClient _httpClient;
    private readonly MetaApiOptions _options;
    private readonly ILogger<FacebookMessageSender> _logger;

    public FacebookMessageSender(
        HttpClient httpClient,
        IOptions<MetaApiOptions> options,
        ILogger<FacebookMessageSender> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public MessageChannel Channel => MessageChannel.Facebook;

    public async Task SendMessageAsync(string recipientId, string message, string accessToken, CancellationToken cancellationToken = default)
    {
        var payload = new
        {
            recipient = new { id = recipientId },
            message = new { text = message }
        };

        var url = $"{_options.GraphApiBaseUrl}/me/messages?access_token={accessToken}";
        var response = await _httpClient.PostAsJsonAsync(url, payload, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("Failed to send Facebook message: {Error}", error);
            response.EnsureSuccessStatusCode();
        }

        _logger.LogInformation("Facebook message sent to {RecipientId}", recipientId);
    }
}
