using attendance_reg.Pages;
using attendance_reg.Pages.Envoys;
using Microsoft.AspNetCore.Components;

namespace attendance_reg.Shared;

public partial class RoleAdminModal
{
    [Parameter]
    public Status? Status { get; set; }

    [Parameter]
    public StatusEnvoy? StatusEnvoy { get; set; }

    private async Task HandleValidSubmit()
    {
        if (Status != null) await StatusEnvoy?.AddStatus(Status)!;
        await InvokeAsync(StateHasChanged);
    }
}