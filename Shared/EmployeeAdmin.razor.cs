using attendance_reg.Pages;
using attendance_reg.Pages.Envoys;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace attendance_reg.Shared;

public partial class EmployeeAdmin
{
    [CascadingParameter] public IModalService? Modal { get; set; }
    [Inject] public EmployeeEnvoy EmployeeEnvoy { get; set; }
    
    private bool _displayModal = false;
    private Employee? _employee = new();
    private List<Employee>? _employees;

    protected override async Task OnInitializedAsync()
    {
        _employees = await EmployeeEnvoy.GetEmployees();
    }
    
    private async Task ShowAddEmployee()
    {
        var parameters = new ModalParameters();
        parameters.Add(nameof(EmployeeAdminModal.Employee), _employee);
        parameters.Add(nameof(EmployeeAdminModal.EmployeeEnvoy), EmployeeEnvoy);

        var modalRef = Modal?.Show<EmployeeAdminModal>("Add Employee", parameters);
        var modalResult = await modalRef?.Result!;
        
        if(modalResult.Cancelled)
            return;
        _employees = await EmployeeEnvoy.GetEmployees();
        await InvokeAsync(StateHasChanged);
    }
}