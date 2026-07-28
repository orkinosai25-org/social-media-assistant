FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/SocialMediaAssistant.Web/SocialMediaAssistant.Web.csproj", "src/SocialMediaAssistant.Web/"]
COPY ["src/SocialMediaAssistant.Core/SocialMediaAssistant.Core.csproj", "src/SocialMediaAssistant.Core/"]
COPY ["src/SocialMediaAssistant.Infrastructure/SocialMediaAssistant.Infrastructure.csproj", "src/SocialMediaAssistant.Infrastructure/"]
COPY ["src/SocialMediaAssistant.Shared/SocialMediaAssistant.Shared.csproj", "src/SocialMediaAssistant.Shared/"]
RUN dotnet restore "src/SocialMediaAssistant.Web/SocialMediaAssistant.Web.csproj"
COPY . .
WORKDIR "/src/src/SocialMediaAssistant.Web"
RUN dotnet build "SocialMediaAssistant.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SocialMediaAssistant.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SocialMediaAssistant.Web.dll"]
