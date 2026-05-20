using System;
using System.Collections.Generic;

namespace TravelCRM.Domain.Models;

/// Статус оплаты поездки.
public enum PaymentStatus
{
    Unpaid,   // Не оплачено
    Pending,  // В процессе (деньги списаны, но не подтверждены агентством)
    Paid      // Оплачено
}

/// Сущность "Поездка".
public class Trip
{
    public int Id { get; set; }

    public string DepartureCity { get; set; } = string.Empty;
    public string ArrivalCity { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public string HotelName { get; set; } = string.Empty;
    
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;

    /// Внешний ключ на туриста.
    public int TouristId { get; set; }
    
    /// Навигационное свойство.
    public Tourist? Tourist { get; set; }

    /// Документы по этой поездке (договор, ваучер и т.д.).
    public ICollection<Document> Documents { get; set; } = new List<Document>();
}