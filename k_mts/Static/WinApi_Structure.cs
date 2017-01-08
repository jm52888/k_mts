using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Runtime.InteropServices;
using System.Globalization;

namespace Microsoft.Win32
{
    public partial class WinApi
    {
        #region Structure

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public POINT(System.Drawing.Point pt) : this(pt.X, pt.Y) { }

            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point(p.X, p.Y);
            }

            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
            public Rectangle ToRectangle()
            {
                return new Rectangle(Left, Top, Right - Left, Bottom - Top);
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct LVITEM
        {
            public int mask;
            public int iItem;
            public int iSubItem;
            public int state;
            public int stateMask;
            public string pszText;
            public int cchTextMax;
            public int iImage;
            public IntPtr lParam;
            public int iIndent;
            public int iGroupId;
            public int cColumns; // tile view columns
            public IntPtr puColumns;

            public void Reset()
            {
                pszText = null;
                mask = 0;
                iItem = 0;
                iSubItem = 0;
                stateMask = 0;
                state = 0;
                cchTextMax = 0;
                iImage = 0;
                lParam = IntPtr.Zero;
                iIndent = 0;
                iGroupId = 0;
                cColumns = 0;
                puColumns = IntPtr.Zero;
            }

            public override string ToString()
            {
                return "LVITEM: pszText = " + pszText
                     + ", iItem = " + iItem.ToString(CultureInfo.InvariantCulture)
                     + ", iSubItem = " + iSubItem.ToString(CultureInfo.InvariantCulture)
                     + ", state = " + state.ToString(CultureInfo.InvariantCulture)
                     + ", iGroupId = " + iGroupId.ToString(CultureInfo.InvariantCulture)
                     + ", cColumns = " + cColumns.ToString(CultureInfo.InvariantCulture);
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]        
        public struct LVITEM_NOTEXT
        {
            public int mask;
            public int iItem;
            public int iSubItem;
            public int state;
            public int stateMask;
            public IntPtr /*string*/   pszText;
            public int cchTextMax;
            public int iImage;
            public IntPtr lParam;
            public int iIndent;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }


        #endregion
    }
}
