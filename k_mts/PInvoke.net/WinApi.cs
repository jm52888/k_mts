using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Diagnostics;

namespace Microsoft.Win32
{
    public partial class WinApi
    {
        #region Constructor

        private WinApi() { }

        #endregion

        #region Function :: Wrapper (CS style return type)

        public static Rectangle GetWindowRect(IntPtr hWnd)
        {
            Debug.Assert(hWnd != IntPtr.Zero);
            RECT rect = new RECT();
            if (GetWindowRect(hWnd, ref rect) == false)
                throw new Exception("GetWindowRect failed");
            return rect.ToRectangle();
        }

        public static string GetWindowText(IntPtr hWnd)
        {
            Debug.Assert(hWnd != IntPtr.Zero);
            StringBuilder WindowText = new StringBuilder(GetWindowTextLength(hWnd) + 1);
            GetWindowText(hWnd, WindowText, WindowText.Capacity);
            return WindowText.ToString();
        }

        #endregion
    }
}
