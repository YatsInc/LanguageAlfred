using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;

namespace L_Alfred.VoiceRecognition.RecognitionServices;

public class AzureSpeechToTextService
{
    private IConfigurationRoot Config { get; set; }

    private string SubsribtionKey { get; set; }

    private string ApiRegion { get; set; }

    private SpeechConfig SpeechConfig { get; set; }

    private ILanguageService LanguageService { get; set; }

    public AzureSpeechToTextService(ILanguageService languageService)
    {
        Config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        SubsribtionKey = Config["SubsribtionKey"];
        ApiRegion = Config["Region"];

        SpeechConfig = SpeechConfig.FromSubscription(SubsribtionKey, ApiRegion);

        LanguageService = languageService;
    }

    public async Task RecognizeCommandAsync()
    {
        using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
        using var recognizer = new SpeechRecognizer(SpeechConfig, audioConfig);

        var stopRecognition = new TaskCompletionSource<int>();

        recognizer.Recognizing += (s, e) =>
        {
            LanguageService.ChangeLanguage(e.Result.Text);
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
}
