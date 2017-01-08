using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Win32
{
    public partial class WinApi
    {
        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);
    }
}
