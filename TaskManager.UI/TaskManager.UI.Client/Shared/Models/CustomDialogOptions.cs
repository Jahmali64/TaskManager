using MudBlazor;

namespace TaskManager.UI.Client.Shared.Models;

public sealed record CustomDialogOptions : DialogOptions
{
    public CustomDialogOptions()
    {
        MaxWidth = MudBlazor.MaxWidth.Medium;
        CloseOnEscapeKey = true;
        CloseOnNavigation = true;
        CloseButton = true;
        BackdropClick = true;
        FullWidth = true;
    }
}