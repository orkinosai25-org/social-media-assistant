using Azure;
using Azure.AI.OpenAI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialMediaAssistant.Core.Interfaces;
using SocialMediaAssistant.Core.Services;
using SocialMediaAssistant.Infrastructure.AI;
using SocialMediaAssistant.Infrastructure.BackgroundServices;
using SocialMediaAssistant.Infrastructure.Configuration;
using SocialMediaAssistant.Infrastructure.Data;
using SocialMediaAssistant.Infrastructure.Data.Repositories;
using SocialMediaAssistant.Infrastructure.Messaging;
using StackExchange.Redis;

namespace SocialMediaAssistant.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.Configure<AzureOpenAIOptions>(configuration.GetSection(AzureOpenAIOptions.SectionName));
        services.Configure<MetaApiOptions>(configuration.GetSection(MetaApiOptions.SectionName));

        services.AddScoped<IConversationRepository, ConversationRepository>();
        services.AddScoped<ISellerRepository, SellerRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<MessageProcessingService>();

        var openAiOptions = configuration.GetSection(AzureOpenAIOptions.SectionName).Get<AzureOpenAIOptions>()
            ?? new AzureOpenAIOptions();

        if (!string.IsNullOrWhiteSpace(openAiOptions.Endpoint) && !string.IsNullOrWhiteSpace(openAiOptions.ApiKey))
        {
            services.AddSingleton(new AzureOpenAIClient(
                new Uri(openAiOptions.Endpoint),
                new AzureKeyCredential(openAiOptions.ApiKey)));
            services.AddScoped<IAIReplyService, AzureOpenAIReplyService>();
        }
        else
        {
            services.AddScoped<IAIReplyService, DisabledAIReplyService>();
        }

        services.AddHttpClient<InstagramMessageSender>();
        services.AddHttpClient<FacebookMessageSender>();
        services.AddHttpClient<WhatsAppMessageSender>();
        services.AddScoped<IMessageSender, InstagramMessageSender>();
        services.AddScoped<IMessageSender, FacebookMessageSender>();
        services.AddScoped<IMessageSender, WhatsAppMessageSender>();

        var redisConnectionString = configuration["Redis:ConnectionString"];
        if (!string.IsNullOrWhiteSpace(redisConnectionString))
        {
            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect($"{redisConnectionString},abortConnect=false"));
        }

        services.AddHostedService<MessageProcessingWorker>();

        return services;
    }
}
