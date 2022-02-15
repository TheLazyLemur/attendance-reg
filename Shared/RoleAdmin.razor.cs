using attendance_reg.Pages;
using attendance_reg.Pages.Envoys;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace attendance_reg.Shared;

public partial class RoleAdmin
{
    [CascadingParameter] public IModalService? Modal { get; set; }
    [Inject] public StatusEnvoy StatusEnvoy { get; set; }
    
    private bool _displayModal = false;
    private Status _status = new();

    private List<Status>? _statuses;

    protected override async Task OnInitializedAsync()
    {
      _statuses = await StatusEnvoy.GetStatuses();
    }
    
    private async Task HandleValidSubmit()
    {
        await StatusEnvoy.AddStatus(_status);
        _statuses = await StatusEnvoy.GetStatuses();
        await InvokeAsync(StateHasChanged);
    }
    async Task ShowAddRole()
    {
        var parameters = new ModalParameters();
        parameters.Add(nameof(RoleAdminModal.Status), _status);
        parameters.Add(nameof(RoleAdminModal.StatusEnvoy), StatusEnvoy);

        var modalRef = Modal?.Show<RoleAdminModal>("Add Status", parameters);
        var modalResult = await modalRef?.Result!;
        
        if(modalResult.Cancelled)
            return;

        _statuses = await StatusEnvoy.GetStatuses();
        StateHasChanged();
    }
}