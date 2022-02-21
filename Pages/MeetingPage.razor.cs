using System.Globalization;
using attendance_reg.Pages.Envoys;
using attendance_reg.Shared;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace attendance_reg.Pages;

public partial class MeetingPage
{
    [Parameter] public string? Id { get; set; }
    [Inject] public EmployeeEnvoy? EmployeeEnvoy { get; set; }
    [Inject] public StatusEnvoy? StatusEnvoy { get; set; }
    [Inject] public MeetingEnvoy? MeetingEnvoy { get; set; }
    [Inject] public SignatureEnvoy? SignatureEnvoy { get; set; }
    [CascadingParameter] public IModalService? Modal { get; set; }

    private string? _speaker;
    private string? _company;
    private string? _topic;
    private string? _date;

    private List<Employee>? _employees = new();
    private List<Status>? _statuses = new();

    private readonly Dictionary<int, MeetingSignature> _signatures = new();
    private readonly Dictionary<int, AttendanceRecord> _attendance = new();
    private readonly Dictionary<int, string> _statusMap = new();

    private List<Meeting> meeting;

    protected override void OnInitialized()
    {
        if (MeetingEnvoy is null || EmployeeEnvoy is null || StatusEnvoy is null || SignatureEnvoy is null)
            return;

        Task.Run(async () =>
        {
            _employees = await EmployeeEnvoy.GetEmployees();
            await InvokeAsync(StateHasChanged);
        });
        
        Task.Run(async () =>
        {
            _statuses = await StatusEnvoy.GetStatuses();
            await InvokeAsync(StateHasChanged);
        });
        
        Task.Run(async () =>
        {
            meeting = await MeetingEnvoy.GetMeeting(Id);
            _speaker = meeting?[0].Speaker;
            _topic = meeting?[0].Topic;
            _company = meeting?[0].Company;
            _date = meeting?[0].MeetingDate.Date.ToString("MM/dd/yyyy");
            await InvokeAsync(StateHasChanged);
        });
         
        LoadStatusMap();
        StateHasChanged();
    }
    
    private async void LoadStatusMap()
    {
        var x = await MeetingEnvoy.GetAttendanceRegister();
        x.Where(it => it.MeetingId == int.Parse(Id)).ToList().ForEach(it =>
        {
            var couldAdd = _statusMap.TryAdd(it.EmployeeId, it.Status);
            if (!couldAdd)
                _statusMap[it.EmployeeId] = it.Status;
            Console.WriteLine(it.Status);
        });
    }
    
    private void UpdateStatus(Tuple<int?, string?> tuple)
    {
        var (employeeId, status) = tuple;
        
        if (status is null || employeeId is null) return;
        var couldAdd = _statusMap.TryAdd(employeeId.Value, status); 
        if(!couldAdd)
            _statusMap[employeeId.Value] = status;
    }
    
    private Task Save(Dictionary<string, string> values)
    {
        if (Id is null) return Task.CompletedTask;

        var employeeId = int.Parse(values["employeeId"]);
        
        _signatures.TryAdd(employeeId, new MeetingSignature
        {
            DataUrl = values["dataUrl"],
            MeetingId = int.Parse(Id),
            EmployeeId = int.Parse(values["employeeId"])
        });
        

        _attendance.TryAdd(employeeId, new AttendanceRecord
        {
            MeetingId = int.Parse(Id),
            EmployeeId = int.Parse(values["employeeId"]),
            DateCreated = DateTime.Now
        });
        
        return Task.CompletedTask;
    }

    private async Task SendToServer()
    {
        if (MeetingEnvoy is null || SignatureEnvoy is null) return;

        meeting.FirstOrDefault().Topic = _topic;
        meeting.FirstOrDefault().Speaker = _speaker;
        meeting.FirstOrDefault().Company = _company;
        
        await MeetingEnvoy.UpdateMeeting(meeting.FirstOrDefault());
        
        _attendance.Values.ToList().ForEach(it =>
        {
            var couldGet = _statusMap.TryGetValue(it.EmployeeId, out var s);
            it.Status = couldGet ? s : "Office";
        });

        Task.Run(async () =>
        {
            await MeetingEnvoy.SendAttendanceRegister(_attendance.Values.ToList());
        });
        
        Task.Run(async () =>
        {
            await SignatureEnvoy.AddSignature(_signatures.Values.ToList());
        });
        
        return;
    }
    
    private async Task ShowNotes()
    {
        var parameters = new ModalParameters();

        var modalRef = Modal?.Show<MyTestPage>("Add Notes", parameters, new ModalOptions {HideHeader = true});
        var modalResult = await modalRef?.Result!;
        
        if(modalResult.Cancelled)
            return;
        await InvokeAsync(StateHasChanged);
    }
    
    
}


