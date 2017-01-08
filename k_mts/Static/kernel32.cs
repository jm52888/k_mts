using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;

namespace Microsoft.Win32
{
    public partial class WinApi
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
