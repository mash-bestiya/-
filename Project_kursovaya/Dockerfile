# syntax=docker/dockerfile:1.7

# ---------- Этап 1: Сборка ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Восстановление зависимостей сначала (для кэширования слоёв)
COPY ["src/TravelCRM.Domain/TravelCRM.Domain.csproj",                 "src/TravelCRM.Domain/"]
COPY ["src/TravelCRM.Application/TravelCRM.Application.csproj",       "src/TravelCRM.Application/"]
COPY ["src/TravelCRM.Infrastructure/TravelCRM.Infrastructure.csproj", "src/TravelCRM.Infrastructure/"]
COPY ["src/TravelCRM.Web/TravelCRM.Web.csproj",                       "src/TravelCRM.Web/"]
RUN dotnet restore "src/TravelCRM.Web/TravelCRM.Web.csproj"

# Копируем остальной исходный код и публикуем
COPY src/ src/
WORKDIR /src/src/TravelCRM.Web
RUN dotnet publish "TravelCRM.Web.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

# ---------- Этап 2: Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Каталог для SQLite-БД, монтируется томом docker-compose
RUN mkdir -p /app/data && chown -R app:app /app/data
USER app

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "TravelCRM.Web.dll"]
