using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleRender
{
	public static class Renderer
	{
		private static SafeFileHandle fHandle;
		private static ScreenBuffer mainSBuffer;
		public static string MainSpriteDirectory { get; set; } = "spr";
		public static int PixelWidth { get; set; }
		public static int BufferWidth { get { return mainSBuffer.Size.X; } }
		public static int BufferHeight { get { return mainSBuffer.Size.Y; } }
		private const int FixedWidthTrueType = 54;
		private const int StandardOutputHandle = -11;
		private static readonly IntPtr ConsoleOutputHandle = External.GetStdHandle(StandardOutputHandle);


		//------------------------------------------------
		//		DLL Structs
		//______________________________________________/
		[StructLayout(LayoutKind.Sequential)]
		public struct Coord
		{
			public short X;
			public short Y;

			public Coord(short x, short y)
			{
				X = x;
				Y = y;
			}

			public static Coord operator /(Coord a, int scalar)
			{
				return new Coord((short)(a.X / scalar), (short)(a.Y / scalar));
			}
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct CharUnion
		{
			//Combo of char character and attributes. Attributes include bg colour, fg colour.
			[FieldOffset(0)] public char UnicodeChar;
			[FieldOffset(0)] public byte AsciiChar;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct CharInfo
		{
			[FieldOffset(0)] public CharUnion Char;
			[FieldOffset(2)] public short Attributes;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SmallRect
		{
			//Still not too sure what this is for, but it seems to define regions within buffers
			public short Left;
			public short Top;
			public short Right;
			public short Bottom;

			public SmallRect(short l, short t, short r, short b)
			{
				Left = l;
				Top = t;
				Right = r;
				Bottom = b;
			}
			public SmallRect(short r, short b)
			{
				this = new SmallRect(0, 0, r, b);
			}
		}
		
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct FontInfo
		{
			internal int cbSize;
			internal int FontIndex;
			internal short FontWidth;
			public short FontSize;
			public int FontFamily;
			public int FontWeight;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			//[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.wc, SizeConst = 32)]
			public string FontName;
		}

		//__________________________________________________________________________________________________
		//
		//			RENDER METHODS
		//__________________________________________________________________________________________________|

		//--------------------------------------------------------------------------------------------------/
		//	Assings console buffer to handle.
		//------------------------------------------------------------------------------------------------/
		private static bool CreateHandle()
		{
			fHandle = External.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

			return !fHandle.IsInvalid;
		}

		//--------------------------------------------------------------------------------------------------/
		//	Creates a new screen buffer for the main sbuffer
		//------------------------------------------------------------------------------------------------/
		private static void CreateMainBuffer(short width, short height)
		{
			if (width < 1 || height < 1)
				throw new Exception("Buffer size must be larger than 0 on all axes.");

			mainSBuffer = new ScreenBuffer(width, height);
		}

		//--------------------------------------------------------------------------------------------------/
		//	Calls all initialiation functions, along with matching window/buffer size to desired size.
		//------------------------------------------------------------------------------------------------/
		public static bool InitialiseRenderer(short width, short height, int pixelWidth, string fontName = "Consolas", short fontSize = 0)
		{
			PixelWidth = pixelWidth;
			if (!CreateHandle())
				return false;
			CreateMainBuffer((short)(width * pixelWidth), height);
			SetCurrentFont(fontName, fontSize);
			Console.SetWindowSize(width * pixelWidth, height);
			Console.SetBufferSize(width * pixelWidth, height);

			return true;
		}

		//--------------------------------------------------------------------------------------------------/
		//		Copies specified/main buffer to CONOUT$ handle
		//------------------------------------------------------------------------------------------------/
		public static void Render(ScreenBuffer sBuffer, bool clearBuffer = true)
		{
			RenderRegion(sBuffer, sBuffer.Size, new Coord(0, 0));

			if (clearBuffer)
				sBuffer.Clear();
		}
		public static void Render(bool clearBuffer = true)
		{
			Render(mainSBuffer, clearBuffer);
		}

		//Use this
		public static void RenderRegion(ScreenBuffer sBuffer, Coord regionSize, Coord regionCoord)
		{
			SmallRect newRect = new SmallRect(regionCoord.X, regionCoord.Y, (short)(regionSize.X + regionCoord.X), (short)(regionSize.Y + regionCoord.Y));

			External.WriteConsoleOutput(fHandle, sBuffer.GetBuffer(), regionSize, regionCoord, ref newRect);
		}

		//dont use this itll make you sad
		public static void RenderParallel(ScreenBuffer sBuffer, int threads)
		{
			Parallel.For(0, threads,
				index =>
				{
					Coord height = new Coord(sBuffer.Size.X, (short)(sBuffer.Size.Y / threads * (index + 1)));

					Coord regionCoord = new Coord(0, (short)(sBuffer.Size.Y / threads * index));

					RenderRegion(sBuffer, height, regionCoord);
				});
		}

		//Sets font by name and size, https://stackoverflow.com/questions/52356843/setcurrentconsolefontex-isnt-working-for-long-font-names
		public static FontInfo[] SetCurrentFont(string font, short fontSize = 0)
		{
			//Console.WriteLine("Set Current Font: " + font);

			FontInfo before = new FontInfo
			{
				cbSize = Marshal.SizeOf<FontInfo>()
			};

			if (External.GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before))
			{

				FontInfo set = new FontInfo
				{
					cbSize = Marshal.SizeOf<FontInfo>(),
					FontIndex = 0,
					FontFamily = FixedWidthTrueType,
					FontName = font,
					FontWeight = 400,
					FontSize = fontSize > 0 ? fontSize : before.FontSize
				};

				// Get some settings from current font.
				if (!External.SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref set))
				{
					var ex = Marshal.GetLastWin32Error();
					Console.WriteLine("Set error " + ex);
					throw new System.ComponentModel.Win32Exception(ex);
				}

				FontInfo after = new FontInfo
				{
					cbSize = Marshal.SizeOf<FontInfo>()
				};
				External.GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref after);

				return new[] { before, set, after };
			}
			else
			{
				var er = Marshal.GetLastWin32Error();
				Console.WriteLine("Get error " + er);
				throw new System.ComponentModel.Win32Exception(er);
			}
		}


		//__________________________________________________________________________________________________
		//
		//		DRAWING METHODS
		//_________________________________________________________________________________________________|

		//---------------------------------------------------------------------------------------------------/
		//	Sets pixel of screeen buffer at position, with specified character and colours.
		//	Will fail if the buffer isn't initialised.
		//------------------------------------------------------------------------------------------------/
		public static void DrawPixel(ScreenBuffer sBuffer, int x, int y, char character, ConsoleColor bgCol, ConsoleColor fgCol)
		{
			try
			{
				for (int i = 0; i < PixelWidth; ++i)
				{
					sBuffer.SetPixel(x * PixelWidth + i, y, character, bgCol, fgCol);
				}
			}
			catch (Exception)
			{
				throw new Exception("Sbuffer must be initialised.");
			}
		}
		public static void DrawPixel(int x, int y, char character, ConsoleColor bgCol, ConsoleColor fgCol)
		{
			DrawPixel(mainSBuffer, x, y, character, bgCol, fgCol);
		}

		//--------------------------------------------------------------------------------------------------/
		//		Iterates along a vector direction, drawing pixels that are snapped to the grid.
		//		Temporary version until Bresenham stuff can work.
		//------------------------------------------------------------------------------------------------/
		public static void DrawLine(ScreenBuffer sBuffer, int startX, int startY, int endX, int endY, char character, ConsoleColor bgCol, ConsoleColor fgCol)
		{
			int dx, dy, i;
			float len, vecX, vecY;


			dx = endX - startX;
			dy = endY - startY;

			len = (float)Math.Sqrt(dx * dx + dy * dy);

			vecX = dx / len;
			vecY = dy / len;

			//Iterates along unit vector
			for (i = 0; i < len; ++i)
			{
				DrawPixel(sBuffer, (int)Math.Floor(startX + vecX * i + 0.5f), (int)Math.Floor(startY + vecY * i + 0.5f), character, bgCol, fgCol);
			}
		}
		public static void DrawLine(int startX, int startY, int endX, int endY, char character, ConsoleColor bgCol, ConsoleColor fgCol)
		{
			DrawLine(mainSBuffer, startX, startY, endX, endY, character, bgCol, fgCol);
		}

		//--------------------------------------------------------------------------------------------------/
		//		Draws a rectangle with given top left/right and size values via loops
		//------------------------------------------------------------------------------------------------/
		public static void DrawRect(ScreenBuffer sBuffer, int tlX, int tlY, int width, int height, char character, ConsoleColor bgCol, ConsoleColor fgCol)
		{
			for (int x = 0; x < width; ++x)
			{
				for (int y = 0; y < height; ++y)
				{
					DrawPixel(sBuffer, tlX + x * Math.Sign(width), tlY + y * Math.Sign(height), character, bgCol, fgCol);
				}
			}
		}
		public static void DrawRect(int tlX, int tlY, int width, int height, char character, ConsoleColor bgCol, ConsoleColor fgCol)
		{
			DrawRect(mainSBuffer, tlX, tlY, width, height, character, bgCol, fgCol);
		}

		//__________________________________________________________________________________________________
		//
		//	Classes
		//_________________________________________________________________________________________________|
		public class ScreenBuffer
		{
			private Coord size;
			private CharInfo[] buffer;
			//Must be made public as a ref is used to access
			public SmallRect rect;

			//Cached variables
			int bufferPos;

			public Coord Size
			{
				get
				{
					return size;
				}
			}

			//Constructor
			public ScreenBuffer(short width, short height)
			{
				size.X = width;
				size.Y = height;
				buffer = new CharInfo[width * height];
				rect = new SmallRect(width, height);
			}

			//--------------------------------------------------------------------------------------------------/
			//		Easily set char_info with position, char and consolecolor
			//------------------------------------------------------------------------------------------------/
			public bool SetPixel(int x, int y, char text, ConsoleColor bgCol, ConsoleColor fgCol)
			{
				if (x < 0 || x >= size.X || y < 0 || y >= size.Y)
					return false;

				bufferPos = y * size.X + x;
				buffer[bufferPos].Char.UnicodeChar = text;
				buffer[bufferPos].Attributes = (short)((int)bgCol << 4 | ((int)fgCol));

				return true;
			}

			public void Clear()
			{
				for (int i = 0; i < buffer.Length; ++i)
				{
					buffer[i].Attributes = 0;
					buffer[i].Char.UnicodeChar = ' ';
				}
			}

			public CharInfo[] GetBuffer()
			{
				return buffer;
			}
		}

		private static class External
		{
			//--------------------------------------------------------------------------------------------/
			//	Methods imported from Kernel32. Allows direct access to the console, which c# .NET
			//	doesn't provide.
			//	http://www.pinvoke.net/default.aspx/kernel32/CreateFile.html
			//	Got the dll imports from here
			//--------------------------------------------------------------------------------------/

			//Imports C++ method to create file handles, used to reference "CONOUT$" (console output) so that it can be overwritten.
			[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
			public static extern SafeFileHandle CreateFile(
				string fileName,
				[MarshalAs(UnmanagedType.U4)] uint fileAccess,
				[MarshalAs(UnmanagedType.U4)] uint fileShare,
				IntPtr securityAttributes,
				[MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
				[MarshalAs(UnmanagedType.U4)] int flags,
				IntPtr template);

			//Imports another C++ method which allows overwriting buffers. Will be used to overwrite "CONOUT$" buffer.
			[DllImport("Kernel32.dll", SetLastError = true)]
			public static extern bool WriteConsoleOutput(
				SafeFileHandle hConsoleOutput,
				CharInfo[] lpBuffer,
				Coord dwBufferSize,
				Coord dwBufferCoord,
				ref SmallRect lpWriteRegion);

			[DllImport("kernel32.dll", SetLastError = true)]
			internal static extern IntPtr GetStdHandle(int nStdHandle);

			[return: MarshalAs(UnmanagedType.Bool)]
			[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
			internal static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);

			[return: MarshalAs(UnmanagedType.Bool)]
			[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
			internal static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);
		}
	}
}
