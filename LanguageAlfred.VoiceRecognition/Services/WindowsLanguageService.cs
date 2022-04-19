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

            var subLanguage = p.EnglishName.Split('(');
            var language = subLanguage[0].Trim();
            var locale = subLanguage.Length > 1 ? subLanguage[1].TrimEnd(')') : string.Empty;

            yield return new LanguageModel
            {
                LanguageIdentifier = identifier,
                Language = language,
                Locale = locale
            };
        }
    }

    public void ChangeLanguage(string switchTo)
    {
        var kbLayout = InstalledLanguages.FirstOrDefault(x => x.Language.ToLower().Contains(switchTo))?.LanguageIdentifier;

        if (kbLayout != null)
        {
            PostMessage(window, WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, (IntPtr)kbLayout);
            PostMessage(window, WM_INPUTLANGCHANGE, IntPtr.Zero, (IntPtr)kbLayout);
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
