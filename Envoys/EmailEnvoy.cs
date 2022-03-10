namespace attendance_reg.Pages.Envoys;

public class EmailEnvoy
{
    private readonly SupabaseEnvoy _supabaseEnvoy;

    public EmailEnvoy(SupabaseEnvoy supabaseEnvoy)
    {
       _supabaseEnvoy = supabaseEnvoy;
    }

    public async Task Send(string to, string subject, string content)
    {
        await _supabaseEnvoy.Post(SupabaseResources.EmailTable,
            new {to = to, subject = subject, content = content, sent = false});
    }
}