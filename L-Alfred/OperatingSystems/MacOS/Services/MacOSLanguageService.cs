using L_Alfred.VoiceRecognition.Models;
using System.Globalization;

namespace L_Alfred.VoiceRecognition.OperatingSystems.MacOS.Services;

public class MacOSLanguageService : ILanguageService
{
    private readonly List<LanguageModel> InstalledLanguages = new List<LanguageModel>();

    public MacOSLanguageService()
    {
        InstalledLanguages = GetInstalledLanguages().ToList();
    }

    public IEnumerable<LanguageModel> GetInstalledLanguages()
    {
        throw new NotImplementedException();
    }

    public void ShowInstalledLanguages()
    {
        throw new NotImplementedException();
    }

    public void ChangeLanguage(string switchTo)
    {
        throw new NotImplementedException();
    }

    private IEnumerable<CultureInfo> GetInstalledInputLanguages()
    {
        throw new NotImplementedException();
    }
}
