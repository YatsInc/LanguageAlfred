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

namespace LanguageAlfred.WinUI.Views;

public sealed partial class MainView : Page
{
    private MainViewModel mainViewModel { get; set; }

    public MainView()
    {
        this.InitializeComponent();

        mainViewModel = Ioc.Default.GetRequiredService<MainViewModel>();

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
    }
}
