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
      var t = await JsRuntime.InvokeAsync<string>("sessionStorage.getItem", "token");

      if (!string.IsNullOrEmpty(t)) return t;
      var password = await JsRuntime.InvokeAsync<string>("prompt", "Please enter password");

      var httpClient = new HttpClient();

      var result = await httpClient.GetAsync($"https://holborn-za-attendance.netlify.app/.netlify/functions/login?credentials={password}");

      if ((int)result.StatusCode == 401)
      {
         return await Login();
      }

      var response = await result.Content.ReadAsStringAsync();
      var token = JsonSerializer.Deserialize<TokenResponse>(response);

      await JsRuntime.InvokeAsync<string>("sessionStorage.setItem", "token", token.Token);
      return token.Token;

   }
}