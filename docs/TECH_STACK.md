# 🛠️ Tech Stack

## Architecture Overview

```
┌─────────────────────────────────────────────┐
│          Seller Dashboard (Blazor)          │
└────────────────────┬────────────────────────┘
                     │
┌────────────────────▼────────────────────────┐
│          ASP.NET Core API + Webhooks        │
└──────┬───────────────┬───────────────┬──────┘
       │               │               │
┌──────▼──────┐ ┌──────▼──────┐ ┌─────▼──────┐
│  Meta API   │ │ Azure OpenAI│ │ PostgreSQL │
│ (IG/FB/WA)  │ │  (GPT-4o)   │ │ + Redis    │
└─────────────┘ └─────────────┘ └────────────┘
```

## Stack Details

| Layer | Technology | Notes |
|---|---|---|
| **Frontend** | ASP.NET Core Blazor Web App | Seller dashboard UI |
| **Backend** | ASP.NET Core Web API | REST API + Meta webhooks |
| **Domain** | .NET 9 class libraries | Core entities, services, DTOs |
| **AI Engine** | Azure OpenAI (GPT-4o) | Message understanding & replies |
| **Messaging** | Meta Graph API | Instagram, Facebook, WhatsApp |
| **Database** | PostgreSQL + EF Core | Conversations, catalog, tenant data |
| **Cache** | Redis | Queue/cache integration point |
| **Hosting** | Docker / Azure App Service / AKS | Containerized deployment |
| **Auth** | Azure AD B2C | Multi-tenant SaaS authentication |

## Solution Layout

- **SocialMediaAssistant.Core** — domain entities, repository contracts, and orchestration services
- **SocialMediaAssistant.Shared** — DTOs and shared constants
- **SocialMediaAssistant.Infrastructure** — EF Core, Azure OpenAI, Redis, messaging adapters, background worker
- **SocialMediaAssistant.Api** — webhook/API host
- **SocialMediaAssistant.Web** — Blazor dashboard
- **Tests** — unit and integration coverage for core flows and API verification

## Meta API Integrations

| Platform | API | Use Case |
|---|---|---|
| Instagram | Instagram Graph API | DMs and webhook ingestion |
| Facebook | Messenger Platform API | Page messaging |
| WhatsApp | WhatsApp Business Cloud API | Customer messaging |

## Azure Services Used

- **Azure OpenAI** — GPT-4o for AI replies
- **Azure Database for PostgreSQL** — Main relational store
- **Azure Cache for Redis** — Low-latency cache and queue support
- **Azure AD B2C** — Seller authentication
- **Azure Monitor / Application Insights** — Observability and alerting
