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

string switchTo = "";

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

var subsribtionKey = config["SubsribtionKey"];
var apiRegion = config["Region"];

var speechConfig = SpeechConfig.FromSubscription(subsribtionKey, apiRegion);

async Task RecognizeCommand()
{
    using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
    using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

    Console.WriteLine("Speak into your microphone.");
    var result = await recognizer.RecognizeOnceAsync();
    Console.WriteLine($"RECOGNIZED: Text = {result.Text}");
    Console.WriteLine($"Status: {result.Reason}");

    switchTo = result.Text;
}

await RecognizeCommand();

Task changeLanguageTask = Task.Factory.StartNew(() =>
{
    var res = false;

    switch (switchTo.ToString().ToLower())
    {
        case string l when l.Contains("english"):
            res = SwitchLang(enUS);
            break;
        case string l when l.Contains("ukrain"):
            res = SwitchLang(ukUA);
            break;
        case string l when l.Contains("russia"):
            res = SwitchLang(ruRU);
            break;
        default:
            break;
    }

    Console.WriteLine(res);
});

Console.ReadLine();