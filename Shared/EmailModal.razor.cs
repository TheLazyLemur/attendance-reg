using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace attendance_reg.Shared;

public partial class EmailModal
{
    class EmailEntry
    {
        public string? Email;
    }

    [CascadingParameter] BlazoredModalInstance? ModalInstance { get; set; }

    private readonly EmailEntry _emailEntry = new();

    private async Task HandleValidSubmit()
    {
        await ModalInstance?.CloseAsync(ModalResult.Ok(_emailEntry.Email))!;
    }
}