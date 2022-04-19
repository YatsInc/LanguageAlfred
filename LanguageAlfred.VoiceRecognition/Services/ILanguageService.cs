using LanguageAlfred.VoiceRecognition.Models;

namespace LanguageAlfred.VoiceRecognition.Services;

public interface ILanguageService
{
    public IEnumerable<LanguageModel> GetInstalledLanguages();

    public void ChangeLanguage(string switchTo);
}
