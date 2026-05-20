using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TravelCRM.Domain.Models;

namespace TravelCRM.Infrastructure.Data;

/// <summary>
/// Заполнение базы демонстрационными данными при первом старте приложения.
/// </summary>
public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new AppDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

        if (context.Tourists.Any())
        {
            return;
        }

        var pushkin = new Tourist
        {
            Phone = "+79001112233",
            LastName = "Пушкин",
            FirstName = "Александр",
            MiddleName = "Сергеевич",
            EngFirstName = "Alexander",
            EngLastName = "Pushkin",
            PassportInfo = "4500 123456 Выдан ОУФМС России по г. Москве",
            IntPassportInfo = "75 1234567 действителен до 2030 г.",
        };
        var tolstoy = new Tourist
        {
            Phone = "+79002223344",
            LastName = "Толстой",
            FirstName = "Лев",
            MiddleName = "Николаевич",
            EngFirstName = "Leo",
            EngLastName = "Tolstoy",
            PassportInfo = "4500 654321 Выдан ОУФМС России по г. Туле",
        };
        var tchaikovsky = new Tourist
        {
            Phone = "+79003334455",
            LastName = "Чайковский",
            FirstName = "Петр",
            MiddleName = "Ильич",
            EngFirstName = "Pyotr",
            EngLastName = "Tchaikovsky",
            PassportInfo = "4500 112233 Выдан ОУФМС России по г. СПб",
        };
        var mendeleev = new Tourist
        {
            Phone = "+79004445566",
            LastName = "Менделеев",
            FirstName = "Дмитрий",
            MiddleName = "Иванович",
            EngFirstName = "Dmitry",
            EngLastName = "Mendeleev",
            PassportInfo = "4500 998877 Выдан ОУФМС России по г. Тобольску",
        };
        var gagarin = new Tourist
        {
            Phone = "+79005556677",
            LastName = "Гагарин",
            FirstName = "Юрий",
            MiddleName = "Алексеевич",
            EngFirstName = "Yuri",
            EngLastName = "Gagarin",
            PassportInfo = "4500 554433 Выдан ОУФМС России по г. Гжатск",
        };

        context.Tourists.AddRange(pushkin, tolstoy, tchaikovsky, mendeleev, gagarin);
        context.SaveChanges();

        var pushkinTrip = new Trip
        {
            TouristId = pushkin.Id,
            DepartureCity = "Москва",
            ArrivalCity = "Анталья",
            StartDate = DateTime.Today.AddDays(14),
            EndDate = DateTime.Today.AddDays(21),
            HotelName = "Rixos Premium Belek 5*",
            PaymentStatus = PaymentStatus.Pending,
        };
        var pushkinPast = new Trip
        {
            TouristId = pushkin.Id,
            DepartureCity = "Москва",
            ArrivalCity = "Санкт-Петербург",
            StartDate = DateTime.Today.AddDays(-60),
            EndDate = DateTime.Today.AddDays(-50),
            HotelName = "Гранд Отель Европа",
            PaymentStatus = PaymentStatus.Paid,
        };
        var tolstoyTrip = new Trip
        {
            TouristId = tolstoy.Id,
            DepartureCity = "Москва",
            ArrivalCity = "Ясная Поляна",
            StartDate = DateTime.Today.AddDays(7),
            EndDate = DateTime.Today.AddDays(10),
            HotelName = "Усадьба Толстого",
            PaymentStatus = PaymentStatus.Paid,
        };
        var gagarinTrip = new Trip
        {
            TouristId = gagarin.Id,
            DepartureCity = "Москва",
            ArrivalCity = "Байконур",
            StartDate = DateTime.Today.AddDays(30),
            EndDate = DateTime.Today.AddDays(45),
            HotelName = "Космодром Inn",
            PaymentStatus = PaymentStatus.Unpaid,
        };

        context.Trips.AddRange(pushkinTrip, pushkinPast, tolstoyTrip, gagarinTrip);
        context.SaveChanges();

        context.Documents.AddRange(
            new Document
            {
                TripId = pushkinTrip.Id,
                Type = DocType.Contract,
                Description = "Договор о реализации турпродукта № 2026-0042",
                FilePath = "/pdfs/contract_template.pdf",
            },
            new Document
            {
                TripId = pushkinTrip.Id,
                Type = DocType.Voucher,
                Description = "Ваучер на проживание Rixos Premium Belek 5*",
                FilePath = "/pdfs/voucher_template.pdf",
            },
            new Document
            {
                TripId = tolstoyTrip.Id,
                Type = DocType.Memo,
                Description = "Памятка туристу",
                FilePath = "/pdfs/memo_template.pdf",
            });
        context.SaveChanges();

        context.LinkedTourists.Add(new LinkedTourist
        {
            MainTouristId = pushkin.Id,
            LinkedTouristId = tolstoy.Id,
            RelationType = "Спутник",
        });
        context.SaveChanges();
    }
}
