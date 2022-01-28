using L_Alfred.Vosk;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;
using System.Text;
using Vosk;

/*[DllImport("L-Alfred.LangSwitch.dll")]
static extern bool SwitchLang(uint kbLayout);*/

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

var subsribtionKey = config["SubsribtionKey"];
var apiRegion = config["Region"];

var speechConfig = SpeechConfig.FromSubscription(subsribtionKey, apiRegion);

//await RecognizeCommand();

Model model = new Model(@"C:\Users\dyats\Desktop\vosk-model-uk-v3");
VoskRecognition.DemoBytes(model);
    
async Task RecognizeCommand()
{
    using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
    using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

    var stopRecognition = new TaskCompletionSource<int>();

    recognizer.Recognizing += async (s, e) =>
    {
        //Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");

        ChangeLanguage(e.Result.Text);
    };

    /*recognizer.Recognized += (s, e) =>
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
    };*/

    Console.WriteLine("Speak into your microphone.");
    await recognizer.StartContinuousRecognitionAsync();

    Task.WaitAny(new[] { stopRecognition.Task });
}

/*void ChangeLanguage(string switchTo)
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
}*/

void ChangeLanguage(string switchTo)
{
    switch (switchTo.ToString().ToLower())
    {
        case string l when l.Contains("english") || l.Contains("англ"):
            PostMessageWrapper(enUS);
            break;
        case string l when l.Contains("ukrain") || l.Contains("украин"):
            PostMessageWrapper(ukUA);
            break;
        case string l when l.Contains("russia") || l.Contains("русск"):
            PostMessageWrapper(ruRU);
            break;
        default:
            break;
    }
}

void PostMessageWrapper(IntPtr kbLayout)
{
    PostMessage(window, 0x0050, IntPtr.Zero, kbLayout);
    PostMessage(window, 0x0051, IntPtr.Zero, kbLayout);
}