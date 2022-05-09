using CommunityToolkit.Mvvm.DependencyInjection;
using H.NotifyIcon;
using H.NotifyIcon.EfficiencyMode;
using LanguageAlfred.VoiceRecognition;
using LanguageAlfred.VoiceRecognition.RecognitionServices;
using LanguageAlfred.VoiceRecognition.Services;
using Microsoft.CognitiveServices.Speech;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Win32;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace LanguageAlfred.WinUI
{
    public sealed partial class MainWindow : Window
    {
        private MainWindowViewModel mainWindowViewModel { get; set; }
        private string LanguageAlfredStartupName = "Language Alfred";

        public MainWindow()
        {
            this.InitializeComponent();

            mainWindowViewModel = Ioc.Default.GetRequiredService<MainWindowViewModel>();

            //ExtendsContentIntoTitleBar = true;
            //SetTitleBar(AppTitleBar);

            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this); // m_window in App.cs
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

            var size = new Windows.Graphics.SizeInt32();
            size.Width = 800;
            size.Height = 400;

            //SetToRunOnStartup();

            appWindow.Resize(size);
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

        //TODO - check this example https://gist.github.com/HelBorn/2266242
        /*private void SetToRunOnStartup()
        {
            //todo - check this example https://gist.github.com/HelBorn/2266242
            try
            {
                string exePath = Assembly.GetEntryAssembly().Location;
                RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                string? regRef = rk.GetValue(LanguageAlfredStartupName)?.ToString();
                rk.DeleteValue(regRef);
                if (regRef is null)
                    rk.SetValue(LanguageAlfredStartupName, exePath);
            }
            catch(Exception ex)
            {

            }
        }*/
    }
}
