using LanguageAlfred.VoiceRecognition;
using LanguageAlfred.VoiceRecognition.RecognitionServices;
using LanguageAlfred.VoiceRecognition.Services;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace LanguageAlfred.WinUI
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this); // m_window in App.cs
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

            var size = new Windows.Graphics.SizeInt32();
            size.Width = 800;
            size.Height = 400;

            appWindow.Resize(size);

            new Thread(async () =>
            {
                await RecognizeVoice();
            }).Start();

            appWindow.Hide();
        }

        private async Task RecognizeVoice()
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

            var recognizer = new AzureSpeechToTextService(languageService);
            await recognizer.RecognizeCommandAsync();
        }
    }
}
