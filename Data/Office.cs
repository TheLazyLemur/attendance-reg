using System.Text.Json.Serialization;

namespace attendance_reg.Pages;

public class Office
{
   [JsonPropertyName("id")] public int Id { get; set; }
   [JsonPropertyName("name")] public string? Name { get; set; } 
}