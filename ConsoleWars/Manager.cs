﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRender;

namespace ConsoleWars
{

	static class Manager
	{
		public static long elapsedTicks;
		public static Dictionary<string, Unit> GlobalUnitList { get; } = new Dictionary<string, Unit>();
		
		public static void Update()
		{
			++elapsedTicks;
		}

		public static void LoadUnitListFromFolder()
		{

		}
	}

	enum Team
	{
		White,
		Red,
		Blue,
		Green,
		Purple
	}
}
