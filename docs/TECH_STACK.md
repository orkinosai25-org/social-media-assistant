# 🛠️ Tech Stack

## Architecture Overview

```
┌─────────────────────────────────────────────┐
│              Seller Dashboard               │
│           (React / Next.js)                 │
└────────────────────┬────────────────────────┘
                     │
┌────────────────────▼────────────────────────┐
│             Backend API                     │
│           (Python / FastAPI)                │
└──────┬───────────────┬───────────────┬──────┘
       │               │               │
┌──────▼──────┐ ┌──────▼──────┐ ┌─────▼──────┐
│  Meta API   │ │ Azure OpenAI│ │  Stock DB  │
│ (IG/FB/WA)  │ │  (GPT-4o)   │ │ (Postgres) │
└─────────────┘ └─────────────┘ └────────────┘
```

## Stack Details

| Layer | Technology | Notes |
|---|---|---|
| **Frontend** | React / Next.js | Seller dashboard UI |
| **Backend** | Python (FastAPI) | REST API + webhooks |
| **AI Engine** | Azure OpenAI (GPT-4o) | Message understanding & replies |
| **Messaging** | Meta Graph API | Instagram, Facebook, WhatsApp |
| **Database** | Azure PostgreSQL | Conversations, orders, stock |
| **Cache** | Redis | Session state, rate limiting |
| **Hosting** | Azure App Service / AKS | Scalable, enterprise-grade |
| **Auth** | Azure AD B2C | Multi-tenant SaaS auth |
| **Storage** | Azure Blob Storage | Product images, attachments |

## Meta API Integrations

| Platform | API | Use Case |
|---|---|---|
| Instagram | Instagram Graph API | DMs, story replies, comments |
| Facebook | Messenger Platform API | Page messages, comments |
| WhatsApp | WhatsApp Business Cloud API | Customer messaging, broadcasts |

## Azure Services Used

- **Azure OpenAI** — GPT-4o for AI replies
- **Azure App Service** — Backend hosting
- **Azure PostgreSQL Flexible Server** — Main database
- **Azure Cache for Redis** — Session caching
- **Azure AD B2C** — Customer authentication
- **Azure Blob Storage** — File storage
- **Azure Monitor** — Logging & alerting
