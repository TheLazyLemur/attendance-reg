namespace attendance_reg.Pages.Envoys;

public class EmailEnvoy
{
   public async Task Send()
   {
      var httpClient = new HttpClient();

      var result = await httpClient.GetAsync("https://holborn-za-attendance.netlify.app/.netlify/functions/email?to=danielrousseau@protonmail.com&subject=testing123&content=hello,World123456");

      result.EnsureSuccessStatusCode();
      
      var response = await result.Content.ReadAsStringAsync();
   }
}