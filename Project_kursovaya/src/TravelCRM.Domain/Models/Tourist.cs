using System.Collections.Generic; // Это важно! Без этого не работают списки

namespace TravelCRM.Domain.Models;

public class Tourist
{
    public int Id { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    
    public string EngFirstName { get; set; } = string.Empty;
    public string EngLastName { get; set; } = string.Empty;

    public string PassportInfo { get; set; } = string.Empty;
    public string IntPassportInfo { get; set; } = string.Empty;
    public string VisaInfo { get; set; } = string.Empty;

    // Связь с поездками (1:N)
    public ICollection<Trip> Trips { get; set; } = new List<Trip>();

    // Связи с другими туристами (N:N)
    public ICollection<LinkedTourist> LinkedAsMain { get; set; } = new List<LinkedTourist>();
    public ICollection<LinkedTourist> LinkedAsSecondary { get; set; } = new List<LinkedTourist>();
}