using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace attendance_reg.Shared;

public partial class SiginingPadModal : IDisposable
{
     [Inject]
     private IJSRuntime? JsRuntime { get; set; }
     
     [Parameter]
     public int EmployeeId { get; set; }
     
     [Parameter]
     public string? Id { get; set; }
     
     [Parameter]
     public EventCallback<Dictionary<string, string>> SaveDataUrl { get; set; }
     
     [CascadingParameter] BlazoredModalInstance? ModalInstance { get; set; }
     
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
         if (firstRender)
         {
              try
              {
                   if(JsRuntime is not null)
                        await JsRuntime.InvokeVoidAsync("loadSig", Id);
              }
              catch (Exception e)
              {
                   Console.WriteLine("Error: " + e.Message);
              }
         }
    }

    private async Task SaveSignature()
    { 
         if(ModalInstance is null || JsRuntime is null) return;
         
         var result = await JsRuntime.InvokeAsync<string>("saveSig", Id);

         var dict = new Dictionary<string, string>
         {
              {"employeeId", EmployeeId.ToString()},
              {"dataUrl", result}
         };

         await SaveDataUrl.InvokeAsync(dict);
         await ModalInstance.CloseAsync(ModalResult.Ok(result));
    }

    public async void Dispose()
    {
         if (JsRuntime is not null)
              await JsRuntime.InvokeVoidAsync("unbindSig", Id);
    }
}