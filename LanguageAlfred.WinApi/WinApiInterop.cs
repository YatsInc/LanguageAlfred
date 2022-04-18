using System.Runtime.InteropServices;

namespace LanguageAlfred.WinApi;

public static class WinApiInterop
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool PostMessage(IntPtr windowHandle, int Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern uint GetKeyboardLayoutList(int nBuff, [Out] IntPtr[] lpList);
}