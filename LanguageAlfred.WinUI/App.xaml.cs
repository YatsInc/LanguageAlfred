using CommunityToolkit.Mvvm.DependencyInjection;
using H.NotifyIcon;
using LanguageAlfred.VoiceRecognition.Services;
using LanguageAlfred.WinUI.Services;
using LanguageAlfred.WinUI.Services.Interfaces;
using LanguageAlfred.WinUI.ViewModels;
using LanguageAlfred.WinUI.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace LanguageAlfred.WinUI
{
    public partial class App : Application
    {
        public static Window MainWindow { get; private set; }

        public App()
        {
            this.InitializeComponent();

            Ioc.Default.ConfigureServices(ConfigureServices());
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            MainWindow = new Window
            {
                Content = new Frame
                {
                    Content = new MainView()
                }
            };

            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
            var size = new Windows.Graphics.SizeInt32();
            size.Width = 800;
            size.Height = 400;
            appWindow.Resize(size);

            MainWindow.Title = "Alfred";
            MainWindow.ExtendsContentIntoTitleBar = true;
            MainWindow.SetTitleBar(MainWindow.Content);
            MainWindow.Activate();
            MainWindow.Hide();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<ILanguageService, WindowsLanguageService>();
            services.AddSingleton<MainViewModel>();
            return services.BuildServiceProvider();
        }
    }
}
