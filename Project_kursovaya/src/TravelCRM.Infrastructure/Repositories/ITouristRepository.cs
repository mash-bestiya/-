using System.Collections.Generic;
using System.Threading.Tasks;
using TravelCRM.Domain.Models;

namespace TravelCRM.Infrastructure.Repositories;

/// <summary>
/// Интерфейс репозитория для работы с туристами.
/// Абстрагирует доступ к данным от бизнес-логики.
/// </summary>
public interface ITouristRepository
{
    /// <summary>
    /// Найти туриста по номеру телефона.
    /// </summary>
    Task<Tourist?> GetByPhoneAsync(string phone);

    /// <summary>
    /// Найти туриста по ID со всеми связанными данными (поездки, документы).
    /// </summary>
    Task<Tourist?> GetByIdWithDetailsAsync(int id);

    /// <summary>
    /// Создать нового туриста.
    /// </summary>
    Task AddAsync(Tourist tourist);

    /// <summary>
    /// Сохранить изменения в БД.
    /// </summary>
    Task SaveChangesAsync();

    /// <summary>
    /// Поиск туристов по фамилии или телефону (для агента).
    /// </summary>
    Task<List<Tourist>> SearchAsync(string query);
}