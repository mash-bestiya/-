# TravelCRM — курсовой проект

Веб-приложение CRM-системы для туристического агентства.
Курсовая работа по дисциплине «Кроссплатформенная среда исполнения программного обеспечения».

**Стек:** .NET 10 · ASP.NET Core · Blazor Server · Entity Framework Core (Code-First) · SQLite · FluentValidation · xUnit · Docker

> 🔗 **GitHub:** https://github.com/yourusername/TravelCRM
> 🐳 **Docker Hub:** https://hub.docker.com/r/yourusername/travelcrm

---

## Архитектура

Проект построен по принципам Clean Architecture: слои разделены по проектам, зависимости направлены внутрь.

```
Project_kursovaya/
├── Project_kursovaya.sln
├── Dockerfile                 # multi-stage сборка
├── docker-compose.yml         # оркестрация (app + volume)
├── .editorconfig              # стиль кода
├── .gitignore  .dockerignore
├── dotnet-tools.json          # локальный dotnet-ef
│
├── src/
│   ├── TravelCRM.Domain/            # доменные модели (Tourist, Trip, Document, LinkedTourist)
│   │
│   ├── TravelCRM.Application/       # DTO, FluentValidation-валидаторы
│   │   ├── Dtos/                    # TouristCreateDto, TripCreateDto
│   │   └── Validators/              # *Validator.cs
│   │
│   ├── TravelCRM.Infrastructure/    # EF Core: DbContext, репозитории, миграции, SeedData
│   │   ├── Data/AppDbContext.cs     # Fluent API, отношения 1:N и N:N
│   │   ├── Data/SeedData.cs         # тестовые данные
│   │   ├── Repositories/            # ITouristRepository + реализация
│   │   └── Migrations/              # EF Core CodeFirst миграции
│   │
│   └── TravelCRM.Web/               # Blazor Server (UI + DI + миграции при старте)
│       ├── Pages/                   # Login, Tourist, Agent (Dashboard, CreateTourist, CreateTrip, TouristDetails), Trip/Details
│       ├── Services/                # TouristService, TripService, AgentService, DocumentService
│       ├── Shared/                  # NavMenu, ConfirmDialog
│       ├── Validation/              # FluentValidationValidator (адаптер для Blazor EditForm)
│       ├── wwwroot/                 # css, pdfs/
│       ├── Program.cs               # точка входа + DI
│       └── appsettings*.json
│
└── tests/
    └── TravelCRM.Tests/             # xUnit + FluentAssertions + EF InMemory
```

### Доменная модель (отношения)

| Сущность | Связь | Описание |
|---|---|---|
| `Tourist` ↔ `Trip` | 1:N | У туриста много поездок |
| `Trip` ↔ `Document` | 1:N | У поездки много документов |
| `Tourist` ↔ `Tourist` через `LinkedTourist` | N:N | Спутники путешествия |
| `Trip` ↔ `Document` | 1:1 в рамках типа | Уникальный документ каждого типа на поездку (логика в `DocumentService`) |

Конфигурация ключей, индексов и стратегий удаления — в [AppDbContext.cs](src/TravelCRM.Infrastructure/Data/AppDbContext.cs) через Fluent API.

---

## Локальный запуск (без Docker)

### Требования
- .NET SDK **10.0** (или 8/9 — поменяйте `TargetFramework` в csproj)
- На Windows / Linux / macOS — кроссплатформенно

### Шаги

```bash
# 1. Клонировать
git clone https://github.com/yourusername/TravelCRM.git
cd TravelCRM

# 2. Восстановить инструменты и пакеты
dotnet tool restore
dotnet restore

# 3. Применить миграции (при первом старте Program.cs сделает это сам,
#    но при желании можно вручную)
dotnet ef database update \
    --project src/TravelCRM.Infrastructure \
    --startup-project src/TravelCRM.Web

# 4. Запустить приложение
dotnet run --project src/TravelCRM.Web

# 5. Открыть в браузере
#    http://localhost:5000
```

### Демо-доступы

