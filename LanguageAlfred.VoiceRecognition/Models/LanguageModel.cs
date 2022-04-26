namespace LanguageAlfred.VoiceRecognition.Models;

public class LanguageModel
{
    public IntPtr LanguageIdentifier { get; set; }

    public string EnglishLanguageName { get; set; } = string.Empty;

    public string DisplayLanguageName { get; set; } = string.Empty;
}
