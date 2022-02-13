using System.Text.Json;
using Microsoft.JSInterop;

namespace attendance_reg.Pages.Envoys;

public class AuthenticationEnvoy
{
   public readonly IJSRuntime JsRuntime;

   public AuthenticationEnvoy(IJSRuntime jsRuntime)
   {
      JsRuntime = jsRuntime;
   }

   public async Task<string> Login()
   {
      var t = await JsRuntime.InvokeAsync<string>("localStorage.getItem", "token");

      if (!string.IsNullOrEmpty(t)) return t;

      var httpClient = new HttpClient();

      var result = await httpClient.PostAsync("http://localhost:8000/login",
         new StringContent("daniel"));

      var response = await result.Content.ReadAsStringAsync();
      var token = JsonSerializer.Deserialize<TokenResponse>(response);

      await JsRuntime.InvokeAsync<string>("localStorage.setItem", "token", token.Token);
      return token.Token;

   }
}