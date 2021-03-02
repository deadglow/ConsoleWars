using System;
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
		public static Unit[] unitList;
		public static Surface[] surfaceList;

		static Team currentTurn;
		static int playerCount = 1;

		public static void Init()
		{

		}
		
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
