using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;
using System.Text;
using static L_Alfred.LanguageList;

[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
static extern bool PostMessage(IntPtr windowHandle, int Msg, IntPtr wParam, IntPtr lParam);

IntPtr window = (IntPtr)0xffff;

IntPtr enUS = (IntPtr)0x00000409;
IntPtr ukUA = (IntPtr)0x00000422;
IntPtr ruRU = (IntPtr)0x00000419;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

var languagesList = GetInstalledLanguages();

Console.WriteLine("Your installed languages:");
foreach (var l in languagesList)
    Console.WriteLine($" - {l.Language}{(!string.IsNullOrEmpty(l.Locale) ? $" ({l.Locale})" : "")}");

var subsribtionKey = config["SubsribtionKey"];
var apiRegion = config["Region"];

var speechConfig = SpeechConfig.FromSubscription(subsribtionKey, apiRegion);

await RecognizeCommand();

async Task RecognizeCommand()
{
    using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
    using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

    var stopRecognition = new TaskCompletionSource<int>();

    recognizer.Recognizing += (s, e) =>
    {
        ChangeLanguage(e.Result.Text);
    };

    recognizer.Recognized += (s, e) =>
    {
        if (e.Result.Reason == ResultReason.NoMatch)
        {
            Console.WriteLine($"NOMATCH: Speech could not be recognized.");
        }
    };

    recognizer.Canceled += (s, e) =>
    {
        Console.WriteLine($"CANCELED: Reason={e.Reason}");

        if (e.Reason == CancellationReason.Error)
        {
            Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
            Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
            Console.WriteLine($"CANCELED: Did you update the speech key and location/region info?");
        }

        stopRecognition.TrySetResult(0);
    };

    recognizer.SessionStopped += (s, e) =>
    {
        Console.WriteLine("\n    Session stopped event.");
        stopRecognition.TrySetResult(0);
    };

    Console.WriteLine("Speak into your microphone.");
    await recognizer.StartContinuousRecognitionAsync();

    Task.WaitAny(new[] { stopRecognition.Task });
}

void ChangeLanguage(string switchTo)
{
    var kbLayout = languagesList.FirstOrDefault(x => x.Language.ToLower().Contains(switchTo))?.LanguageIdentifier;

    if(kbLayout != null)
        PostMessageWrapper((IntPtr)kbLayout);
}

void PostMessageWrapper(IntPtr kbLayout)
{
    PostMessage(window, 0x0050, IntPtr.Zero, kbLayout);
    PostMessage(window, 0x0051, IntPtr.Zero, kbLayout);
}