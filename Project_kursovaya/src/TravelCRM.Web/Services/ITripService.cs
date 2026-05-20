using TravelCRM.Application.Dtos;
using TravelCRM.Domain.Models;

namespace TravelCRM.Web.Services;

/// <summary>
/// Интерфейс сервиса для управления поездками.
/// </summary>
public interface ITripService
{
    /// <summary>
    /// Получить поездку по ID со всеми документами и туристом.
    /// </summary>
    Task<Trip?> GetTripDetailsAsync(int id);

    /// <summary>
    /// Обновить статус оплаты поездки.
    /// </summary>
    Task UpdatePaymentStatusAsync(int tripId, PaymentStatus status);

    /// <summary>
    /// Создать новую поездку из DTO.
    /// </summary>
    Task<Trip> CreateAsync(TripCreateDto dto);

    /// <summary>
    /// Удалить поездку.
    /// </summary>
    Task DeleteAsync(int tripId);

    /// <summary>
    /// Все поездки конкретного туриста.
    /// </summary>
    Task<List<Trip>> GetForTouristAsync(int touristId);
}
