using attendance_reg.Pages;
using attendance_reg.Pages.Envoys;
using Microsoft.AspNetCore.Components;

namespace attendance_reg.Shared;

public partial class EmployeeAdminModal
{
    [Parameter]
    public EmployeeEnvoy? EmployeeEnvoy { get; set; }

    [Parameter]
    public Employee? Employee { get; set; }

    private async Task HandleValidSubmit()
    {
        await EmployeeEnvoy?.AddEmployee(Employee)!;
    }
}