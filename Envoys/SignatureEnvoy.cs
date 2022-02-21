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
    
    public async Task<List<MeetingSignature>?> GetSignature()
    {
       return await _supabaseEnvoy.Get<List<MeetingSignature>>(SupabaseResources.SignatureTable, "?select=*");
    }
    
    public async Task<List<MeetingSignature>?> GetSignatureImage(string filter)
    {
      var toReturn =  await _supabaseEnvoy.Get<List<MeetingSignature>>(SupabaseResources.SignatureTable, $"?select=data_url&{filter}");
      toReturn = new List<MeetingSignature>()
      {
          toReturn.MaxBy(it => it.Id)
      }; 
      return toReturn;
    }
}