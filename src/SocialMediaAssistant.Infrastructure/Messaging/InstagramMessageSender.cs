using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SocialMediaAssistant.Core.Entities;
using SocialMediaAssistant.Core.Interfaces;
using SocialMediaAssistant.Infrastructure.Configuration;
using System.Net.Http.Json;

namespace SocialMediaAssistant.Infrastructure.Messaging;

public class InstagramMessageSender : IMessageSender
{
    private readonly HttpClient _httpClient;
    private readonly MetaApiOptions _options;
    private readonly ILogger<InstagramMessageSender> _logger;

    public InstagramMessageSender(
        HttpClient httpClient,
        IOptions<MetaApiOptions> options,
        ILogger<InstagramMessageSender> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public MessageChannel Channel => MessageChannel.Instagram;

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
            _logger.LogError("Failed to send Instagram message: {Error}", error);
            response.EnsureSuccessStatusCode();
        }

        _logger.LogInformation("Instagram message sent to {RecipientId}", recipientId);
    }
}
