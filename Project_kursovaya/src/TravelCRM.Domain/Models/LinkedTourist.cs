namespace TravelCRM.Domain.Models;

public class LinkedTourist
{
    public int MainTouristId { get; set; }
    public Tourist? MainTourist { get; set; }

    public int LinkedTouristId { get; set; }
    public Tourist? LinkedTouristRef { get; set; } // Переименовали свойство, чтобы не конфликтовало с именем класса

    public string RelationType { get; set; } = string.Empty;
}