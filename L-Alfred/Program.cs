using L_Alfred.VoiceRecognition;
using System.Text;
using static L_Alfred.Languages;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

ShowInstalledLanguages();

var recognizer = new AzureSpeechToText();
await recognizer.RecognizeCommandAsync();
