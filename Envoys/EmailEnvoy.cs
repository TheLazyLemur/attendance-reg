using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace attendance_reg.Pages.Envoys;

public class To
{
   [JsonPropertyName("email")] public string? Email { get; set; }
}

public class Personalization
{
   [JsonPropertyName("to")] public List<To>? To { get; set; }
   [JsonPropertyName("subject")] public string? Subject { get; set; }
}

public class From
{
   [JsonPropertyName("email")] public string? Email { get; set; }
}

public class Content
{
   [JsonPropertyName("type")] public string? Type { get; set; }
   [JsonPropertyName("value")] public string? Value { get; set; }
}

public class Root
{
   [JsonPropertyName("personalizations")] public List<Personalization>? Personalizations { get; set; }
   [JsonPropertyName("from")] public From? From { get; set; }
   [JsonPropertyName("content")] public List<Content>? Content { get; set; }
}


public class EmailEnvoy
{
    private readonly SupabaseEnvoy _supabaseEnvoy;
    private readonly AppState _appState;

    public EmailEnvoy(SupabaseEnvoy supabaseEnvoy, AppState appState)
    {
       _supabaseEnvoy = supabaseEnvoy;
       _appState = appState;
    }

    public async Task Save(string to, string subject, string content)
    {
        await _supabaseEnvoy.Post(SupabaseResources.EmailTable, new {to =to,subject = subject,  content = content, sent = false});
    }

    public async Task Send(string to, string subject, string content)
    {
       await Save(to, subject, content);
   }
}