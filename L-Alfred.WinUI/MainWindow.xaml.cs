using L_Alfred.VoiceRecognition;
using L_Alfred.VoiceRecognition.OperatingSystems.Linux.Services;
using L_Alfred.VoiceRecognition.OperatingSystems.MacOS.Services;
using L_Alfred.VoiceRecognition.OperatingSystems.Windows.Services;
using L_Alfred.VoiceRecognition.RecognitionServices;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using static L_Alfred.VoiceRecognition.OperatingSystems.CheckOS;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace L_Alfred.WinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
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
        }

        private async Task RecognizeVoice()
        {
            ILanguageService languageService = null;

            if (IsWindows())
            {
                languageService = new WindowsLanguageService();
            }
            else if (IsMacOS())
            {
                languageService = new MacOSLanguageService();
            }
            else if (IsLinux())
            {
                languageService = new LinuxLanguageService();
            }
            else
            {
                throw new NotImplementedException();
            }

            //languageService.ShowInstalledLanguages();

            var recognizer = new AzureSpeechToTextService(languageService);
            await recognizer.RecognizeCommandAsync();
        }
    }
}
