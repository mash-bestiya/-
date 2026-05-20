using TravelCRM.Domain.Models;

namespace TravelCRM.Application.Dtos;

/// <summary>
/// DTO для создания поездки через UI.
/// </summary>
public class TripCreateDto
{
    public int TouristId { get; set; }
    public string DepartureCity { get; set; } = string.Empty;
    public string ArrivalCity { get; set; } = string.Empty;
    public DateTime StartDate { get; set; } = DateTime.Today.AddDays(7);
    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(14);
    public string HotelName { get; set; } = string.Empty;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
}
