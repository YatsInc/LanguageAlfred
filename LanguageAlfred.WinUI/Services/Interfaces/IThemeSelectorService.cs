using Microsoft.UI.Xaml;

namespace LanguageAlfred.WinUI.Services.Interfaces;

public interface IThemeSelectorService
{
    public ElementTheme GetTheme();
    public void SetTheme(ElementTheme theme);
}