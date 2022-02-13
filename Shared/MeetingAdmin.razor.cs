using attendance_reg.Pages;
using attendance_reg.Pages.Envoys;
using Microsoft.AspNetCore.Components;

namespace attendance_reg.Shared;

public partial class MeetingAdmin
{
    [Inject] public MeetingEnvoy? MeetingEnvoy { get; set; }
    
    private bool _displayModal = false;
    private readonly Meeting _meeting = new();

    private List<Meeting>? _meetings;

    protected override async Task OnInitializedAsync()
    {
      _meetings = await MeetingEnvoy?.GetMeetings()!;
    }
    
    private async Task HandleValidSubmit()
    {
        await MeetingEnvoy?.AddStatus(_meeting)!;
        _meetings = await MeetingEnvoy.GetMeetings();
        await InvokeAsync(StateHasChanged);
    }
}