namespace LanguageAlfred.WinApi;

public static class WinApiParameters
{
    public static readonly IntPtr window = (IntPtr)0xffff;
    public const int WM_INPUTLANGCHANGEREQUEST = 0x0050;
    public const int WM_INPUTLANGCHANGE = 0x0051;
}
