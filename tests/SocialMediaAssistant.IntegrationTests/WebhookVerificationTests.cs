using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SocialMediaAssistant.IntegrationTests;

public class WebhookVerificationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public WebhookVerificationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task InstagramWebhook_WithWrongToken_Returns401()
    {
        var response = await _client.GetAsync("/webhooks/instagram?hub.mode=subscribe&hub.verify_token=wrong&hub.challenge=test");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task FacebookWebhook_WithWrongToken_Returns401()
    {
        var response = await _client.GetAsync("/webhooks/facebook?hub.mode=subscribe&hub.verify_token=wrong&hub.challenge=test");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }
}
