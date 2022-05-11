using LanguageAlfred.WinUI.Services.Interfaces;
using Microsoft.UI.Xaml;

namespace LanguageAlfred.WinUI.Services;

public class ThemeSelectorService : IThemeSelectorService
{
    public ElementTheme GetTheme()
    {
        if (App.MainWindow?.Content is FrameworkElement frameworkElement)
        {
            return frameworkElement.ActualTheme;
        }

        return ElementTheme.Default;
    }

    public void SetTheme(ElementTheme theme)
    {
        if (App.MainWindow?.Content is FrameworkElement frameworkElement)
        {
            frameworkElement.RequestedTheme = theme;
        }
    }
}
