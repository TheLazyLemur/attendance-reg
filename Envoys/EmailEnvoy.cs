using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace attendance_reg.Pages.Envoys;

public class To
{
   public string email { get; set; }
}

public class Personalization
{
   public List<To> to { get; set; }
   public string subject { get; set; }
}

public class From
{
   public string email { get; set; }
}

public class Content
{
   public string type { get; set; }
   public string value { get; set; }
}

public class Root
{
   public List<Personalization> personalizations { get; set; }
   public From from { get; set; }
   public List<Content> content { get; set; }
}


public class EmailEnvoy
{
   public async Task Send(string to, string subject, string content)
   {
      var email = new Root();
      email.personalizations = new List<Personalization>
      {
         new()
         {
            to = new List<To> { new To { email = to } },
            subject = subject
         }
      };

      email.from = new From {email = "testSign@demo.com"};
      email.content.Add(new Content {type = "text/plain", value = content});
      
      
      var client = new HttpClient();
      var request = new HttpRequestMessage
      {
         Method = HttpMethod.Post,
         RequestUri = new Uri("https://holborn-za-attendance.netlify.app/.netlify/functions/email"),
         Content = new StringContent(JsonSerializer.Serialize(email))
      };
      using (var response = await client.SendAsync(request))
      {
         response.EnsureSuccessStatusCode();
         var body = await response.Content.ReadAsStringAsync();
         Console.WriteLine(body);
      }
   }
}