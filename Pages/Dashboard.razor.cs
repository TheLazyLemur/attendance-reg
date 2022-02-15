using attendance_reg.Pages.Envoys;
using attendance_reg.Shared;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace attendance_reg.Pages;

public partial class Dashboard
{
    [CascadingParameter] public IModalService? Modal { get; set; }
    
    [Inject]
    public MeetingEnvoy? MeetingEnvoy { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    private List<Meeting>? Meetings { get; set; }
    private bool _displayModal = false;
    private readonly Meeting? _meeting = new();

    protected override async Task OnInitializedAsync()
    {
        Meetings = await MeetingEnvoy?.GetMeetings()!;
    }
    
    async Task ShowAddMeeting()
    {
        var parameters = new ModalParameters();
        parameters.Add(nameof(MeetingAdminModal.MeetingEnvoy), MeetingEnvoy);
        parameters.Add(nameof(MeetingAdminModal.Meeting), _meeting);

        var modalRef = Modal?.Show<MeetingAdminModal>("Add Meeting", parameters);
        var modalResult = await modalRef?.Result!;
        
        if(modalResult.Cancelled)
            return;
        
        Meetings = await MeetingEnvoy?.GetMeetings()!;
        StateHasChanged();
    }
}