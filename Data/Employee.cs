using System.Text.Json.Serialization;

namespace attendance_reg.Pages;

public class Employee
{
    [JsonPropertyName("id")]public int? Id { get; set; }
    [JsonPropertyName("first_name")]public string Name { get; set; }
    [JsonPropertyName("surname")]public string Surname { get; set; }
    [JsonPropertyName("title")]public string Title { get; set; }
    [JsonPropertyName("office_id")]public int? OfficeId { get; set; }

    public string GetInitials()
    {
        return Name[..1] + Surname[..1];
    }
}