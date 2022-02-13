using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace attendance_reg.Pages;

public class AppState
{
   private readonly IJSRuntime _jsRuntime;

   public AppState(IJSRuntime jsRuntime)
   {
      _jsRuntime = jsRuntime;
   }
   
   public async void SaveOfficeId(string OfficeId)
   {
       await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "OfficeId", OfficeId);
   }
   
   public async Task<string> GetOfficeId()
   {
       return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "OfficeId");
   }
}