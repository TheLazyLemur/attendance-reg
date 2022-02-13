using System.Text.Json.Serialization;

namespace attendance_reg.Pages;

public class MeetingSignature
{
   [JsonPropertyName("meeting_id")]public string? MeetingId { get; set; }
   [JsonPropertyName("employee_id")]public int EmployeeId { get; set; }
   [JsonPropertyName("data_url")]public string DataUrl { get; set; }
   [JsonPropertyName("created_at")]public string CreatedAt { get; set; }
}