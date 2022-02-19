using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;
using System.Text;
using static L_Alfred.Languages;


Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

ShowInstalledLanguages();

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
