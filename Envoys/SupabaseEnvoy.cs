using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace attendance_reg.Pages.Envoys;

public class SupabaseEnvoy
{
    private readonly AuthenticationEnvoy _authenticationEnvoy;

    public SupabaseEnvoy(AuthenticationEnvoy authenticationEnvoy)
    {
        _authenticationEnvoy = authenticationEnvoy;
    }

   public async Task<T?> Get<T>(string resource, string query)
   {
        var token = await _authenticationEnvoy.Login();
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{SupabaseResources.SupabaseUrl}/{resource}{query}"),
            Headers =
            {
                { "apikey", token },
                { "Authorization", "Bearer " + token },
            },
        };
        using var response = await client.SendAsync(request);
        
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        var employees = JsonSerializer.Deserialize<T>(body);

        return employees;
   }
   
   public async Task Post<T>(string resource, T payload)
   {
        var token = await _authenticationEnvoy.Login();
        var jsonPayload = JsonSerializer.Serialize(payload, new JsonSerializerOptions {DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull});
        
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{SupabaseResources.SupabaseUrl}/{resource}"),
            Headers =
            {
                { "apikey", token },
                { "Authorization", "Bearer " + token },
                { "Prefer", "return=representation" },
            },
            Content = new StringContent(jsonPayload)
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
            }
        };
        using var response = await client.SendAsync(request);
        try
        {
            Console.WriteLine("Uploading to Supabase");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine(jsonPayload);
        }
   }
   
}