using attendance_reg.Pages;
using attendance_reg.Pages.Envoys;
using Microsoft.AspNetCore.Components;

namespace attendance_reg.Shared;

public partial class MeetingAdminModal
{
    [Parameter] public Meeting? Meeting { get; set; }
    [Parameter] public MeetingEnvoy? MeetingEnvoy { get; set; }
    
    private async Task HandleValidSubmit()
    {
        await MeetingEnvoy?.AddStatus(Meeting)!;
        await InvokeAsync(StateHasChanged);
    }
}