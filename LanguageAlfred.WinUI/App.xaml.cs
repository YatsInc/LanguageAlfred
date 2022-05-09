using CommunityToolkit.Mvvm.DependencyInjection;
using H.NotifyIcon;
using LanguageAlfred.WinUI.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
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
            MainWindow = new MainWindow();
            MainWindow.Title = "Alfred";
            MainWindow.Activate();
            MainWindow.Hide();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<MainWindowViewModel>();
            return services.BuildServiceProvider();
        }
    }
}
