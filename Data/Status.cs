using System.Text.Json.Serialization;

namespace attendance_reg.Pages;

public class Status
{
    [JsonPropertyName("name")] public string Name { get; set; }
}