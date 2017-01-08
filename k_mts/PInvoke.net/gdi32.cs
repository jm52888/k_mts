using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
    public partial class WinApi
    {
        #region Constant

        public const int R2_NOT = 6;
        public const int PS_INSIDEFRAME = 6;
        public const int NULL_BRUSH = 5;

        #endregion

        [DllImport("gdi32.dll")]
        public static extern int SetROP2(IntPtr hdc, int fnDrawMode);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePen(int fnPenStyle, int nWidth, uint crColor);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        public static extern IntPtr GetStockObject(int fnObject);

        [DllImport("gdi32.dll")]
        public static extern uint Rectangle(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}
