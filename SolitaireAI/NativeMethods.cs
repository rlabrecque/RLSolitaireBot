using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SolitaireAI {
	[System.Security.SuppressUnmanagedCodeSecurity()]
	internal sealed class NativeMethods {
		[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr memcpy(IntPtr dest, IntPtr src, long count);

		internal static bool IsWindowInForeground(IntPtr hWnd) {
			return hWnd == GetForegroundWindow();
		}

		#region user32
		[StructLayout(LayoutKind.Sequential)]
		public struct INPUT {
			internal InputType type;
			internal InputUnion U;
			internal static int Size {
				get { return Marshal.SizeOf(typeof(INPUT)); }
			}
		}

		internal enum InputType : uint {
			MOUSE = 0,
			KEYBOARD = 1,
			HARDWARE = 2
		}

		[StructLayout(LayoutKind.Explicit)]
		internal struct InputUnion {
			[FieldOffset(0)]
			internal MOUSEINPUT mi;
			[FieldOffset(0)]
			internal KEYBDINPUT ki;
			[FieldOffset(0)]
			internal HARDWAREINPUT hi;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct MOUSEINPUT {
			internal int dx;
			internal int dy;
			internal int mouseData;
			internal MOUSEEVENTF dwFlags;
			internal uint time;
			internal UIntPtr dwExtraInfo;
		}

		[Flags]
		internal enum MOUSEEVENTF : uint {
			ABSOLUTE = 0x8000,
			HWHEEL = 0x01000,
			MOVE = 0x0001,
			MOVE_NOCOALESCE = 0x2000,
			LEFTDOWN = 0x0002,
			LEFTUP = 0x0004,
			RIGHTDOWN = 0x0008,
			RIGHTUP = 0x0010,
			MIDDLEDOWN = 0x0020,
			MIDDLEUP = 0x0040,
			VIRTUALDESK = 0x4000,
			WHEEL = 0x0800,
			XDOWN = 0x0080,
			XUP = 0x0100
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct HARDWAREINPUT {
			internal int uMsg;
			internal short wParamL;
			internal short wParamH;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct KEYBDINPUT {
			internal VirtualKeyShort wVk;
			internal ScanCodeShort wScan;
			internal KEYEVENTF dwFlags;
			internal int time;
			internal UIntPtr dwExtraInfo;
		}

		[Flags]
		internal enum KEYEVENTF : uint {
			EXTENDEDKEY = 0x0001,
			KEYUP = 0x0002,
			SCANCODE = 0x0008,
			UNICODE = 0x0004
		}

		internal enum VirtualKeyShort : short {
			LBUTTON = 0x01,
			RBUTTON = 0x02,
			CANCEL = 0x03,
			MBUTTON = 0x04,
			XBUTTON1 = 0x05,
			XBUTTON2 = 0x06,
			BACK = 0x08,
			TAB = 0x09,
			CLEAR = 0x0C,
			RETURN = 0x0D,
			SHIFT = 0x10,
			CONTROL = 0x11,
			MENU = 0x12,
			PAUSE = 0x13,
			CAPITAL = 0x14,
			KANA = 0x15,
			HANGUL = 0x15,
			JUNJA = 0x17,
			FINAL = 0x18,
			HANJA = 0x19,
			KANJI = 0x19,
			ESCAPE = 0x1B,
			CONVERT = 0x1C,
			NONCONVERT = 0x1D,
			ACCEPT = 0x1E,
			MODECHANGE = 0x1F,
			SPACE = 0x20,
			PRIOR = 0x21,
			NEXT = 0x22,
			END = 0x23,
			HOME = 0x24,
			LEFT = 0x25,
			UP = 0x26,
			RIGHT = 0x27,
			DOWN = 0x28,
			SELECT = 0x29,
			PRINT = 0x2A,
			EXECUTE = 0x2B,
			SNAPSHOT = 0x2C,
			INSERT = 0x2D,
			DELETE = 0x2E,
			HELP = 0x2F,
			KEY_0 = 0x30,
			KEY_1 = 0x31,
			KEY_2 = 0x32,
			KEY_3 = 0x33,
			KEY_4 = 0x34,
			KEY_5 = 0x35,
			KEY_6 = 0x36,
			KEY_7 = 0x37,
			KEY_8 = 0x38,
			KEY_9 = 0x39,
			KEY_A = 0x41,
			KEY_B = 0x42,
			KEY_C = 0x43,
			KEY_D = 0x44,
			KEY_E = 0x45,
			KEY_F = 0x46,
			KEY_G = 0x47,
			KEY_H = 0x48,
			KEY_I = 0x49,
			KEY_J = 0x4A,
			KEY_K = 0x4B,
			KEY_L = 0x4C,
			KEY_M = 0x4D,
			KEY_N = 0x4E,
			KEY_O = 0x4F,
			KEY_P = 0x50,
			KEY_Q = 0x51,
			KEY_R = 0x52,
			KEY_S = 0x53,
			KEY_T = 0x54,
			KEY_U = 0x55,
			KEY_V = 0x56,
			KEY_W = 0x57,
			KEY_X = 0x58,
			KEY_Y = 0x59,
			KEY_Z = 0x5A,
			LWIN = 0x5B,
			RWIN = 0x5C,
			APPS = 0x5D,
			SLEEP = 0x5F,
			NUMPAD0 = 0x60,
			NUMPAD1 = 0x61,
			NUMPAD2 = 0x62,
			NUMPAD3 = 0x63,
			NUMPAD4 = 0x64,
			NUMPAD5 = 0x65,
			NUMPAD6 = 0x66,
			NUMPAD7 = 0x67,
			NUMPAD8 = 0x68,
			NUMPAD9 = 0x69,
			MULTIPLY = 0x6A,
			ADD = 0x6B,
			SEPARATOR = 0x6C,
			SUBTRACT = 0x6D,
			DECIMAL = 0x6E,
			DIVIDE = 0x6F,
			F1 = 0x70,
			F2 = 0x71,
			F3 = 0x72,
			F4 = 0x73,
			F5 = 0x74,
			F6 = 0x75,
			F7 = 0x76,
			F8 = 0x77,
			F9 = 0x78,
			F10 = 0x79,
			F11 = 0x7A,
			F12 = 0x7B,
			F13 = 0x7C,
			F14 = 0x7D,
			F15 = 0x7E,
			F16 = 0x7F,
			F17 = 0x80,
			F18 = 0x81,
			F19 = 0x82,
			F20 = 0x83,
			F21 = 0x84,
			F22 = 0x85,
			F23 = 0x86,
			F24 = 0x87,
			NUMLOCK = 0x90,
			SCROLL = 0x91,
			LSHIFT = 0xA0,
			RSHIFT = 0xA1,
			LCONTROL = 0xA2,
			RCONTROL = 0xA3,
			LMENU = 0xA4,
			RMENU = 0xA5,
			BROWSER_BACK = 0xA6,
			BROWSER_FORWARD = 0xA7,
			BROWSER_REFRESH = 0xA8,
			BROWSER_STOP = 0xA9,
			BROWSER_SEARCH = 0xAA,
			BROWSER_FAVORITES = 0xAB,
			BROWSER_HOME = 0xAC,
			VOLUME_MUTE = 0xAD,
			VOLUME_DOWN = 0xAE,
			VOLUME_UP = 0xAF,
			MEDIA_NEXT_TRACK = 0xB0,
			MEDIA_PREV_TRACK = 0xB1,
			MEDIA_STOP = 0xB2,
			MEDIA_PLAY_PAUSE = 0xB3,
			LAUNCH_MAIL = 0xB4,
			LAUNCH_MEDIA_SELECT = 0xB5,
			LAUNCH_APP1 = 0xB6,
			LAUNCH_APP2 = 0xB7,
			OEM_1 = 0xBA,
			OEM_PLUS = 0xBB,
			OEM_COMMA = 0xBC,
			OEM_MINUS = 0xBD,
			OEM_PERIOD = 0xBE,
			OEM_2 = 0xBF,
			OEM_3 = 0xC0,
			OEM_4 = 0xDB,
			OEM_5 = 0xDC,
			OEM_6 = 0xDD,
			OEM_7 = 0xDE,
			OEM_8 = 0xDF,
			OEM_102 = 0xE2,
			PROCESSKEY = 0xE5,
			PACKET = 0xE7,
			ATTN = 0xF6,
			CRSEL = 0xF7,
			EXSEL = 0xF8,
			EREOF = 0xF9,
			PLAY = 0xFA,
			ZOOM = 0xFB,
			NONAME = 0xFC,
			PA1 = 0xFD,
			OEM_CLEAR = 0xFE
		}

		internal enum ScanCodeShort : short {
			LBUTTON = 0,
			RBUTTON = 0,
			CANCEL = 70,
			MBUTTON = 0,
			XBUTTON1 = 0,
			XBUTTON2 = 0,
			BACK = 14,
			TAB = 15,
			CLEAR = 76,
			RETURN = 28,
			SHIFT = 42,
			CONTROL = 29,
			MENU = 56,
			PAUSE = 0,
			CAPITAL = 58,
			KANA = 0,
			HANGUL = 0,
			JUNJA = 0,
			FINAL = 0,
			HANJA = 0,
			KANJI = 0,
			ESCAPE = 1,
			CONVERT = 0,
			NONCONVERT = 0,
			ACCEPT = 0,
			MODECHANGE = 0,
			SPACE = 57,
			PRIOR = 73,
			NEXT = 81,
			END = 79,
			HOME = 71,
			LEFT = 75,
			UP = 72,
			RIGHT = 77,
			DOWN = 80,
			SELECT = 0,
			PRINT = 0,
			EXECUTE = 0,
			SNAPSHOT = 84,
			INSERT = 82,
			DELETE = 83,
			HELP = 99,
			KEY_0 = 11,
			KEY_1 = 2,
			KEY_2 = 3,
			KEY_3 = 4,
			KEY_4 = 5,
			KEY_5 = 6,
			KEY_6 = 7,
			KEY_7 = 8,
			KEY_8 = 9,
			KEY_9 = 10,
			KEY_A = 30,
			KEY_B = 48,
			KEY_C = 46,
			KEY_D = 32,
			KEY_E = 18,
			KEY_F = 33,
			KEY_G = 34,
			KEY_H = 35,
			KEY_I = 23,
			KEY_J = 36,
			KEY_K = 37,
			KEY_L = 38,
			KEY_M = 50,
			KEY_N = 49,
			KEY_O = 24,
			KEY_P = 25,
			KEY_Q = 16,
			KEY_R = 19,
			KEY_S = 31,
			KEY_T = 20,
			KEY_U = 22,
			KEY_V = 47,
			KEY_W = 17,
			KEY_X = 45,
			KEY_Y = 21,
			KEY_Z = 44,
			LWIN = 91,
			RWIN = 92,
			APPS = 93,
			SLEEP = 95,
			NUMPAD0 = 82,
			NUMPAD1 = 79,
			NUMPAD2 = 80,
			NUMPAD3 = 81,
			NUMPAD4 = 75,
			NUMPAD5 = 76,
			NUMPAD6 = 77,
			NUMPAD7 = 71,
			NUMPAD8 = 72,
			NUMPAD9 = 73,
			MULTIPLY = 55,
			ADD = 78,
			SEPARATOR = 0,
			SUBTRACT = 74,
			DECIMAL = 83,
			DIVIDE = 53,
			F1 = 59,
			F2 = 60,
			F3 = 61,
			F4 = 62,
			F5 = 63,
			F6 = 64,
			F7 = 65,
			F8 = 66,
			F9 = 67,
			F10 = 68,
			F11 = 87,
			F12 = 88,
			F13 = 100,
			F14 = 101,
			F15 = 102,
			F16 = 103,
			F17 = 104,
			F18 = 105,
			F19 = 106,
			F20 = 107,
			F21 = 108,
			F22 = 109,
			F23 = 110,
			F24 = 118,
			NUMLOCK = 69,
			SCROLL = 70,
			LSHIFT = 42,
			RSHIFT = 54,
			LCONTROL = 29,
			RCONTROL = 29,
			LMENU = 56,
			RMENU = 56,
			BROWSER_BACK = 106,
			BROWSER_FORWARD = 105,
			BROWSER_REFRESH = 103,
			BROWSER_STOP = 104,
			BROWSER_SEARCH = 101,
			BROWSER_FAVORITES = 102,
			BROWSER_HOME = 50,
			VOLUME_MUTE = 32,
			VOLUME_DOWN = 46,
			VOLUME_UP = 48,
			MEDIA_NEXT_TRACK = 25,
			MEDIA_PREV_TRACK = 16,
			MEDIA_STOP = 36,
			MEDIA_PLAY_PAUSE = 34,
			LAUNCH_MAIL = 108,
			LAUNCH_MEDIA_SELECT = 109,
			LAUNCH_APP1 = 107,
			LAUNCH_APP2 = 33,
			OEM_1 = 39,
			OEM_PLUS = 13,
			OEM_COMMA = 51,
			OEM_MINUS = 12,
			OEM_PERIOD = 52,
			OEM_2 = 53,
			OEM_3 = 41,
			OEM_4 = 26,
			OEM_5 = 43,
			OEM_6 = 27,
			OEM_7 = 40,
			OEM_8 = 0,
			OEM_102 = 86,
			PROCESSKEY = 0,
			PACKET = 0,
			ATTN = 0,
			CRSEL = 0,
			EXSEL = 0,
			EREOF = 93,
			PLAY = 0,
			ZOOM = 98,
			NONAME = 0,
			PA1 = 0,
			OEM_CLEAR = 0,
		}


		[DllImport("user32.dll")]
		public static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);

		[DllImport("user32.dll")]
		public static extern IntPtr GetDC(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		#region ShowWindow
		/// <summary>Shows a Window</summary>
		/// <remarks>
		/// <para>To perform certain special effects when showing or hiding a
		/// window, use AnimateWindow.</para>
		///<para>The first time an application calls ShowWindow, it should use
		///the WinMain function's nCmdShow parameter as its nCmdShow parameter.
		///Subsequent calls to ShowWindow must use one of the values in the
		///given list, instead of the one specified by the WinMain function's
		///nCmdShow parameter.</para>
		///<para>As noted in the discussion of the nCmdShow parameter, the
		///nCmdShow value is ignored in the first call to ShowWindow if the
		///program that launched the application specifies startup information
		///in the structure. In this case, ShowWindow uses the information
		///specified in the STARTUPINFO structure to show the window. On
		///subsequent calls, the application must call ShowWindow with nCmdShow
		///set to SW_SHOWDEFAULT to use the startup information provided by the
		///program that launched the application. This behavior is designed for
		///the following situations: </para>
		///<list type="">
		///    <item>Applications create their main window by calling CreateWindow
		///    with the WS_VISIBLE flag set. </item>
		///    <item>Applications create their main window by calling CreateWindow
		///    with the WS_VISIBLE flag cleared, and later call ShowWindow with the
		///    SW_SHOW flag set to make it visible.</item>
		///</list></remarks>
		/// <param name="hWnd">Handle to the window.</param>
		/// <param name="nCmdShow">Specifies how the window is to be shown.
		/// This parameter is ignored the first time an application calls
		/// ShowWindow, if the program that launched the application provides a
		/// STARTUPINFO structure. Otherwise, the first time ShowWindow is called,
		/// the value should be the value obtained by the WinMain function in its
		/// nCmdShow parameter. In subsequent calls, this parameter can be one of
		/// the WindowShowStyle members.</param>
		/// <returns>
		/// If the window was previously visible, the return value is nonzero.
		/// If the window was previously hidden, the return value is zero.
		/// </returns>
		[DllImport("user32.dll")]
		internal static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

		/// <summary>Enumeration of the different ways of showing a window using
		/// ShowWindow</summary>
		internal enum WindowShowStyle : uint {
			/// <summary>Hides the window and activates another window.</summary>
			/// <remarks>See SW_HIDE</remarks>
			Hide = 0,
			/// <summary>Activates and displays a window. If the window is minimized
			/// or maximized, the system restores it to its original size and
			/// position. An application should specify this flag when displaying
			/// the window for the first time.</summary>
			/// <remarks>See SW_SHOWNORMAL</remarks>
			ShowNormal = 1,
			/// <summary>Activates the window and displays it as a minimized window.</summary>
			/// <remarks>See SW_SHOWMINIMIZED</remarks>
			ShowMinimized = 2,
			/// <summary>Activates the window and displays it as a maximized window.</summary>
			/// <remarks>See SW_SHOWMAXIMIZED</remarks>
			ShowMaximized = 3,
			/// <summary>Maximizes the specified window.</summary>
			/// <remarks>See SW_MAXIMIZE</remarks>
			Maximize = 3,
			/// <summary>Displays a window in its most recent size and position.
			/// This value is similar to "ShowNormal", except the window is not
			/// actived.</summary>
			/// <remarks>See SW_SHOWNOACTIVATE</remarks>
			ShowNormalNoActivate = 4,
			/// <summary>Activates the window and displays it in its current size
			/// and position.</summary>
			/// <remarks>See SW_SHOW</remarks>
			Show = 5,
			/// <summary>Minimizes the specified window and activates the next
			/// top-level window in the Z order.</summary>
			/// <remarks>See SW_MINIMIZE</remarks>
			Minimize = 6,
			/// <summary>Displays the window as a minimized window. This value is
			/// similar to "ShowMinimized", except the window is not activated.</summary>
			/// <remarks>See SW_SHOWMINNOACTIVE</remarks>
			ShowMinNoActivate = 7,
			/// <summary>Displays the window in its current size and position. This
			/// value is similar to "Show", except the window is not activated.</summary>
			/// <remarks>See SW_SHOWNA</remarks>
			ShowNoActivate = 8,
			/// <summary>Activates and displays the window. If the window is
			/// minimized or maximized, the system restores it to its original size
			/// and position. An application should specify this flag when restoring
			/// a minimized window.</summary>
			/// <remarks>See SW_RESTORE</remarks>
			Restore = 9,
			/// <summary>Sets the show state based on the SW_ value specified in the
			/// STARTUPINFO structure passed to the CreateProcess function by the
			/// program that started the application.</summary>
			/// <remarks>See SW_SHOWDEFAULT</remarks>
			ShowDefault = 10,
			/// <summary>Windows 2000/XP: Minimizes a window, even if the thread
			/// that owns the window is hung. This flag should only be used when
			/// minimizing windows from a different thread.</summary>
			/// <remarks>See SW_FORCEMINIMIZE</remarks>
			ForceMinimized = 11
		}
		#endregion

		public delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern bool EnumThreadWindows(uint dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWindowVisible(IntPtr hWnd);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		public delegate bool EnumedWindow(IntPtr handleWindow, ArrayList handles);
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool EnumWindows(EnumedWindow lpEnumFunc, ArrayList lParam);

		[DllImport("user32.dll")]
		internal static extern IntPtr GetDesktopWindow();

		/// <summary>
		/// The GetForegroundWindow function returns a handle to the foreground window.
		/// </summary>
		[DllImport("user32.dll")]
		internal static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool IsIconic(IntPtr hWnd);

		#endregion
	}
}
