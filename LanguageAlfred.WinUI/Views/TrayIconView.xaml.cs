using H.NotifyIcon;
using Microsoft.UI.Xaml.Input;
using System;

namespace LanguageAlfred.WinUI.Views;

public sealed partial class TrayIconView
{
    public TrayIconView()
    {
        InitializeComponent();
    }

    public void ShowHideWindowCommand_ExecuteRequested(object? _, ExecuteRequestedEventArgs args)
    {
        var window = App.MainWindow;
        if (window == null)
        {
            return;
        }

        if (window.Visible)
        {
            window.Hide();
        }
        else
        {
            window.Show();
        }
    }

    public void ExitApplicationCommand_ExecuteRequested(object? _, ExecuteRequestedEventArgs args)
    {
        App.MainWindow?.Close();
    }
}
