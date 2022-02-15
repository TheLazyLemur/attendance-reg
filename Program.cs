using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using attendance_reg;
using attendance_reg.Pages;
using attendance_reg.Pages.Envoys;
using Blazored.Modal;
using Blazored.Toast;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddScoped<AuthenticationEnvoy>();
builder.Services.AddScoped<EmailEnvoy>();
builder.Services.AddScoped<SupabaseEnvoy>();
builder.Services.AddScoped<EmployeeEnvoy>();
builder.Services.AddScoped<StatusEnvoy>();
builder.Services.AddScoped<MeetingEnvoy>();
builder.Services.AddScoped<SignatureEnvoy>();
builder.Services.AddScoped<OfficeEnvoy>();
builder.Services.AddScoped<AppState>();
builder.Services.AddScoped<MeetingReportService>();
builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredToast();


await builder.Build().RunAsync();
