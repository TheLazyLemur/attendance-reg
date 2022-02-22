namespace attendance_reg.Pages.Envoys;

public class OfficeEnvoy
{
    private readonly SupabaseEnvoy _supabaseEnvoy;

    public OfficeEnvoy(SupabaseEnvoy supabaseEnvoy)
    {
        _supabaseEnvoy = supabaseEnvoy;
    } 
    
    public async Task<List<Office>?> GetOffices()
    {
        var result = await _supabaseEnvoy.Get<List<Office>>(SupabaseResources.OfficeTable, "?select=*");
        return result;
    }
}