using System.Runtime.InteropServices;

namespace L_Alfred;

public static class WinApi
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool PostMessage(IntPtr windowHandle, int Msg, IntPtr wParam, IntPtr lParam);
}