После первого запуска БД будет заполнена тестовыми данными:

| Роль | Вход |
|---|---|
| Турист (Пушкин А. С.) | телефон `+79001112233` на главной |
| Турист (Толстой Л. Н.) | `+79002223344` |
| Турист (Гагарин Ю. А.) | `+79005556677` |
| Агент агентства | кнопка «Я турагентство» (без пароля) |

---

## Запуск через Docker

```bash
# Собрать и поднять контейнер с volume для SQLite
docker compose up -d --build

# Открыть в браузере
#  http://localhost:8080

# Логи
docker compose logs -f travelcrm-web

# Остановить
docker compose down
```

### Публикация образа в Docker Hub

```bash
docker build -t yourusername/travelcrm:latest .
docker login
docker push yourusername/travelcrm:latest
```

Имя образа в `docker-compose.yml` (`image: travelcrm:latest`) подразумевает локальную сборку. Для деплоя готового образа замените на `yourusername/travelcrm:latest`.

---

## Работа с миграциями

```bash
# Создать новую миграцию
dotnet ef migrations add <Имя> \
    --project src/TravelCRM.Infrastructure \
    --startup-project src/TravelCRM.Web \
    --output-dir Migrations

# Применить к БД
dotnet ef database update \
    --project src/TravelCRM.Infrastructure \
    --startup-project src/TravelCRM.Web

# Откатить последнюю
dotnet ef migrations remove \
    --project src/TravelCRM.Infrastructure \
    --startup-project src/TravelCRM.Web
```

Существующие миграции: `Migrations/20260520183931_InitialCreate.cs`.

При старте `Program.cs` сам выполняет `context.Database.Migrate()` и затем `SeedData.Initialize()` — БД создаётся и наполняется автоматически.

---

## Тестирование

```bash
dotnet test --nologo

# с покрытием
dotnet test --collect:"XPlat Code Coverage"
```

Покрыты:
- Валидаторы FluentValidation (`TouristCreateDtoValidator`, `TripCreateDtoValidator`).
- Репозиторий туристов через EF InMemory.

---

## Применённые технологии и требования

| Требование ТЗ | Реализация |
|---|---|
| .NET 8+ | .NET 10 |
| ASP.NET Core + Blazor Server | `TravelCRM.Web`, `Microsoft.NET.Sdk.Web`, Razor-компоненты |
| EF Core Code-First | `TravelCRM.Infrastructure` + миграции в `Migrations/` |
| Отношения 1:1, 1:N, N:N | `Tourist`↔`Trip` (1:N), `Trip`↔`Document` (1:N), `Tourist`↔`Tourist` через `LinkedTourist` (N:N), `Trip`↔`Document` уникальный по типу (1:1) |
| Репозитории + Fluent API | `ITouristRepository`/`TouristRepository`, `AppDbContext.OnModelCreating` |
| DI через `IServiceCollection` | `Program.cs` — `AddScoped`, `AddValidatorsFromAssemblyContaining`, `AddDbContext` |
| Компоненты Razor, роутинг, формы, валидация | Pages под `/`, `/login`, `/tourist/{id}`, `/trip/{id}`, `/agent/...` + `EditForm` + кастомный `FluentValidationValidator` |
| FluentValidation | `TouristCreateDtoValidator`, `TripCreateDtoValidator` |
| Модальные окна | `Shared/ConfirmDialog.razor` |
| Docker multi-stage | `Dockerfile` (sdk:10.0 → aspnet:10.0) |
| Volume для БД | `sqlite_data:/app/data` в `docker-compose.yml` |
| Healthcheck | `docker-compose.yml` — wget-проверка `/` |
| Unit-тесты (xUnit) | `tests/TravelCRM.Tests` |
| .editorconfig / StyleCop | `.editorconfig` с правилами форматирования и именования |
| XML-комментарии | `GenerateDocumentationFile=true` во всех проектах |
| README | этот файл |

---

## Лицензия и автор

Курсовая работа: **Горюнова М. М.**, Москва, 2026 г.
