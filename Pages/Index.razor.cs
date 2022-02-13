using attendance_reg.Pages.Envoys;
using Microsoft.AspNetCore.Components;

namespace attendance_reg.Pages;

public partial class Index
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public AppState AppState { get; set; }
    
    [Inject]
    public OfficeEnvoy? OfficeEnvoy { get; set; }
    
    private List<Office>? Offices { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Offices =  await OfficeEnvoy?.GetOffices()!;
        await InvokeAsync(StateHasChanged);
    }
}