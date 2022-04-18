using System.Runtime.InteropServices;

namespace LanguageAlfred.VoiceRecognition;

public static class CheckOS
{
    public static bool IsWindows() =>
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
}
