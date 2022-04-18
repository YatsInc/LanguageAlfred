using LanguageAlfred.VoiceRecognition.Models;

namespace LanguageAlfred.VoiceRecognition;

public interface ILanguageService
{
    public IEnumerable<LanguageModel> GetInstalledLanguages();

    public void ChangeLanguage(string switchTo);
}
