using System.Net.Http.Headers;

namespace attendance_reg.Pages.Envoys;

public class EmailEnvoy
{
   public async Task Send(string to, string subject, string content)
   {
      var client = new HttpClient();
      var request = new HttpRequestMessage
      {
         Method = HttpMethod.Post,
         RequestUri = new Uri("https://holborn-za-attendance.netlify.app/.netlify/functions/email"),
         Content = new StringContent("{\n    \"personalizations\": [\n        {\n            \"to\": [\n                {\n                    \"email\": \"danrousseau@protonmail.com\"\n                }\n            ],\n            \"subject\": \"{asdajpasdasdasdasdasdasdasd}\"\n        }\n    ],\n    \"from\": {\n        \"email\": \"from_address@example.com\"\n    },\n    \"content\": [\n        {\n            \"type\": \"text/plain\",\n            \"value\": \"asodjnaiod adhnoasd ojwqojwi oasdjodjoqwie qo asodoiqwhr0qwpdaps dphwoqheoqw \"\n        }\n    ]\n}")
         {
            Headers =
            {
               ContentType = new MediaTypeHeaderValue("application/json")
            }
         }
      };
      using (var response = await client.SendAsync(request))
      {
         response.EnsureSuccessStatusCode();
         var body = await response.Content.ReadAsStringAsync();
         Console.WriteLine(body);
      }
   }
}