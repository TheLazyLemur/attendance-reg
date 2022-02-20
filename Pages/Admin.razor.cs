using Microsoft.AspNetCore.Components;

namespace attendance_reg.Pages;

public partial class Admin
{
    [Inject] public Envoys.EmployeeEnvoy _emplyeeEnvoy { get; set; }
    
    private enum PageStatus
    {
        Employee,
        Status,
        Meeting
    }
    
    private PageStatus _pageStatus = PageStatus.Employee;
}