namespace TravelCRM.Application.Dtos;

/// <summary>
/// DTO для создания нового туриста через UI.
/// </summary>
public class TouristCreateDto
{
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string EngFirstName { get; set; } = string.Empty;
    public string EngLastName { get; set; } = string.Empty;
    public string PassportInfo { get; set; } = string.Empty;
    public string IntPassportInfo { get; set; } = string.Empty;
    public string VisaInfo { get; set; } = string.Empty;
}
