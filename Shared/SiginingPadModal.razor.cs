using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace attendance_reg.Shared;

public partial class SiginingPadModal
{
     [Inject]
     private IJSRuntime JsRuntime { get; set; }
     
     [Parameter]
     public int EmployeeId { get; set; }
     
     [Parameter]
     public string Id { get; set; }
     
     [Parameter]
     public EventCallback<Dictionary<string, string>> SaveDataUrl { get; set; }
     
     [CascadingParameter] BlazoredModalInstance ModalInstance { get; set; }
     
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
         if (!firstRender)
              await JsRuntime.InvokeVoidAsync("loadSig", Id);
    }
     
     public async Task SaveSignature()
     {
          var result = await JsRuntime.InvokeAsync<string>("saveSig");
          
          var dict = new Dictionary<string, string>
          {
               {"employeeId", EmployeeId.ToString()},
               {"dataUrl", result}
          };

          await SaveDataUrl.InvokeAsync(dict);
          await ModalInstance.CloseAsync(ModalResult.Ok(result));

     }
}