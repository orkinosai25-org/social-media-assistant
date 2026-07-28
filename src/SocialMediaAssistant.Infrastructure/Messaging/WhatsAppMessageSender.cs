using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SocialMediaAssistant.Core.Entities;
using SocialMediaAssistant.Core.Interfaces;
using SocialMediaAssistant.Infrastructure.Configuration;
using System.Net.Http.Json;

namespace SocialMediaAssistant.Infrastructure.Messaging;

public class WhatsAppMessageSender : IMessageSender
{
    private readonly HttpClient _httpClient;
    private readonly MetaApiOptions _options;
    private readonly ILogger<WhatsAppMessageSender> _logger;

    public WhatsAppMessageSender(
        HttpClient httpClient,
        IOptions<MetaApiOptions> options,
        ILogger<WhatsAppMessageSender> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public MessageChannel Channel => MessageChannel.WhatsApp;

    public async Task SendMessageAsync(string recipientId, string message, string accessToken, CancellationToken cancellationToken = default)
    {
        var phoneNumberId = "YOUR_PHONE_NUMBER_ID";
        var url = $"{_options.GraphApiBaseUrl}/{phoneNumberId}/messages?access_token={accessToken}";

        var payload = new
        {
            messaging_product = "whatsapp",
            to = recipientId,
            type = "text",
            text = new { body = message }
        };

        var response = await _httpClient.PostAsJsonAsync(url, payload, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("Failed to send WhatsApp message: {Error}", error);
            response.EnsureSuccessStatusCode();
        }

        _logger.LogInformation("WhatsApp message sent to {RecipientId}", recipientId);
    }
}
