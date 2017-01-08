using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Win32
{
    public partial class WinApi
    {
        public class WM
        {
            public const int NULL = 0x00;
            public const int CREATE = 0x01;
            public const int DESTROY = 0x02;
            public const int MOVE = 0x03;
            public const int SIZE = 0x05;
            public const int ACTIVATE = 0x06;
            public const int SETFOCUS = 0x07;
            public const int KILLFOCUS = 0x08;
            public const int ENABLE = 0x0A;
            public const int SETREDRAW = 0x0B;
            public const int SETTEXT = 0x0C;
            public const int GETTEXT = 0x0D;
            public const int GETTEXTLENGTH = 0x0E;
            public const int PAINT = 0x0F;
            public const int CLOSE = 0x10;
            public const int QUERYENDSESSION = 0x11;
            public const int QUIT = 0x12;
            public const int QUERYOPEN = 0x13;
            public const int ERASEBKGND = 0x14;
            public const int SYSCOLORCHANGE = 0x15;
            public const int ENDSESSION = 0x16;
            public const int SYSTEMERROR = 0x17;
            public const int SHOWWINDOW = 0x18;
            public const int CTLCOLOR = 0x19;
            public const int WININICHANGE = 0x1A;
            public const int SETTINGCHANGE = 0x1A;
            public const int DEVMODECHANGE = 0x1B;
            public const int ACTIVATEAPP = 0x1C;
            public const int FONTCHANGE = 0x1D;
            public const int TIMECHANGE = 0x1E;
            public const int CANCELMODE = 0x1F;
            public const int SETCURSOR = 0x20;
            public const int MOUSEACTIVATE = 0x21;
            public const int CHILDACTIVATE = 0x22;
            public const int QUEUESYNC = 0x23;
            public const int GETMINMAXINFO = 0x24;
            public const int PAINTICON = 0x26;
            public const int ICONERASEBKGND = 0x27;
            public const int NEXTDLGCTL = 0x28;
            public const int SPOOLERSTATUS = 0x2A;
            public const int DRAWITEM = 0x2B;
            public const int MEASUREITEM = 0x2C;
            public const int DELETEITEM = 0x2D;
            public const int VKEYTOITEM = 0x2E;
            public const int CHARTOITEM = 0x2F;

            public const int SETFONT = 0x30;
            public const int GETFONT = 0x31;
            public const int SETHOTKEY = 0x32;
            public const int GETHOTKEY = 0x33;
            public const int QUERYDRAGICON = 0x37;
            public const int COMPAREITEM = 0x39;
            public const int COMPACTING = 0x41;
            public const int WINDOWPOSCHANGING = 0x46;
            public const int WINDOWPOSCHANGED = 0x47;
            public const int POWER = 0x48;
            public const int COPYDATA = 0x4A;
            public const int CANCELJOURNAL = 0x4B;
            public const int NOTIFY = 0x4E;
            public const int INPUTLANGCHANGEREQUEST = 0x50;
            public const int INPUTLANGCHANGE = 0x51;
            public const int TCARD = 0x52;
            public const int HELP = 0x53;
            public const int USERCHANGED = 0x54;
            public const int NOTIFYFORMAT = 0x55;
            public const int CONTEXTMENU = 0x7B;
            public const int STYLECHANGING = 0x7C;
            public const int STYLECHANGED = 0x7D;
            public const int DISPLAYCHANGE = 0x7E;
            public const int GETICON = 0x7F;
            public const int SETICON = 0x80;

            public const int NCCREATE = 0x81;
            public const int NCDESTROY = 0x82;
            public const int NCCALCSIZE = 0x83;
            public const int NCHITTEST = 0x84;
            public const int NCPAINT = 0x85;
            public const int NCACTIVATE = 0x86;
            public const int GETDLGCODE = 0x87;
            public const int NCMOUSEMOVE = 0xA0;
            public const int NCLBUTTONDOWN = 0xA1;
            public const int NCLBUTTONUP = 0xA2;
            public const int NCLBUTTONDBLCLK = 0xA3;
            public const int NCRBUTTONDOWN = 0xA4;
            public const int NCRBUTTONUP = 0xA5;
            public const int NCRBUTTONDBLCLK = 0xA6;
            public const int NCMBUTTONDOWN = 0xA7;
            public const int NCMBUTTONUP = 0xA8;
            public const int NCMBUTTONDBLCLK = 0xA9;

            public const int KEYFIRST = 0x100;
            public const int KEYDOWN = 0x100;
            public const int KEYUP = 0x101;
            public const int CHAR = 0x102;
            public const int DEADCHAR = 0x103;
            public const int SYSKEYDOWN = 0x104;
            public const int SYSKEYUP = 0x105;
            public const int SYSCHAR = 0x106;
            public const int SYSDEADCHAR = 0x107;
            public const int KEYLAST = 0x108;

            public const int IME_STARTCOMPOSITION = 0x10D;
            public const int IME_ENDCOMPOSITION = 0x10E;
            public const int IME_COMPOSITION = 0x10F;
            public const int IME_KEYLAST = 0x10F;

            public const int INITDIALOG = 0x110;
            public const int COMMAND = 0x111;
            public const int SYSCOMMAND = 0x112;
            public const int TIMER = 0x113;
            public const int HSCROLL = 0x114;
            public const int VSCROLL = 0x115;
            public const int INITMENU = 0x116;
            public const int INITMENUPOPUP = 0x117;
            public const int MENUSELECT = 0x11F;
            public const int MENUCHAR = 0x120;
            public const int ENTERIDLE = 0x121;

            public const int CTLCOLORMSGBOX = 0x132;
            public const int CTLCOLOREDIT = 0x133;
            public const int CTLCOLORLISTBOX = 0x134;
            public const int CTLCOLORBTN = 0x135;
            public const int CTLCOLORDLG = 0x136;
            public const int CTLCOLORSCROLLBAR = 0x137;
            public const int CTLCOLORSTATIC = 0x138;

            public const int MOUSEFIRST = 0x200;
            public const int MOUSEMOVE = 0x200;
            public const int LBUTTONDOWN = 0x201;
            public const int LBUTTONUP = 0x202;
            public const int LBUTTONDBLCLK = 0x203;
            public const int RBUTTONDOWN = 0x204;
            public const int RBUTTONUP = 0x205;
            public const int RBUTTONDBLCLK = 0x206;
            public const int MBUTTONDOWN = 0x207;
            public const int MBUTTONUP = 0x208;
            public const int MBUTTONDBLCLK = 0x209;
            public const int MOUSEWHEEL = 0x20A;
            public const int MOUSEHWHEEL = 0x20E;

            public const int PARENTNOTIFY = 0x210;
            public const int ENTERMENULOOP = 0x211;
            public const int EXITMENULOOP = 0x212;
            public const int NEXTMENU = 0x213;
            public const int SIZING = 0x214;
            public const int CAPTURECHANGED = 0x215;
            public const int MOVING = 0x216;
            public const int POWERBROADCAST = 0x218;
            public const int DEVICECHANGE = 0x219;

            public const int MDICREATE = 0x220;
            public const int MDIDESTROY = 0x221;
            public const int MDIACTIVATE = 0x222;
            public const int MDIRESTORE = 0x223;
            public const int MDINEXT = 0x224;
            public const int MDIMAXIMIZE = 0x225;
            public const int MDITILE = 0x226;
            public const int MDICASCADE = 0x227;
            public const int MDIICONARRANGE = 0x228;
            public const int MDIGETACTIVE = 0x229;
            public const int MDISETMENU = 0x230;
            public const int ENTERSIZEMOVE = 0x231;
            public const int EXITSIZEMOVE = 0x232;
            public const int DROPFILES = 0x233;
            public const int MDIREFRESHMENU = 0x234;

            public const int IME_SETCONTEXT = 0x281;
            public const int IME_NOTIFY = 0x282;
            public const int IME_CONTROL = 0x283;
            public const int IME_COMPOSITIONFULL = 0x284;
            public const int IME_SELECT = 0x285;
            public const int IME_CHAR = 0x286;
            public const int IME_KEYDOWN = 0x290;
            public const int IME_KEYUP = 0x291;

            public const int MOUSEHOVER = 0x2A1;
            public const int NCMOUSELEAVE = 0x2A2;
            public const int MOUSELEAVE = 0x2A3;

            public const int CUT = 0x300;
            public const int COPY = 0x301;
            public const int PASTE = 0x302;
            public const int CLEAR = 0x303;
            public const int UNDO = 0x304;

            public const int RENDERFORMAT = 0x305;
            public const int RENDERALLFORMATS = 0x306;
            public const int DESTROYCLIPBOARD = 0x307;
            public const int DRAWCLIPBOARD = 0x308;
            public const int PAINTCLIPBOARD = 0x309;
            public const int VSCROLLCLIPBOARD = 0x30A;
            public const int SIZECLIPBOARD = 0x30B;
            public const int ASKCBFORMATNAME = 0x30C;
            public const int CHANGECBCHAIN = 0x30D;
            public const int HSCROLLCLIPBOARD = 0x30E;
            public const int QUERYNEWPALETTE = 0x30F;
            public const int PALETTEISCHANGING = 0x310;
            public const int PALETTECHANGED = 0x311;

            public const int HOTKEY = 0x312;
            public const int PRINT = 0x317;
            public const int PRINTCLIENT = 0x318;

            public const int HANDHELDFIRST = 0x358;
            public const int HANDHELDLAST = 0x35F;
            public const int PENWINFIRST = 0x380;
            public const int PENWINLAST = 0x38F;
            public const int COALESCE_FIRST = 0x390;
            public const int COALESCE_LAST = 0x39F;
            public const int DDE_FIRST = 0x3E0;
            public const int DDE_INITIATE = 0x3E0;
            public const int DDE_TERMINATE = 0x3E1;
            public const int DDE_ADVISE = 0x3E2;
            public const int DDE_UNADVISE = 0x3E3;
            public const int DDE_ACK = 0x3E4;
            public const int DDE_DATA = 0x3E5;
            public const int DDE_REQUEST = 0x3E6;
            public const int DDE_POKE = 0x3E7;
            public const int DDE_EXECUTE = 0x3E8;
            public const int DDE_LAST = 0x3E8;

            public const int USER = 0x400;
            public const int APP = 0x8000;
        }
        public class GW
        {
            public const int HWNDFIRST = 0;
            public const int HWNDLAST = 1;
            public const int HWNDNEXT = 2;
            public const int HWNDPREV = 3;
            public const int OWNER = 4;
            public const int CHILD = 5;
            public const int ENABLEDPOPUP = 6;
        }
        public class SM
        {
            public const int CXBORDER = 5;
            public const int CXFULLSCREEN = 16;
            public const int CYFULLSCREEN = 17;
        }
        public class WH
        {
            public const int JOURNALRECORD = 0;
            public const int JOURNALPLAYBACK = 1;
            public const int KEYBOARD = 2;
            public const int GETMESSAGE = 3;
            public const int CALLWNDPROC = 4;
            public const int CBT = 5;
            public const int SYSMSGFILTER = 6;
            public const int MOUSE = 7;
            public const int HARDWARE = 8;
            public const int DEBUG = 9;
            public const int SHELL = 10;
            public const int FOREGROUNDIDLE = 11;
            public const int CALLWNDPROCRET = 12;
            public const int KEYBOARD_LL = 13;
            public const int MOUSE_LL = 14;
        }
        public class LVM
        {
            public const int REDRAWITEMS = 0x1000+21;
            public const int SCROLL=0x1000+20;
            public const int SETBKCOLOR = 0x1000+1;
            public const int SETBKIMAGEA = 0x1000+68;
            public const int SETBKIMAGEW = 0x1000+138;
            public const int SETCALLBACKMASK = 0x1000+11;
            public const int GETCALLBACKMASK = 0x1000+10;
            public const int GETCOLUMNORDERARRAY = 0x1000+59;
            public const int GETITEMCOUNT = 0x1000+4;
            public const int SETCOLUMNORDERARRAY = 0x1000+58;
            public const int SETINFOTIP = 0x1000+173;

            public const int SETIMAGELIST = 0x1000+3;
            public const int SETSELECTIONMARK = 0x1000+67;
            public const int SETTOOLTIPS = 0x1000 + 74;

            public const int GETITEMA = 0x1000+5;
            public const int GETITEMW = 0x1000+75;
            public const int SETITEMA = 0x1000+6;
            public const int SETITEMW = 0x1000+76;
            public const int SETITEMPOSITION32 = 0x01000 + 49;
            public const int INSERTITEMA = 0x1000+7;
            public const int INSERTITEMW = 0x1000+77;
            public const int DELETEITEM = 0x1000+8;
            public const int DELETECOLUMN = 0x1000+28;
            public const int DELETEALLITEMS = 0x1000+9;
            public const int UPDATE = 0x1000+42;

            public const int GETNEXTITEM = 0x1000+12;

            public const int SUBITEMHITTEST = 0x1000 + 57;
            public const int HITTEST = 0x1000+18;
            public const int ENSUREVISIBLE = 0x1000+19;

            public const int ARRANGE = 0x1000+22;
            public const int EDITLABELA = 0x1000+23;
            public const int EDITLABELW = 0x1000+118;

            public const int ENABLEGROUPVIEW = 0x1000 + 157;
            public const int MOVEITEMTOGROUP = 0x1000 + 154;
            public const int GETCOLUMNA = 0x1000+25;
            public const int GETCOLUMNW = 0x1000+95;
            public const int SETCOLUMNA = 0x1000+26;
            public const int SETCOLUMNW = 0x1000+96;
            public const int INSERTCOLUMNA = 0x1000+27;
            public const int INSERTCOLUMNW = 0x1000+97;
            public const int INSERTGROUP = 0x1000 + 145;
            public const int REMOVEGROUP = 0x1000 + 150;
            public const int INSERTMARKHITTEST = 0x1000 + 168;
            public const int REMOVEALLGROUPS = 0x1000 + 160;
            public const int GETCOLUMNWIDTH = 0x1000+29;
            public const int SETCOLUMNWIDTH = 0x1000+30;
            public const int SETINSERTMARK = 0x1000 + 166;
            public const int GETHEADER = 0x1000+31;
            public const int SETTEXTCOLOR = 0x1000+36;
            public const int SETTEXTBKCOLOR = 0x1000+38;
            public const int GETTOPINDEX = 0x1000+39;
            public const int SETITEMPOSITION = 0x1000+15;
            public const int SETITEMSTATE = 0x1000+43;
            public const int GETITEMSTATE = 0x1000+44;
            public const int GETITEMTEXTA = 0x1000+45;
            public const int GETITEMTEXTW = 0x1000+115;
            public const int GETHOTITEM = 0x1000+61;
            public const int SETITEMTEXTA = 0x1000+46;
            public const int SETITEMTEXTW = 0x1000+116;
            public const int SETITEMCOUNT = 0x1000+47;
            public const int SORTITEMS = 0x1000+48;
            public const int GETSELECTEDCOUNT = 0x1000+50;
            public const int GETISEARCHSTRINGA = 0x1000+52;
            public const int GETISEARCHSTRINGW = 0x1000+117;
            public const int SETEXTENDEDLISTVIEWSTYLE = 0x1000+54;
            public const int SETVIEW = 0x1000 + 142;
            public const int GETGROUPINFO = 0x1000 + 149;
            public const int SETGROUPINFO = 0x1000 + 147;
            public const int HASGROUP = 0x1000 + 161;
            public const int SETTILEVIEWINFO = 0x1000 + 162;
            public const int GETTILEVIEWINFO = 0x1000 + 163;        
            public const int GETINSERTMARK = 0x1000 + 167;
            public const int GETINSERTMARKRECT = 0x1000 + 169;
            public const int SETINSERTMARKCOLOR = 0x1000 + 170;
            public const int GETINSERTMARKCOLOR = 0x1000 + 171;        
            public const int ISGROUPVIEWENABLED = 0x1000 + 175;
        }
        public class LVIF
        {
            public const int TEXT = 0x0001;
            public const int IMAGE = 0x0002;
            public const int INDENT = 0x0010;
            public const int PARAM = 0x0004;
            public const int STATE = 0x0008;
            public const int GROUPID = 0x0100;
            public const int COLUMNS = 0x0200;
        }
        public class LVIS
        {
            public const int FOCUSED = 0x0001;
            public const int SELECTED = 0x0002;
            public const int CUT = 0x0004;
            public const int DROPHILITED = 0x0008;
            public const int OVERLAYMASK = 0x0F00;
            public const int STATEIMAGEMASK = 0xF000;
        }
        public class BM
        {
            public const int CLICK = 0x00F5;
        }
    }
}
