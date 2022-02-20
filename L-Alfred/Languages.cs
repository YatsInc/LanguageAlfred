using System.ComponentModel;
using System.Globalization;
using static L_Alfred.Constants;
using static L_Alfred.WinApi;

namespace L_Alfred;

public static class Languages
{
    private static readonly List<LanguageModel> InstalledLanguages = new List<LanguageModel>();

    static Languages()
    {
        InstalledLanguages = GetInstalledLanguages().ToList();
    }

    public static IEnumerable<LanguageModel> GetInstalledLanguages()
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

    public static void ShowInstalledLanguages()
    {
        Console.WriteLine("Your installed languages:");
        foreach (var l in InstalledLanguages)
            Console.WriteLine($" - {l.Language}{(!string.IsNullOrEmpty(l.Locale) ? $" ({l.Locale})" : "")}");
    }

    public static void ChangeLanguage(string switchTo)
    {
        var kbLayout = InstalledLanguages.FirstOrDefault(x => x.Language.ToLower().Contains(switchTo))?.LanguageIdentifier;

        if (kbLayout != null)
        {
            PostMessage(window, WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, (IntPtr)kbLayout);
            PostMessage(window, WM_INPUTLANGCHANGE, IntPtr.Zero, (IntPtr)kbLayout);
        }
    }

    private static IEnumerable<CultureInfo> GetInstalledInputLanguages()
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
