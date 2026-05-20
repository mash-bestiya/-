using Microsoft.EntityFrameworkCore;
using TravelCRM.Domain.Models;

namespace TravelCRM.Infrastructure.Data;

/// <summary>
/// Контекст базы данных приложения.
/// Использует подход CodeFirst и Fluent API для конфигурации.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tourist> Tourists { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<LinkedTourist> LinkedTourists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- Конфигурация Tourist ---
        modelBuilder.Entity<Tourist>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Phone).IsRequired().HasMaxLength(20);
            
            // Индекс для быстрого поиска по телефону (требование ТЗ)
            entity.HasIndex(t => t.Phone).IsUnique();
        });

        // --- Конфигурация Trip ---
        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(t => t.Id);
            
            // Связь 1:N: Один турист -> Много поездок
            entity.HasOne(t => t.Tourist)
                  .WithMany(t => t.Trips)
                  .HasForeignKey(t => t.TouristId)
                  .OnDelete(DeleteBehavior.Cascade); // Если удалили туриста, удаляем его поездки
        });

        // --- Конфигурация Document ---
        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(d => d.Id);
            
            // Связь 1:N: Одна поездка -> Много документов
            entity.HasOne(d => d.Trip)
                  .WithMany(t => t.Documents)
                  .HasForeignKey(d => d.TripId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // --- Конфигурация LinkedTourist (Связь N:N) ---
        modelBuilder.Entity<LinkedTourist>(entity =>
        {
            // Составной первичный ключ
            entity.HasKey(lt => new { lt.MainTouristId, lt.LinkedTouristId });

            entity.HasOne(lt => lt.MainTourist)
                  .WithMany(t => t.LinkedAsMain)
                  .HasForeignKey(lt => lt.MainTouristId)
                  .OnDelete(DeleteBehavior.Restrict); // Чтобы не удалять главного, если есть связи

            entity.HasOne(lt => lt.LinkedTouristRef) // <-- Новое имя свойства
                    .WithMany(t => t.LinkedAsSecondary)
                    .HasForeignKey(lt => lt.LinkedTouristId)
                    .OnDelete(DeleteBehavior.Restrict);
        });
    }
}