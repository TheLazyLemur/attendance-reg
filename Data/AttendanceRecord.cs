using System.Text.Json.Serialization;

namespace attendance_reg.Pages;

public class AttendanceRecord
{
    [JsonPropertyName("employee_id")] public int EmployeeId { get; set; }
    [JsonPropertyName("meeting_id")] public int MeetingId { get; set; }
    [JsonPropertyName("office_id")] public int OfficeId { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }  
    [JsonPropertyName("date_created")] public DateTime DateCreated { get; set; }  
}
