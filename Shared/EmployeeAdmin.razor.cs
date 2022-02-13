using attendance_reg.Pages;
using attendance_reg.Pages.Envoys;
using Microsoft.AspNetCore.Components;

namespace attendance_reg.Shared;

public partial class EmployeeAdmin
{
    [Inject] public EmployeeEnvoy EmployeeEnvoy { get; set; }
    
    private bool _displayModal = false;
    private Employee _employee = new();

    private List<Employee>? _employees;

    protected override async Task OnInitializedAsync()
    {
        _employees = await EmployeeEnvoy.GetEmployees();
    }
    
    private async Task HandleValidSubmit()
    {
        await EmployeeEnvoy.AddEmployee(_employee);
        _employees = await EmployeeEnvoy.GetEmployees();
        await InvokeAsync(StateHasChanged);
    }
}