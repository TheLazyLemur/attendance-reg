using System.Text;
using attendance_reg.Pages.Envoys;
using attendance_reg.Shared;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace attendance_reg.Pages;

public partial class TestReport
{
    [Parameter]
    public string? MeetingId { get; set; }

    [CascadingParameter]
    public IModalService? Modal { get; set; }

    [Inject] private MeetingReportService MeetingReportService { get; set; }
    [Inject] private MeetingEnvoy MeetingEnvoy { get; set; }
    [Inject] private EmailEnvoy EmailEnvoy { get; set; }

    private List<FinalReport>? FinalReports { get; set; }
    private List<Meeting>? Meeting { get; set; }


    protected override void OnInitialized()
    {
        Task.Run(async () =>
        {
            Meeting = await MeetingEnvoy.GetMeeting(MeetingId);
            await InvokeAsync(StateHasChanged);
        });

        Task.Run(async () =>
        {
            FinalReports = await MeetingReportService.GenerateReport(int.Parse(MeetingId));
            FinalReports = FinalReports.DistinctBy(it => it.Employee.Id).ToList();
            await InvokeAsync(StateHasChanged);
        });
    }

    private async Task EmailReport()
    {
        var parameters = new ModalParameters();

        var modalRef = Modal?.Show<EmailModal>("Enter Email", parameters);
        var modalResult = await modalRef?.Result!;

        if (modalResult.Cancelled)
            return;

        var emailAddress = modalResult.Data as string;
        var x = BuildHtml(Meeting.FirstOrDefault(), FinalReports);
        await EmailEnvoy.Send(emailAddress, Meeting.FirstOrDefault().Name, x);
    }

    public string BuildHtml(Meeting meeting, List<FinalReport> finalReports)
    {
        var sb = new StringBuilder();

        sb.AppendLine("<table style=\"width:100%\">");
        sb.AppendLine("<tr style=\"border: 1px solid black;\">");
        sb.Append("<th style=\"border: 1px solid black;\">"); 
        sb.Append("Name");
        sb.Append("</th>"); 
        sb.Append("<th style=\"border: 1px solid black;\">"); 
        sb.Append("Surname");
        sb.Append("</th>"); 
        sb.Append("<th style=\"border: 1px solid black;\">"); 
        sb.Append("Image");
        sb.Append("</th>"); 
        sb.AppendLine("</tr>");
        finalReports.ForEach(it =>
        {
            sb.AppendLine("<tr style=\"border: 1px solid black;\">");
            sb.AppendLine("<td style=\"border: 1px solid black;\">");
            sb.AppendLine(it.Employee.Name);
            sb.AppendLine("</td>");
            sb.AppendLine("<td style=\"border: 1px solid black;\">");
            sb.AppendLine(it.Employee.Surname);
            sb.AppendLine("</td>");
            sb.AppendLine("<td style=\"border: 1px solid black;\">");
            sb.AppendLine("<img src=\"" + it.Signature.DataUrl + "\" style=\"height:50px; width:50px; \" alt=\"\">");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
        });
        sb.AppendLine("</table>");
        return sb.ToString();
    }
}