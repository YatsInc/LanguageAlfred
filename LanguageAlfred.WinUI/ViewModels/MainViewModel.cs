using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LanguageAlfred.VoiceRecognition.RecognitionServices;
using LanguageAlfred.VoiceRecognition.Services;
using LanguageAlfred.WinUI.Services.Interfaces;
using Microsoft.CognitiveServices.Speech;
using Microsoft.UI.Xaml;
using System;
using System.Threading;
using System.Windows.Input;

namespace LanguageAlfred.WinUI.ViewModels;

public class MainViewModel : ObservableObject
{
    private string _subscriptionKey = "a2169e4dec5b42fd8b8390d914224916";
    public string SubscriptionKey
    {
        get => _subscriptionKey;
        set => SetProperty(ref _subscriptionKey, value);
    }

    private string _region = "westeurope";
    public string Region
    {
        get => _region;
        set => SetProperty(ref _region, value);
    }

    private readonly IThemeSelectorService _themeSelectorService;
    private readonly ILanguageService _languageService;

    public ICommand SetThemeCommand { get; set; }
    public ICommand ConnectToAzure { get; set; }

    public MainViewModel(IThemeSelectorService themeSelectorService, ILanguageService languageService)
    {
        SetThemeCommand = new RelayCommand<string>((themeName) => UpdateTheme(themeName));
        ConnectToAzure = new RelayCommand(() => ConnectToAzureSpeechToText(_subscriptionKey, _region));

        _themeSelectorService = themeSelectorService;
        _languageService = languageService;

        ConnectToAzure.Execute(this);
    }

    private void UpdateTheme(string themeName)
    {
        if (Enum.TryParse(themeName, out ElementTheme elementTheme) is true)
        {
            _themeSelectorService.SetTheme(elementTheme);
        }
    }

    private void ConnectToAzureSpeechToText(string subscriptionKey, string region)
    {
        SpeechConfig speechConfig = SpeechConfig.FromSubscription(subscriptionKey, region);
        var recognizer = new AzureSpeechToTextService(_languageService, speechConfig);
        new Thread(async () =>
        {
            await recognizer.RecognizeCommandAsync();
        }).Start();
    }
}
