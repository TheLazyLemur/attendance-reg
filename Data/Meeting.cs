using System.Text.Json.Serialization;

namespace attendance_reg.Pages;

public class Meeting
{
    [JsonPropertyName("id")] public int? Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("topic")] public string? Topic { get; set; }
    [JsonPropertyName("speaker")] public string? Speaker { get; set; }
    [JsonPropertyName("office_id")] public int OfficeId { get; set; }
    [JsonPropertyName("meeting_date")] public DateTime MeetingDate { get; set; }
}