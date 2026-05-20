using Microsoft.EntityFrameworkCore;
using TravelCRM.Domain.Models;
using TravelCRM.Infrastructure.Data;

namespace TravelCRM.Web.Services;

/// <summary>
/// Реализация сервиса документов: привязывает PDF-шаблоны из wwwroot/pdfs
/// к конкретным поездкам и хранит метаданные в БД.
/// </summary>
public class DocumentService : IDocumentService
{
    private readonly AppDbContext _context;

    public DocumentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Document>> GetForTripAsync(int tripId)
    {
        return await _context.Documents
            .Where(d => d.TripId == tripId)
            .OrderBy(d => d.Type)
            .ToListAsync();
    }

    public async Task<Document> EnsureDocumentAsync(int tripId, DocType type, string description = "")
    {
        var existing = await _context.Documents
            .FirstOrDefaultAsync(d => d.TripId == tripId && d.Type == type);
        if (existing is not null) return existing;

        var doc = new Document
        {
            TripId = tripId,
            Type = type,
            Description = description,
            FilePath = $"/pdfs/{GetTemplateFileName(type)}",
            CreatedAt = DateTime.UtcNow,
        };
        _context.Documents.Add(doc);
        await _context.SaveChangesAsync();
        return doc;
    }

    public async Task DeleteAsync(int documentId)
    {
        var doc = await _context.Documents.FindAsync(documentId);
        if (doc is null) return;
        _context.Documents.Remove(doc);
        await _context.SaveChangesAsync();
    }

    public string GetTemplateFileName(DocType type) => type switch
    {
        DocType.Passport => "passport_template.pdf",
        DocType.IntPassport => "int_passport_template.pdf",
        DocType.Visa => "visa_template.pdf",
        DocType.Contract => "contract_template.pdf",
        DocType.Voucher => "voucher_template.pdf",
        DocType.Insurance => "insurance_template.pdf",
        DocType.Tickets => "tickets_template.pdf",
        DocType.Memo => "memo_template.pdf",
        _ => "memo_template.pdf",
    };

    public string GetTitle(DocType type) => type switch
    {
        DocType.Passport => "Паспорт РФ",
        DocType.IntPassport => "Загранпаспорт",
        DocType.Visa => "Виза",
        DocType.Contract => "Договор",
        DocType.Voucher => "Ваучер",
        DocType.Insurance => "Страховка",
        DocType.Tickets => "Билеты",
        DocType.Memo => "Памятка",
        _ => type.ToString(),
    };
}
