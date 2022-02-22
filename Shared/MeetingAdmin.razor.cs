using attendance_reg.Pages;
using attendance_reg.Pages.Envoys;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace attendance_reg.Shared;

public partial class MeetingAdmin
{
    [Inject] public MeetingEnvoy? MeetingEnvoy { get; set; }
    [CascadingParameter] public IModalService? Modal { get; set; }
    
    private readonly Meeting? _meeting = new();

    private List<Meeting>? _meetings;

    protected override async Task OnInitializedAsync()
    {
      _meetings = await MeetingEnvoy?.GetMeetings()!;
    }

    async Task ShowAddMeeting()
    {
        var parameters = new ModalParameters();
        parameters.Add(nameof(MeetingAdminModal.MeetingEnvoy), MeetingEnvoy);
        parameters.Add(nameof(MeetingAdminModal.Meeting), _meeting);

        var modalRef = Modal?.Show<MeetingAdminModal>("Add Meeting", parameters, new ModalOptions {HideHeader = true});
        var modalResult = await modalRef?.Result!;
        
        if(modalResult.Cancelled)
            return;
        
        _meetings = await MeetingEnvoy?.GetMeetings()!;
        StateHasChanged();
    }
}