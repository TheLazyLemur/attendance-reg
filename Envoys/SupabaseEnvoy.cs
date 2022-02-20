using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.Toast.Services;

namespace attendance_reg.Pages.Envoys;

public class SupabaseEnvoy
{
    private readonly AuthenticationEnvoy _authenticationEnvoy;
    private readonly IToastService _toastService;
    private static Dictionary<string, string> _cachedTokens = new();

    public SupabaseEnvoy(AuthenticationEnvoy authenticationEnvoy, IToastService toastService)
    {
        _authenticationEnvoy = authenticationEnvoy;
        _toastService = toastService;
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

        try
        {
            var b = _cachedTokens.TryGetValue(resource+query, out var cachedToken);
            if (b && cachedToken != null)
            {
                var employees = JsonSerializer.Deserialize<T>(cachedToken);
                Console.WriteLine("Using Cache");
                return employees;
            }
            else
            {
                using var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var employees = JsonSerializer.Deserialize<T>(body);
                _cachedTokens.TryAdd(resource+query, body);
                Console.WriteLine("Not Using Cache");
                return employees;
            }
        }
        catch (Exception e)
        {
            _toastService.ShowError($"failed to fetch {resource}.");
        }

        return default;
   }
   
   public async Task Delete (string resource, string query)
   {
        var token = await _authenticationEnvoy.Login();
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"{SupabaseResources.SupabaseUrl}/{resource}{query}"),
            Headers =
            {
                { "apikey", token },
                { "Authorization", "Bearer " + token },
            },
        };

        try
        {
            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            _toastService.ShowSuccess($"Deleted {resource} with query of {query}");
            _cachedTokens = new Dictionary<string, string?>();
        }
        catch (Exception e)
        {
            _toastService.ShowError($"failed to delete {resource}.");
        }
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
            _toastService.ShowSuccess($"Created new {resource}");
            _cachedTokens = new Dictionary<string, string?>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine(jsonPayload);
            _toastService.ShowError($"failed to create new {resource}.");
        }
   }
}