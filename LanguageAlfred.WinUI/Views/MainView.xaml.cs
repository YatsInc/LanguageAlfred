using CommunityToolkit.Mvvm.DependencyInjection;
using LanguageAlfred.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Popups;

namespace LanguageAlfred.WinUI.Views;

public sealed partial class MainView : Page
{
    private MainViewModel mainViewModel { get; set; }

    public MainView()
    {
        this.InitializeComponent();

        mainViewModel = Ioc.Default.GetRequiredService<MainViewModel>();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        var startup = await StartupTask.GetAsync("LaunchOnStartupTaskId");
        UpdateToggleState(startup.State);
        
        await ToggleLaunchOnStartup(true);
    }

    private void UpdateToggleState(StartupTaskState state)
    {
        /*LaunchOnStartupToggle.IsEnabled = true;
        switch (state)
        {
            case StartupTaskState.Enabled:
                LaunchOnStartupToggle.IsChecked = true;
                break;
            case StartupTaskState.Disabled:
            case StartupTaskState.DisabledByUser:
                LaunchOnStartupToggle.IsChecked = false;
                break;
            default:
                LaunchOnStartupToggle.IsEnabled = false;
                break;
        }*/

    }

    private async Task ToggleLaunchOnStartup(bool enable)
    {
        var startup = await StartupTask.GetAsync("LaunchOnStartupTaskId");
        switch (startup.State)
        {
            case StartupTaskState.Enabled when !enable:
                startup.Disable();
                break;
            case StartupTaskState.Disabled when enable:
                var updatedState = await startup.RequestEnableAsync();
                UpdateToggleState(updatedState);
                break;
            case StartupTaskState.DisabledByUser when enable:
                await new MessageDialog("Unable to change state of startup task via the application - enable via Startup tab on Task Manager (Ctrl+Shift+Esc)").ShowAsync();
                break;
            default:
                await new MessageDialog("Unable to change state of startup task").ShowAsync();
                break;
        }
    }
}
