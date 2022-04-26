using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Windows.Input;

namespace LanguageAlfred.WinUI;

public class MainWindowViewModel : ObservableObject
{
    private readonly IThemeSelectorService _themeSelectorService;
    private string? _topTitle;
    public string? TopTitle
    {
        get => _topTitle;
        set => SetProperty(ref _topTitle, value);
    }

    public ICommand SetThemeCommand { get; set; }

    public MainWindowViewModel(IThemeSelectorService themeSelectorService)
    {
        SetThemeCommand = new RelayCommand<string>((themeName) => UpdateTheme(themeName));
        _themeSelectorService = themeSelectorService;
    }

    private void UpdateTheme(string themeName)
    {
        if (Enum.TryParse(themeName, out ElementTheme elementTheme) is true)
        {
            _themeSelectorService.SetTheme(elementTheme);
        }
    }
}
