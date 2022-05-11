using CommunityToolkit.Mvvm.DependencyInjection;
using LanguageAlfred.VoiceRecognition.RecognitionServices;
using LanguageAlfred.VoiceRecognition.Services;
using LanguageAlfred.WinUI.ViewModels;
using Microsoft.CognitiveServices.Speech;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace LanguageAlfred.WinUI.Views
{
    public sealed partial class MainView : Page
    {
        private MainViewModel mainViewModel { get; set; }

        public MainView()
        {
            this.InitializeComponent();

            mainViewModel = Ioc.Default.GetRequiredService<MainViewModel>();

            //ExtendsContentIntoTitleBar = true;
            //SetTitleBar(AppTitleBar);

            ConnectToAzure();
        }

        private void ConnectToAzureSpeechToText(object sender, RoutedEventArgs e)
        {
            ConnectToAzure();
        }

        private void ConnectToAzure()
        {
            var subscribtionKey = subscribtionKeyTextBox.Text;
            var region = regionTextBox.Text;

            SpeechConfig speechConfig = SpeechConfig.FromSubscription(subscribtionKey, region);
            new Thread(async () =>
            {
                await RecognizeVoice(speechConfig);
            }).Start();

            ConnectToAzureBtn.IsEnabled = false;
        }

        private async Task RecognizeVoice(SpeechConfig speechConfig)
        {
            ILanguageService languageService = null;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                languageService = new WindowsLanguageService();
            }
            else
            {
                throw new NotImplementedException();
            }

            var recognizer = new AzureSpeechToTextService(languageService, speechConfig);
            await recognizer.RecognizeCommandAsync();
        }
    }
}
