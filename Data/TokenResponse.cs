using System.Text.Json.Serialization;

namespace attendance_reg.Pages;

public class TokenResponse
{
    [JsonPropertyName("token")] public string? Token { get; set; }
}