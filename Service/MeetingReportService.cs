namespace attendance_reg.Pages.Envoys;

    public class FinalReport
    {
        public Employee? Employee { get; }
        public MeetingSignature? Signature { get; }
        public AttendanceRecord Attendance { get; }

        public FinalReport(Employee? employee, MeetingSignature? signature, AttendanceRecord attendance)
        {
            Employee = employee;
            Signature = signature;
            Attendance = attendance;
        }
    }

public class MeetingReportService
{
    private readonly MeetingEnvoy _meetingEnvoy;
    private readonly EmployeeEnvoy _employeeEnvoy;
    private readonly SignatureEnvoy _signatureEnvoy;

    public MeetingReportService(MeetingEnvoy meetingEnvoy, EmployeeEnvoy employeeEnvoy, SignatureEnvoy signatureEnvoy)
    {
        _meetingEnvoy = meetingEnvoy;
        _employeeEnvoy = employeeEnvoy;
        _signatureEnvoy = signatureEnvoy;
    }

    public async Task<List<FinalReport>?> GenerateReport(int meetingId)
    {
        var attendanceRecords = await _meetingEnvoy.GetAttendanceRegister();
        var employees = await _employeeEnvoy.GetEmployees();
        var signatures = await _signatureEnvoy.GetSignature();

        var recs = attendanceRecords?
            .OrderByDescending(it => it.Id)
            .Where(it => it.MeetingId == meetingId)
            .ToList();
        
        var sigs = recs?.Select(it => signatures?
            .OrderByDescending(sig => sig.Id)
            .FirstOrDefault(s => s.EmployeeId == it.EmployeeId && s.MeetingId == it.MeetingId))
            .ToList();
        
        var emps = recs?
            .Select(it => employees?
                .FirstOrDefault(e => e.Id == it.EmployeeId))
            .ToList();
        
        var finalReport = recs?
            .Select(it => new FinalReport(emps?.FirstOrDefault(e => e?.Id == it.EmployeeId), sigs!
                .FirstOrDefault(s => s?.EmployeeId == it.EmployeeId), it))
            .ToList();

        return finalReport;
    }
}