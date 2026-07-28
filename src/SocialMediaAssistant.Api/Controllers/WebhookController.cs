using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SocialMediaAssistant.Core.Entities;
using SocialMediaAssistant.Core.Events;
using SocialMediaAssistant.Infrastructure.BackgroundServices;
using SocialMediaAssistant.Infrastructure.Configuration;
using SocialMediaAssistant.Shared.DTOs;

namespace SocialMediaAssistant.Api.Controllers;

[ApiController]
[Route("webhooks")]
public class WebhookController : ControllerBase
{
    private readonly MetaApiOptions _metaOptions;
    private readonly ILogger<WebhookController> _logger;

    public WebhookController(IOptions<MetaApiOptions> metaOptions, ILogger<WebhookController> logger)
    {
        _metaOptions = metaOptions.Value;
        _logger = logger;
    }

    [HttpGet("instagram")]
    public IActionResult VerifyInstagram(
        [FromQuery(Name = "hub.mode")] string mode,
        [FromQuery(Name = "hub.verify_token")] string token,
        [FromQuery(Name = "hub.challenge")] string challenge)
    {
        if (mode == "subscribe" && token == _metaOptions.VerifyToken)
        {
            return Ok(challenge);
        }

        return Unauthorized();
    }

    [HttpPost("instagram")]
    public async Task<IActionResult> InstagramWebhook()
    {
        if (!await ValidateSignatureAsync())
        {
            return Unauthorized("Invalid signature");
        }

        var body = await ReadBodyAsync();
        var payload = JsonSerializer.Deserialize<MetaWebhookPayload>(body);
        if (payload is null)
        {
            return BadRequest();
        }

        foreach (var entry in payload.Entry)
        {
            foreach (var messaging in entry.Messaging ?? [])
            {
                if (messaging.Message?.Text is null)
                {
                    continue;
                }

                EnqueueMessage(messaging, entry.Id, MessageChannel.Instagram);
            }
        }

        return Ok();
    }

    [HttpGet("facebook")]
    public IActionResult VerifyFacebook(
        [FromQuery(Name = "hub.mode")] string mode,
        [FromQuery(Name = "hub.verify_token")] string token,
        [FromQuery(Name = "hub.challenge")] string challenge)
    {
        if (mode == "subscribe" && token == _metaOptions.VerifyToken)
        {
            return Ok(challenge);
        }

        return Unauthorized();
    }

    [HttpPost("facebook")]
    public async Task<IActionResult> FacebookWebhook()
    {
        if (!await ValidateSignatureAsync())
        {
            return Unauthorized("Invalid signature");
        }

        var body = await ReadBodyAsync();
        var payload = JsonSerializer.Deserialize<MetaWebhookPayload>(body);
        if (payload is null)
        {
            return BadRequest();
        }

        foreach (var entry in payload.Entry)
        {
            foreach (var messaging in entry.Messaging ?? [])
            {
                if (messaging.Message?.Text is null)
                {
                    continue;
                }

                EnqueueMessage(messaging, entry.Id, MessageChannel.Facebook);
            }
        }

        return Ok();
    }

    [HttpGet("whatsapp")]
    public IActionResult VerifyWhatsApp(
        [FromQuery(Name = "hub.mode")] string mode,
        [FromQuery(Name = "hub.verify_token")] string token,
        [FromQuery(Name = "hub.challenge")] string challenge)
    {
        if (mode == "subscribe" && token == _metaOptions.VerifyToken)
        {
            return Ok(challenge);
        }

        return Unauthorized();
    }

    [HttpPost("whatsapp")]
    public async Task<IActionResult> WhatsAppWebhook()
    {
        if (!await ValidateSignatureAsync())
        {
            return Unauthorized("Invalid signature");
        }

        var body = await ReadBodyAsync();
        var payload = JsonSerializer.Deserialize<MetaWebhookPayload>(body);
        if (payload is null)
        {
            return BadRequest();
        }

        foreach (var entry in payload.Entry)
        {
            foreach (var change in entry.Changes ?? [])
            {
                if (change.Field != "messages")
                {
                    continue;
                }

                var messages = change.Value?.Messages ?? [];
                var contacts = change.Value?.Contacts ?? [];
                foreach (var message in messages.Where(m => m.Type == "text"))
                {
                    var contact = contacts.FirstOrDefault(c => c.WaId == message.From);
                    var @event = new IncomingMessageEvent(
                        ExternalConversationId: message.From,
                        CustomerExternalId: message.From,
                        CustomerName: contact?.Profile?.Name ?? message.From,
                        MessageText: message.Text?.Body ?? string.Empty,
                        ExternalMessageId: message.Id,
                        Channel: MessageChannel.WhatsApp,
                        SenderAccountId: change.Value?.Metadata?.PhoneNumberId ?? entry.Id,
                        ReceivedAt: DateTimeOffset.FromUnixTimeSeconds(long.Parse(message.Timestamp)).UtcDateTime);

                    var sellerId = Guid.Empty;
                    MessageProcessingWorker.MessageQueue.Enqueue((@event, sellerId));
                }
            }
        }

        return Ok();
    }

    private void EnqueueMessage(WebhookMessaging messaging, string pageId, MessageChannel channel)
    {
        var @event = new IncomingMessageEvent(
            ExternalConversationId: messaging.Sender.Id,
            CustomerExternalId: messaging.Sender.Id,
            CustomerName: messaging.Sender.Id,
            MessageText: messaging.Message!.Text!,
            ExternalMessageId: messaging.Message.Mid,
            Channel: channel,
            SenderAccountId: pageId,
            ReceivedAt: DateTimeOffset.FromUnixTimeMilliseconds(messaging.Timestamp).UtcDateTime);

        var sellerId = Guid.Empty;
        MessageProcessingWorker.MessageQueue.Enqueue((@event, sellerId));
        _logger.LogInformation("Enqueued {Channel} message for sender account {SenderAccountId}", channel, pageId);
    }

    private async Task<bool> ValidateSignatureAsync()
    {
        if (!Request.Headers.TryGetValue("X-Hub-Signature-256", out var signatureHeader))
        {
            return false;
        }

        var signature = signatureHeader.ToString();
        if (!signature.StartsWith("sha256=", StringComparison.Ordinal))
        {
            return false;
        }

        var body = await ReadBodyAsync();
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_metaOptions.AppSecret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(body));
        var expectedSignature = "sha256=" + Convert.ToHexString(hash).ToLowerInvariant();

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(signature),
            Encoding.UTF8.GetBytes(expectedSignature));
    }

    private async Task<string> ReadBodyAsync()
    {
        Request.EnableBuffering();
        Request.Body.Position = 0;
        using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        Request.Body.Position = 0;
        return body;
    }
}
