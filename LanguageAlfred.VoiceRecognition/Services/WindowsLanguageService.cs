using LanguageAlfred.VoiceRecognition.Models;
using System.ComponentModel;
using System.Globalization;
using static LanguageAlfred.WinApi.WinApiInterop;
using static LanguageAlfred.WinApi.WinApiParameters;

namespace LanguageAlfred.VoiceRecognition.Services;

public class WindowsLanguageService : ILanguageService
{
    private readonly List<LanguageModel> InstalledLanguages = new List<LanguageModel>();

    public WindowsLanguageService()
    {
        InstalledLanguages = GetInstalledLanguages().ToList();
    }

    public IEnumerable<LanguageModel> GetInstalledLanguages()
    {
        foreach (var p in GetInstalledInputLanguages())
        {
            var kbLayoutId = "0x" + string.Format("{0:x}", p.KeyboardLayoutId);
            var identifier = (IntPtr)(int?)new Int32Converter().ConvertFromString(kbLayoutId);

            var displayLanguageName = p.DisplayName
                .Split('(')[0]
                .Trim();

            var englishLanguageName = p.EnglishName
                .Split('(')[0]
                .Trim();

            yield return new LanguageModel
            {
                LanguageIdentifier = identifier,
                DisplayLanguageName = displayLanguageName,
                EnglishLanguageName = englishLanguageName,
            };
        }
    }

    public void ChangeLanguage(string switchTo)
    {
        var kbLayout = InstalledLanguages.FirstOrDefault(x =>   x.DisplayLanguageName.ToLower() == switchTo.ToLower() ||
                                                                x.EnglishLanguageName.ToLower() == switchTo.ToLower());

        if (kbLayout is not null)
        {
            PostMessage(window, WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, kbLayout.LanguageIdentifier);
            PostMessage(window, WM_INPUTLANGCHANGE, IntPtr.Zero, kbLayout.LanguageIdentifier);
        }
    }

    private IEnumerable<CultureInfo> GetInstalledInputLanguages()
    {
        // first determine the number of installed languages
        uint size = GetKeyboardLayoutList(0, null!);
        IntPtr[] ids = new IntPtr[size];

        // then get the handles list of those languages
        GetKeyboardLayoutList(ids.Length, ids);

        foreach (int id in ids) // note the explicit cast IntPtr -> int
        {
            yield return new CultureInfo(id & 0xFFFF);
        }
    }
}
