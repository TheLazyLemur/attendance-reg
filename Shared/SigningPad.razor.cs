using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace attendance_reg.Shared;
public partial class SigningPad
{
     [Inject]
     private IJSRuntime JsRuntime { get; set; }
    
    [CascadingParameter]
    public IModalService? Modal { get; set; }

    [Parameter]
    public string Id { get; set; }

    [Parameter]
    public int EmployeeId { get; set; }

    [Parameter]
    public EventCallback<Dictionary<string, string>> SaveDataUrl { get; set; }

    private string src { get; set; }

    private async Task ShowSigModal()
    {
        var parameters = new ModalParameters();
        parameters.Add(nameof(SiginingPadModal.Id), Id);
        parameters.Add(nameof(SiginingPadModal.EmployeeId), EmployeeId);
        parameters.Add(nameof(SiginingPadModal.SaveDataUrl), SaveDataUrl);
        
        var modalRef = Modal?.Show<SiginingPadModal>("Add Status", parameters);
        var modalResult = await modalRef?.Result!;
        
        if(modalResult.Cancelled)
            return;

        var modalData = modalResult.Data as string;
        src = modalData;
        StateHasChanged();
    }

}