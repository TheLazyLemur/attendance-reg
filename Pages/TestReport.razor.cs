using System.Globalization;
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

    [Inject] private MeetingReportService? MeetingReportService { get; set; }
    [Inject] private MeetingEnvoy? MeetingEnvoy { get; set; }
    [Inject] private EmailEnvoy? EmailEnvoy { get; set; }

    private List<FinalReport>? FinalReports { get; set; }
    private List<Meeting>? Meeting { get; set; }


    protected override void OnInitialized()
    {
        Task.Run(async () =>
        {
            if(MeetingEnvoy is null) return;
            
            Meeting = await MeetingEnvoy.GetMeeting(MeetingId);
            await InvokeAsync(StateHasChanged);
        });

        Task.Run(async () =>
        {
            if(MeetingId is null || MeetingReportService is null) return;
            FinalReports = await MeetingReportService.GenerateReport(int.Parse(MeetingId));
            if(FinalReports is null) return;
            FinalReports = FinalReports.DistinctBy(it => it.Employee?.Id).ToList();
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

        if (Meeting != null && FinalReports != null && modalResult.Data is string emailAddress)
        {
            var x = BuildHtml(Meeting.FirstOrDefault(), FinalReports);
            var n = Meeting.FirstOrDefault()?.Name;
            if (n != null && EmailEnvoy != null)
                await EmailEnvoy.Send(emailAddress, n, x);
        }
    }

    public string BuildHtml(Meeting? meeting, List<FinalReport> finalReports)
    {
        var sb = new StringBuilder();

        sb.Append("<img style=\"height:100px; padding-top:50px; padding-bottom:50px\" src=\"https://holborn-report.netlify.app/img/holborn.png\">");

        sb.Append("<div style=\"padding-bottom:50px;\">");
        sb.Append("<div> Name: " + meeting?.Name + "</div>");
        sb.Append("<div> Speaker: " + meeting?.Speaker + "</div>");
        sb.Append("<div> Topic: " + meeting?.Topic + "</div>");
        sb.Append("<div> Company: " + meeting?.Company + "</div>");
        sb.Append("<div> Date: " + meeting?.MeetingDate.ToString(CultureInfo.InvariantCulture) + "</div>");
        sb.Append("</div>");

        if (meeting?.AssetManagementPresentation is true)
            sb.Append("<div> Asset Management Presentation" +"</div>");
        if (meeting?.ProductTraining is true)
            sb.Append("<div> Asset Management Product Training" + "</div>");
        if (meeting?.InternalTraining is true)
            sb.Append("<div> Asset Management Internal Training" + "</div>");
        
        sb.AppendLine("<table style=\"width:100%\">");
        sb.AppendLine("<tr style=\"border: 1px solid black;\">");
        sb.Append("<th style=\"border: 1px solid black;\">"); 
        sb.Append("Name");
        sb.Append("</th>"); 
        sb.Append("<th style=\"border: 1px solid black;\">"); 
        sb.Append("Status");
        sb.Append("</th>"); 
        sb.Append("<th style=\"border: 1px solid black;\">"); 
        sb.Append("Signature");
        sb.Append("</th>"); 
        sb.AppendLine("</tr>");
        finalReports.ForEach(it =>
        {
            sb.AppendLine("<tr style=\"border: 1px solid black;\">");
            sb.AppendLine("<td style=\"border: 1px solid black;\">");
            sb.AppendLine(it.Employee?.Name + " " + it.Employee?.Surname);
            sb.AppendLine("</td>");
            sb.AppendLine("<td style=\"border: 1px solid black;\">");
            sb.AppendLine(it.Attendance.Status);
            sb.AppendLine("</td>");
            sb.AppendLine("<td style=\"border: 1px solid black;\">");
            if (it.Signature != null && !string.IsNullOrEmpty(it.Signature.DataUrl))
                sb.AppendLine("<img src=\"" + it.Signature?.DataUrl + "\" style=\"height:50px; width:50px; \" alt=\"\">");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
        });
        sb.AppendLine("</table>");
        return sb.ToString();
    }
}