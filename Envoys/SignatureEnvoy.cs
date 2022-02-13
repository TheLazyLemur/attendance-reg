namespace attendance_reg.Pages.Envoys;

public class SignatureEnvoy
{
    private readonly SupabaseEnvoy _supabaseEnvoy;

    public SignatureEnvoy(SupabaseEnvoy supabaseEnvoy)
    {
        _supabaseEnvoy = supabaseEnvoy;
    }
    
    public async Task AddSignature(List<MeetingSignature> meetingSignature)
    {
        await _supabaseEnvoy.Post(SupabaseResources.SignatureTable, meetingSignature);
    }
}