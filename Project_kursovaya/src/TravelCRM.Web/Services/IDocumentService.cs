using TravelCRM.Domain.Models;

namespace TravelCRM.Web.Services;

/// <summary>
/// Сервис управления документами поездки.
/// </summary>
public interface IDocumentService
{
    /// <summary>
    /// Получить все документы для поездки.
    /// </summary>
    Task<List<Document>> GetForTripAsync(int tripId);

    /// <summary>
    /// Сгенерировать (привязать) документ типа <paramref name="type"/> к поездке.
    /// Если документ уже есть — возвращается существующий.
    /// </summary>
    Task<Document> EnsureDocumentAsync(int tripId, DocType type, string description = "");

    /// <summary>
    /// Удалить документ.
    /// </summary>
    Task DeleteAsync(int documentId);

    /// <summary>
    /// Получить имя PDF-шаблона по типу документа (имя файла из wwwroot/pdfs).
    /// </summary>
    string GetTemplateFileName(DocType type);

    /// <summary>
    /// Человекочитаемое название типа документа.
    /// </summary>
    string GetTitle(DocType type);
}
