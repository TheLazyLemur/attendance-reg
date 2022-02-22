using System.Text.Json;
using System.Text.Json.Serialization;

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
   public async Task Send(string to, string subject, string content)
   {
      var email = new Root
      {
         Personalizations = new List<Personalization>
         {
            new()
            {
               To = new List<To> { new() { Email = to } },
               Subject = subject
            }
         },
         From = new From {Email = "hr.sa@holbornassets.com"},
         Content = new List<Content>
         {
            new()
            {
               Type = "text/html",
               Value = content
            }
         }
      };

      var client = new HttpClient();
      var request = new HttpRequestMessage
      {
         Method = HttpMethod.Post,
         RequestUri = new Uri("https://holborn-za-attendance.netlify.app/.netlify/functions/email"),
         Content = new StringContent(JsonSerializer.Serialize(email))
      };
      
      using var response = await client.SendAsync(request);
      response.EnsureSuccessStatusCode();
   }
}