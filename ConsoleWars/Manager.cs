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

		//Convert Dir (int between 0-3, east, north, west, south) to Vec or vice versa
		public static Vector2 DirToVec(int i)
		{
			Vector2 newVec = new Vector2();
			switch (i)
			{
				case 0:
					newVec.x++;
					break;
				case 1:
					newVec.y--;
					break;
				case 2:
					newVec.x--;
					break;
				case 3:
					newVec.y++;
					break;
				default:
					break;
			}

			return newVec;
		}
		public static int VecToDir(Vector2 a)
		{
			int newDir = -1;
			if (a.x != 0)
				newDir = 1 - Math.Sign(a.x);

			if (a.y != 0)
				newDir = 2 + Math.Sign(a.y);

			return newDir;
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
