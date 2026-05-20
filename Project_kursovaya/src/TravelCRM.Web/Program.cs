using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TravelCRM.Application.Validators;
using TravelCRM.Infrastructure.Data;
using TravelCRM.Infrastructure.Repositories;
using TravelCRM.Web;
using TravelCRM.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// --- Blazor Server (interactive components) ---
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// --- EF Core (SQLite) через DI ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=travelcrm.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// --- Репозитории (Infrastructure) ---
builder.Services.AddScoped<ITouristRepository, TouristRepository>();

// --- FluentValidation: регистрируем валидаторы из Application ---
builder.Services.AddValidatorsFromAssemblyContaining<TouristCreateDtoValidator>();

// --- Бизнес-сервисы (Web) ---
builder.Services.AddScoped<ITouristService, TouristService>();
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();

var app = builder.Build();

// --- Middleware ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// API: безопасная отдача PDF из wwwroot/pdfs.
app.MapGet("/api/pdf/{fileName}", (string fileName, IWebHostEnvironment env) =>
{
    if (string.IsNullOrEmpty(fileName) ||
        fileName.Contains("..") || fileName.Contains('/') || fileName.Contains('\\'))
    {
        return Results.BadRequest("Неверное имя файла");
    }

    var filePath = Path.Combine(env.WebRootPath, "pdfs", fileName);
    if (!File.Exists(filePath))
    {
        return Results.NotFound("Файл не найден");
    }

    var bytes = File.ReadAllBytes(filePath);
    return Results.File(bytes, "application/pdf", fileName);
});

// --- Применение миграций и сидинг при старте ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
        SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Ошибка инициализации БД.");
    }
}

app.Run();

/// <summary>
/// Класс-маркер, используется тестовыми сборками для WebApplicationFactory.
/// </summary>
public partial class Program;
