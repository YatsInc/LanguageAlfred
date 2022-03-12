using L_Alfred.VoiceRecognition.Models;
using System.Globalization;

namespace L_Alfred.VoiceRecognition.OperatingSystems.Linux.Services;

public class LinuxLanguageService : ILanguageService
{
    private readonly List<LanguageModel> InstalledLanguages = new List<LanguageModel>();

    public LinuxLanguageService()
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
