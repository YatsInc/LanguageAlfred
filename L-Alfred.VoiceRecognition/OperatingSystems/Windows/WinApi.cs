﻿using System.Runtime.InteropServices;

namespace L_Alfred.VoiceRecognition.OperatingSystems.Windows;

public static class WinApi
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool PostMessage(IntPtr windowHandle, int Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern uint GetKeyboardLayoutList(int nBuff, [Out] IntPtr[] lpList);
}