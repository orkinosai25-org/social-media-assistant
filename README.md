# Social Media Assistant

Social Media Assistant is a production-oriented .NET 9 SaaS scaffold for sellers who want AI-assisted customer messaging across Instagram, Facebook, and WhatsApp. The solution includes a webhook-driven API, a Blazor-based back office, core domain models, infrastructure integrations for Azure OpenAI/PostgreSQL/Redis, and test projects for core workflows.

## Prerequisites

- .NET 9 SDK
- Docker and Docker Compose
- Azure subscription with Azure OpenAI and Azure AD B2C
- Meta developer account for Instagram/Facebook/WhatsApp integrations
- PostgreSQL 16+ and Redis 7+ for local non-Docker development

## Quick Start

1. Copy the sample environment file:
   ```bash
   cp .env.example .env
   ```
2. Update the values for Azure OpenAI, Meta, database, Redis, and Azure AD B2C.
3. Start the local stack:
   ```bash
   docker compose up --build
   ```
4. Open:
   - API: `http://localhost:5000`
   - Web: `http://localhost:5001`

## Environment Variables

Use `.env.example` as the reference for required settings:

- `AZURE_OPENAI_*` for AI reply generation
- `META_*` for webhook verification and outbound messaging
- `ConnectionStrings__DefaultConnection` for PostgreSQL
- `Redis__ConnectionString` for Redis
- `AzureAdB2C__*` for seller authentication

## Development Setup

Restore, build, and test locally:

```bash
dotnet restore SocialMediaAssistant.sln
dotnet build SocialMediaAssistant.sln
dotnet test tests/SocialMediaAssistant.UnitTests/SocialMediaAssistant.UnitTests.csproj
```

Useful development entry points:

```bash
dotnet run --project src/SocialMediaAssistant.Api
dotnet run --project src/SocialMediaAssistant.Web
```

The API exposes Meta webhook endpoints under `/webhooks/{instagram|facebook|whatsapp}`. In development, OpenAPI is available from the API project.

## Project Structure

```text
src/
  SocialMediaAssistant.Core/            Domain entities, interfaces, services
  SocialMediaAssistant.Shared/          Shared DTOs and constants
  SocialMediaAssistant.Infrastructure/  EF Core, Azure OpenAI, Redis, messaging adapters
  SocialMediaAssistant.Api/             Webhook/API host
  SocialMediaAssistant.Web/             Blazor seller dashboard
tests/
  SocialMediaAssistant.UnitTests/       Core service tests
  SocialMediaAssistant.IntegrationTests/ API verification tests
```

## Architecture Notes

- **Core** keeps business rules and orchestration logic independent of frameworks.
- **Infrastructure** wires persistence, AI, HTTP messaging, and background processing.
- **API** receives webhook traffic and queues message processing.
- **Web** gives sellers a starting dashboard for inbox, products, and settings.
- **Docker Compose** provisions API, web, PostgreSQL, and Redis for local orchestration.

## Documentation

Live demo: https://orkinosai25-org.github.io/social-media-assistant

- [Feature Plan](docs/FEATURES.md)
- [Roadmap](docs/ROADMAP.md)
- [Tech Stack](docs/TECH_STACK.md)
- [Business Model](docs/BUSINESS_MODEL.md)
