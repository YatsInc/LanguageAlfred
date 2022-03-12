using L_Alfred.VoiceRecognition.Models;

namespace L_Alfred.VoiceRecognition;

public interface ILanguageService
{
    public IEnumerable<LanguageModel> GetInstalledLanguages();

    public void ShowInstalledLanguages();

    public void ChangeLanguage(string switchTo);
}
