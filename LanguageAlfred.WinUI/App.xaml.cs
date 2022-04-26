using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;

namespace LanguageAlfred.WinUI
{
    public partial class App : Application
    {
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
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<MainWindowViewModel>();
            return services.BuildServiceProvider();
        }

        public static Window MainWindow { get; private set; }
    }
}
