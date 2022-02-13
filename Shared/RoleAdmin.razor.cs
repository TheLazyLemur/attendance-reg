using attendance_reg.Pages;
using attendance_reg.Pages.Envoys;
using Microsoft.AspNetCore.Components;

namespace attendance_reg.Shared;

public partial class RoleAdmin
{
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
}