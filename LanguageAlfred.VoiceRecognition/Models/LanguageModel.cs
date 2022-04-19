namespace LanguageAlfred.VoiceRecognition.Models;

public class LanguageModel
{
    public IntPtr LanguageIdentifier { get; set; }

    public string Language { get; set; } = string.Empty;

    public string Locale { get; set; } = string.Empty;
}
