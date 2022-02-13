namespace attendance_reg.Pages.Envoys;

public class StatusEnvoy
{
    private readonly SupabaseEnvoy _supabaseEnvoy;

    public StatusEnvoy(SupabaseEnvoy supabaseEnvoy)
    {
        _supabaseEnvoy = supabaseEnvoy;
    }
    
    public async Task<List<Status>?> GetStatuses()
    {
        
        return (await _supabaseEnvoy.Get<List<Status?>>(SupabaseResources.Status, "?select=*"))!;
    }

    public async Task AddStatus(Status employeeEnvoy)
    {
        await _supabaseEnvoy.Post(SupabaseResources.Status, employeeEnvoy);
    }
}