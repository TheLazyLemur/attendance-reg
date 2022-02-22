using attendance_reg.Pages.Envoys;
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
    
    private bool _assetManagementPresentation;
    private bool _productTraining;
    private bool _internalTraining;

    private List<Employee>? _employees = new();
    private List<Status>? _statuses = new();

    private readonly Dictionary<int, MeetingSignature> _signatures = new();
    private readonly Dictionary<int, AttendanceRecord> _attendance = new();
    private readonly Dictionary<int, string?> _statusMap = new();

    private List<Meeting>? _meeting;

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
            _meeting = await MeetingEnvoy.GetMeeting(Id);
            _speaker = _meeting?[0].Speaker;
            _topic = _meeting?[0].Topic;
            _company = _meeting?[0].Company;
            _productTraining = _meeting?[0].ProductTraining ?? false;
            _internalTraining = _meeting?[0].InternalTraining ?? false;
            _assetManagementPresentation = _meeting?[0].AssetManagementPresentation ?? false;

            _date = _meeting?[0].MeetingDate.Date.ToString("MM/dd/yyyy");
            await InvokeAsync(StateHasChanged);
        });
         
        LoadStatusMap();
        StateHasChanged();
    }
    
    private async void LoadStatusMap()
    {
        if(MeetingEnvoy is null) return;

        var x = await MeetingEnvoy.GetAttendanceRegister();

        x?.Where(it => Id != null && it.MeetingId == int.Parse(Id)).ToList().ForEach(it =>
        {
            var couldAdd = it.Status != null && _statusMap.TryAdd(it.EmployeeId, it.Status);
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

    private void SendToServer()
    {
        if (MeetingEnvoy is null || SignatureEnvoy is null) return;

        if (_meeting != null)
        {
            var m = _meeting.FirstOrDefault();
            if (m is null) return;
           
            m.Topic = _topic;
            m.Speaker = _speaker;
            m.Company = _company;
            
            m.ProductTraining = _productTraining;
            m.InternalTraining = _internalTraining;
            m.AssetManagementPresentation = _assetManagementPresentation;

            Console.WriteLine(m.ProductTraining);
            Console.WriteLine(m.InternalTraining);
            Console.WriteLine(m.AssetManagementPresentation);
        }

        Task.Run(async () =>
        {
            if (_meeting != null) await MeetingEnvoy.UpdateMeeting(_meeting.FirstOrDefault());
        });
        
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
    }
}


