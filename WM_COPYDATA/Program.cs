using System;
using System.Runtime.InteropServices;

class Program
{
    //In C# we don't have libraries like windows.h, we just use the conversions from: https://pinvoke.net/
    [StructLayout(LayoutKind.Sequential)]
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        public IntPtr lpData;
    }
    const int WM_COPYDATA = 0x004A;

    [DllImport("user32.dll")]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

    static void Main(string[] args)
    {
        const string m_pTestCommand = "echo \"TEST\""; //"TEST" should appear in your csgo console

        IntPtr m_hEngine = FindWindow("Valve001", null);
        COPYDATASTRUCT m_cData;
        m_cData.cbData = m_pTestCommand.Length + 1;
        m_cData.dwData = IntPtr.Zero;
        m_cData.lpData = Marshal.StringToHGlobalAnsi(m_pTestCommand);
        SendMessage(m_hEngine, WM_COPYDATA, IntPtr.Zero, ref m_cData);
        Marshal.FreeHGlobal(m_cData.lpData); //Free memory after use, save ya RAM!
    }
}