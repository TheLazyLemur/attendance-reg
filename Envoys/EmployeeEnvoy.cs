namespace attendance_reg.Pages.Envoys;

public class EmployeeEnvoy
{
    private readonly SupabaseEnvoy _supabaseEnvoy;
    private readonly AppState _appState;


    public EmployeeEnvoy(SupabaseEnvoy supabaseEnvoy, AppState appState)
    {
        _supabaseEnvoy = supabaseEnvoy;
        _appState = appState;
    }
    
    public async Task<List<Employee>?> GetEmployees()
    {
        return await _supabaseEnvoy.Get<List<Employee>>(SupabaseResources.EmployeeTable, $"?select=*&office_id=eq.{int.Parse(await _appState.GetOfficeId())}");
    }
    
    public async Task DeleteEmployee(int employeeId)
    {
        await _supabaseEnvoy.Delete(SupabaseResources.EmployeeTable, $"?id=eq.{employeeId}");
    }

    public async Task AddEmployee(Employee? employeeEnvoy)
    {
        employeeEnvoy.OfficeId = int.Parse(await _appState.GetOfficeId());
        await _supabaseEnvoy.Post(SupabaseResources.EmployeeTable, employeeEnvoy);
    }
}