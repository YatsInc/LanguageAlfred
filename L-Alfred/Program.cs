using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;
using System.Text;

[DllImport("L-Alfred.LangSwitch.dll")]
static extern bool SwitchLang(uint kbLayout);

const uint enUS = 0x00000409;
const uint ukUA = 0x00000422;
const uint ruRU = 0x00000419;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

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
        Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");

        ChangeLanguage(e.Result.Text);
    };

    recognizer.Recognized += (s, e) =>
    {
        if (e.Result.Reason == ResultReason.RecognizedSpeech)
        {
            Console.WriteLine($"RECOGNIZED: Text={e.Result.Text}");
        }
        else if (e.Result.Reason == ResultReason.NoMatch)
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
    //Console.WriteLine($"RECOGNIZED: Text = {result.Text}");

    // ChangeLanguage(result.Text);
}

void ChangeLanguage(string switchTo)
{
    switch (switchTo.ToString().ToLower())
    {
        case string l when l.Contains("english") || l.Contains("англ"):
            SwitchLang(enUS);
            break;
        case string l when l.Contains("ukrain") || l.Contains("украин"):
            SwitchLang(ukUA);
            break;
        case string l when l.Contains("russia") || l.Contains("русск"):
            SwitchLang(ruRU);
            break;
        default:
            break;
    }
}