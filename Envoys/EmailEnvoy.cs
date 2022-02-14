namespace attendance_reg.Pages.Envoys;

public class EmailEnvoy
{
   public async Task Send(string to, string subject, string content)
   {
      var httpClient = new HttpClient();

      var result = await httpClient.GetAsync(new Uri($"https://holborn-za-attendance.netlify.app/.netlify/functions/email?to={to}&subject={subject}&content={content}"));

      result.EnsureSuccessStatusCode();
      
      var response = await result.Content.ReadAsStringAsync();
   }
}