using System.ComponentModel;
using System.Management.Automation;
using static L_Alfred.Constants;
using static L_Alfred.WinApi;

namespace L_Alfred;

public static class Languages
{
    private static readonly List<LanguageModel> InstalledLanguages = new List<LanguageModel>();

    static Languages()
    {
        InstalledLanguages = GetInstalledLanguages();
    }

    public static List<LanguageModel> GetInstalledLanguages()
    {
        PowerShell ps = PowerShell.Create().AddCommand("Get-WinUserLanguageList");

        IAsyncResult asyncResult = ps.BeginInvoke();

        List<LanguageModel> installedLanguages = new List<LanguageModel>();

        foreach (PSObject pSObject in ps.EndInvoke(asyncResult))
        {
            foreach (var p in (dynamic)pSObject.BaseObject)
            {
                var stringIdentifier = ((List<string>)p.InputMethodTips)?
                    .FirstOrDefault()?
                    .Split(':')
                    .LastOrDefault();

                IntPtr identifier = (IntPtr)(int)new Int32Converter().ConvertFromString("0x" + stringIdentifier);

                var subLanguage = ((string)p.LocalizedName).Split('(');

                var language = subLanguage[0].Trim();
                var locale = subLanguage.Length > 1
                    ? subLanguage[1].TrimEnd(')')
                    : string.Empty;

                installedLanguages.Add(new LanguageModel
                {
                    LanguageIdentifier = identifier,
                    Language = language,
                    Locale = locale
                });
            }
        }

        return installedLanguages;
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
}
