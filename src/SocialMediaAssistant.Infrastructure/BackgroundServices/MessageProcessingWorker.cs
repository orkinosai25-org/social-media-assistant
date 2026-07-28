using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SocialMediaAssistant.Core.Events;
using SocialMediaAssistant.Core.Services;
using System.Collections.Concurrent;

namespace SocialMediaAssistant.Infrastructure.BackgroundServices;

public class MessageProcessingWorker : BackgroundService
{
    public static readonly ConcurrentQueue<(IncomingMessageEvent Event, Guid SellerId)> MessageQueue = new();

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MessageProcessingWorker> _logger;

    public MessageProcessingWorker(IServiceScopeFactory scopeFactory, ILogger<MessageProcessingWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Message processing worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            if (MessageQueue.TryDequeue(out var item))
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var processor = scope.ServiceProvider.GetRequiredService<MessageProcessingService>();
                    await processor.ProcessAsync(item.Event, item.SellerId, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message from queue");
                }
            }
            else
            {
                await Task.Delay(500, stoppingToken);
            }
        }
    }
}
