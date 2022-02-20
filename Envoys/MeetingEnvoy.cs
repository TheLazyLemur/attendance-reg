namespace attendance_reg.Pages.Envoys;

public class MeetingEnvoy
{
    private readonly SupabaseEnvoy _supabaseEnvoy;
    private readonly AppState _appState;

    public MeetingEnvoy(SupabaseEnvoy supabaseEnvoy, AppState appState)
    {
        _supabaseEnvoy = supabaseEnvoy;
        _appState = appState;
    }
    
    public async Task<List<Meeting>?> GetMeetings()
    {
        return await _supabaseEnvoy.Get<List<Meeting>>(SupabaseResources.MeetingTable, $"?select=*&office_id=eq.{int.Parse(await _appState.GetOfficeId())}");
    }
    
    public async Task DeleteMeeting(int meetingId)
    {
        await _supabaseEnvoy.Delete(SupabaseResources.MeetingTable, $"?id=eq.{meetingId}");
    }
    
    public async Task<List<Meeting>?> GetMeeting(string? id)
    {
        return await _supabaseEnvoy.Get<List<Meeting>>(SupabaseResources.MeetingTable, $"?id=eq.{id}&select=*&office_id=eq.{int.Parse(await _appState.GetOfficeId())}");
    }

    public async Task AddMeeting(Meeting? meeting)
    {
        meeting.OfficeId = int.Parse(await _appState.GetOfficeId());
        await _supabaseEnvoy.Post(SupabaseResources.MeetingTable, meeting);
    }
    
    public async Task<List<AttendanceRecord>?> GetAttendanceRegister()
    {
        return await _supabaseEnvoy.Get<List<AttendanceRecord>>(SupabaseResources.AttendanceTable, "?select=*");
    }
    
    public async Task<List<AttendanceRecord>?> GetAttendanceRegister(string filter)
    {
        return await _supabaseEnvoy.Get<List<AttendanceRecord>>(SupabaseResources.AttendanceTable, $"?select=*&{filter}");
    }
    
    public async Task SendAttendanceRegister(List<AttendanceRecord> attendanceRecords)
    {
        attendanceRecords.ForEach(async it =>
        {
            it.OfficeId = int.Parse(await _appState.GetOfficeId());
        });
        
        await _supabaseEnvoy.Post(SupabaseResources.AttendanceTable, attendanceRecords);
    }
}