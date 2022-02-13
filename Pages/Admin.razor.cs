using Microsoft.AspNetCore.Components;

namespace attendance_reg.Pages;

public partial class Admin
{
    [Inject] public Envoys.EmployeeEnvoy _emplyeeEnvoy { get; set; }
    
    private bool displayModal = false;
    private Employee _employee = new();

    private List<Employee>? _employees;

    protected override async Task OnInitializedAsync()
    {
        _employees = await _emplyeeEnvoy.GetEmployees();
    }
    
    private async Task HandleValidSubmit()
    {
        await _emplyeeEnvoy.AddEmployee(_employee);
        _employees = await _emplyeeEnvoy.GetEmployees();
        await InvokeAsync(StateHasChanged);
    }

}