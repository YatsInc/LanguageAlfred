/*using L_Alfred.VoiceRecognition;
using L_Alfred.VoiceRecognition.OperatingSystems.Linux.Services;
using L_Alfred.VoiceRecognition.OperatingSystems.MacOS.Services;
using L_Alfred.VoiceRecognition.OperatingSystems.Windows.Services;
using L_Alfred.VoiceRecognition.RecognitionServices;
using System.Text;
using static L_Alfred.VoiceRecognition.OperatingSystems.CheckOS;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

ILanguageService languageService = null;

if (IsWindows())
{
    languageService = new WindowsLanguageService();
}
else if(IsMacOS())
{
    languageService = new MacOSLanguageService();
}
else if (IsLinux())
{
    languageService = new LinuxLanguageService();
}
else
{
    throw new NotImplementedException();
}

languageService.ShowInstalledLanguages();

var recognizer = new AzureSpeechToTextService(languageService);
await recognizer.RecognizeCommandAsync();
*/