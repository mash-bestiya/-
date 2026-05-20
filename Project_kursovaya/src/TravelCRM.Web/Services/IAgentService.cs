using TravelCRM.Application.Dtos;
using TravelCRM.Domain.Models;

namespace TravelCRM.Web.Services;

/// <summary>
/// Сервис рабочих сценариев агента туристического агентства.
/// </summary>
public interface IAgentService
{
    /// <summary>
    /// Создать нового туриста на основе DTO.
    /// </summary>
    Task<Tourist> CreateTouristAsync(TouristCreateDto dto);

    /// <summary>
    /// Список всех туристов.
    /// </summary>
    Task<List<Tourist>> GetAllTouristsAsync();

    /// <summary>
    /// Поиск туристов по фамилии, имени или телефону.
    /// </summary>
    Task<List<Tourist>> SearchAsync(string query);
}
