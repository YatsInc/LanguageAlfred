using System.ComponentModel;
using System.Management.Automation;

namespace L_Alfred;

public static class LanguageList
{
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
}
