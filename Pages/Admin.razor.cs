namespace attendance_reg.Pages;

public partial class Admin
{
    private enum PageStatus
    {
        Employee,
        Status,
        Meeting
    }
    
    private PageStatus _pageStatus = PageStatus.Employee;
}