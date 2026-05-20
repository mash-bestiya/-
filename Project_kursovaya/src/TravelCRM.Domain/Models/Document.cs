namespace TravelCRM.Domain.Models;

public enum DocType { Passport, IntPassport, Visa, Contract, Voucher, Insurance, Tickets, Memo }
//класс для документов
public class Document
{
    public int Id { get; set; }
    public DocType Type { get; set; }
    public string Description { get; set; } = string.Empty; //например "Серия 1234 №567890"
    public string FilePath { get; set; } = string.Empty;   //путь к PDF в wwwroot
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int TripId { get; set; }
    public Trip? Trip { get; set; }
}