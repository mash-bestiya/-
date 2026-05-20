using System.Collections.Generic;
using System.Threading.Tasks;
using TravelCRM.Domain.Models;

namespace TravelCRM.Web.Services;

/// <summary>
/// Интерфейс сервиса для управления данными туристов.
/// </summary>
public interface ITouristService
{
    /// <summary>
    /// Вход туриста по номеру телефона.
    /// </summary>
    Task<Tourist?> LoginAsync(string phone);

    /// <summary>
    /// Получение полного профиля туриста по ID.
    /// </summary>
    Task<Tourist?> GetProfileAsync(int id);

    /// <summary>
    /// Создание нового туриста (для агента).
    /// </summary>
    Task CreateTouristAsync(Tourist tourist);

    /// <summary>
    /// Поиск туристов по фамилии или телефону (для агента).
    /// </summary>
    Task<List<Tourist>> SearchAgentsAsync(string query);
}