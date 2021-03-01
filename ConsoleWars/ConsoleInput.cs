using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace ConsoleWars
{
	class ConsoleInput
	{

		//[DllImport("kernel32.dll", EntryPoint="ReadConsoleInput]
		//static extern bool ReadConsoleInput(
		//	IntPtr hConsoleInput,
		//	out input_record[] lpBuffer,
		//	uint nLength,
		//	out uint lpNumberOfEventsRead);
	}
}
