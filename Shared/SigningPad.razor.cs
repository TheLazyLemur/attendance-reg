using attendance_reg.Pages.Envoys;
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
    public string MeetingId { get; set; }
    
    [Parameter]
    public int EmployeeId { get; set; }
    
    [Parameter]
    public EventCallback<Dictionary<string, string>> SaveDataUrl { get; set; }
    
    [Inject]
    private SignatureEnvoy SignatureEnvoy { get; set; }

    private string src { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var sigs = await SignatureEnvoy.GetSignatureImage($"" +
                                                          $"meeting_id=eq.{MeetingId}" +
                                                          $"&employee_id=eq.{EmployeeId}");
        var sig = sigs.MaxBy(it => it.Id);
        src = sig != null ? sig.DataUrl : "https://via.placeholder.com/150?text=";
    }

    private async Task ShowSigModal()
    {
        var parameters = new ModalParameters();
        parameters.Add(nameof(SiginingPadModal.Id), Id);
        parameters.Add(nameof(SiginingPadModal.EmployeeId), EmployeeId);
        parameters.Add(nameof(SiginingPadModal.SaveDataUrl), SaveDataUrl);
        
        var modalRef = Modal?.Show<SiginingPadModal>("Add Status", parameters, new ModalOptions {HideHeader = true});
        var modalResult = await modalRef?.Result!;
        
        if(modalResult.Cancelled)
            return;

        var modalData = modalResult.Data as string;
        src = modalData;
        StateHasChanged();
    }

}