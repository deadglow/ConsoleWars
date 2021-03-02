using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRender;

namespace ConsoleWars
{
	class Program
	{
		static Random random = new Random();
		static Vector2 camSize = new Vector2(100, 100);
		static Vector2 mapSize = new Vector2(32, 32);
		static int pixelSize = 2;
		static int tileSize = 8;
		static Map mainMap;

		static AnimatedSprite cursorSprite;
		static Vector2 curPos;
		static Unit selectedUnit;
		static Vector2 selectedUnitPos;
		
		static void Main(string[] args)
		{
			bool quit = false;
			Console.CursorVisible = false;

			Graphics.LoadSpritesFromFolder();


			Graphics.GlobalAnimSpriteList.Add("grass", new AnimatedSprite(new string[] { "grass.spr" }));
			Graphics.GlobalAnimSpriteList.Add("red_infantry", new AnimatedSprite(new string[] { "infantry_1.spr", "infantry_1.spr", "infantry_2.spr", "infantry_1.spr"}));
			Graphics.GlobalAnimSpriteList.Add("red_at_infantry", new AnimatedSprite(new string[] { "at_infantry_1.spr", "at_infantry_1.spr", "at_infantry_2.spr", "at_infantry_1.spr" }));
			Graphics.GlobalAnimSpriteList.Add("red_tank", new AnimatedSprite(new string[] { "tank_1.spr", "tank_2.spr" }));
			Graphics.GlobalAnimSpriteList.Add("cursor", new AnimatedSprite(new string[] { "cursor_1.spr", "cursor_1.spr", "cursor_2.spr" }));
			Graphics.GlobalAnimSpriteList.Add("tree", new AnimatedSprite(new string[] { "tree_1.spr", "tree_1.spr", "tree_1.spr", "tree_1.spr", "tree_2.spr" }));
			Graphics.GlobalAnimSpriteList["tree"].Speed = 0.1f;

			Graphics.GlobalAnimSpriteList.Add("blue_infantry", Graphics.GlobalAnimSpriteList["red_infantry"].Colorize( "blue_infantry",
			new KeyValuePair<ConsoleColor, ConsoleColor>(ConsoleColor.Red, ConsoleColor.Blue),
			new KeyValuePair<ConsoleColor, ConsoleColor>(ConsoleColor.DarkRed, ConsoleColor.DarkBlue
			)));
			Graphics.GlobalAnimSpriteList.Add("blue_tank", Graphics.GlobalAnimSpriteList["red_tank"].Colorize("blue_tank",
			new KeyValuePair<ConsoleColor, ConsoleColor>(ConsoleColor.Red, ConsoleColor.Blue),
			new KeyValuePair<ConsoleColor, ConsoleColor>(ConsoleColor.DarkRed, ConsoleColor.DarkBlue
			)));
			Graphics.GlobalAnimSpriteList.Add("blue_at_infantry", Graphics.GlobalAnimSpriteList["red_at_infantry"].Colorize("blue_at_infantry",
			new KeyValuePair<ConsoleColor, ConsoleColor>(ConsoleColor.Red, ConsoleColor.Blue),
			new KeyValuePair<ConsoleColor, ConsoleColor>(ConsoleColor.DarkRed, ConsoleColor.DarkBlue
			)));



			foreach (Sprite sprite in Graphics.GlobalSpriteList.Values)
			{
				Console.WriteLine(sprite.Name);
			}
			Console.ReadKey();

			mainMap = new Map((int)mapSize.x, (int)mapSize.y);
			mainMap.TileSize = tileSize;
			Surface[] surfaces = new Surface[]
			{
				new Surface("Grass", Graphics.GlobalAnimSpriteList["grass"], mainMap),
				new Surface("Tree", Graphics.GlobalAnimSpriteList["tree"], mainMap)
			};

			mainMap.InitialiseMap(surfaces);

			//Units
			Unit redInfantry = new Unit("Infantry", surfaces[0], Graphics.GlobalAnimSpriteList["red_infantry"]);
			redInfantry.SpriteSpeed = 0.5f;
			Unit blueInfantry = new Unit("Blue Infantry", surfaces[0], Graphics.GlobalAnimSpriteList["blue_infantry"]);
			blueInfantry.SpriteSpeed = 0.3f;
			Unit redTank = new Unit("Red Tank", surfaces[0], Graphics.GlobalAnimSpriteList["red_tank"]);
			redTank.SpriteSpeed = 0.5f;
			Unit blueTank = new Unit("Blue Tank", surfaces[0], Graphics.GlobalAnimSpriteList["blue_tank"]);
			blueTank.SpriteSpeed = 0.5f;
			Unit redAtInfantry = new Unit("Red AT", surfaces[0], Graphics.GlobalAnimSpriteList["red_at_infantry"]);
			redAtInfantry.SpriteSpeed = 0.3f;
			Unit blueAtInfantry = new Unit("Blue AT", surfaces[0], Graphics.GlobalAnimSpriteList["blue_at_infantry"]);
			blueAtInfantry.SpriteSpeed = 0.3f;

			Unit[] units = new Unit[]
			{
				redInfantry,
				blueInfantry,
				redTank,
				blueTank,
				redAtInfantry,
				blueAtInfantry
			};

			for (int i = 0; i < units.Length; ++i)
			{
				units[i].Clone(mainMap.GetSurface(i, i));
			}

			//for (int x = 0; x < mapSize.x; ++x)
			//{
			//	for (int y = 0; y < mapSize.y; ++y)
			//	{
			//		units[random.Next(0, units.Length)].Clone(mainMap.GetSurface(x, y));
			//	}
			//}

			Renderer.InitialiseRenderer((short)camSize.x, (short)camSize.y, pixelSize, "Consolas", 6);

			Camera.main = new Camera(Vector2.Zero, camSize, mapSize * mainMap.TileSize);

			cursorSprite = Graphics.GlobalAnimSpriteList["cursor"];
			cursorSprite.Speed = 0.5f;

			while(!quit)
			{
				while (!Console.KeyAvailable)
				{
					System.Diagnostics.Stopwatch frameTimer = new System.Diagnostics.Stopwatch();
					frameTimer.Start();

					Camera.main.MoveTo(Vector2.Lerp(Camera.main.Position, curPos * mainMap.TileSize - camSize / 2 + Vector2.One * mainMap.TileSize / 2, 0.55f));

					mainMap.DrawRegion(Camera.main.Position - (Camera.main.Position % mainMap.TileSize), Camera.main.Position / mainMap.TileSize, camSize / mainMap.TileSize);
					
					cursorSprite.DrawAnimated(curPos * mainMap.TileSize, 1, 1);

					if (selectedUnit != null)
					{
						cursorSprite.Draw(0, (int)selectedUnitPos.x * mainMap.TileSize, (int)selectedUnitPos.y * mainMap.TileSize, 1, 1);
					}

					Manager.Update();

					Renderer.Render();

					System.Threading.Thread.Sleep((int)Mathf.Clamp((float)(1000.0 / 30.0) - frameTimer.ElapsedMilliseconds, 0, (float)(1000.0 / 30.0)));
				}

				int x = 0, y = 0;

				switch (Console.ReadKey(true).Key)
				{
					case ConsoleKey.RightArrow:
						++x;
						break;

					case ConsoleKey.LeftArrow:
						--x;
						break;

					case ConsoleKey.DownArrow:
						++y;
						break;

					case ConsoleKey.UpArrow:
						--y;
						break;

					case ConsoleKey.Z:
						if (selectedUnit != null)
						{
							if (selectedUnit.ParentSurface != null)
							{
								if (selectedUnit.MoveUnit((int)curPos.x, (int)curPos.y))
								{
									selectedUnit = null;
								}
							}
							else
							{
								selectedUnit = null;
							}
						}
						else
						{
							selectedUnit = mainMap.GetSurface(curPos).CurrentUnit;
							selectedUnitPos = curPos;
						}
						break;

					case ConsoleKey.X:
						if (selectedUnit != null)
						{
							selectedUnit.FlipX = -selectedUnit.FlipX;
						}
						break;

					case ConsoleKey.A:
						if (mainMap.GetSurface(curPos).CurrentUnit == null)
						{
							units[random.Next(0, units.Length)].Clone(mainMap.GetSurface(curPos));
						}
						break;

					case ConsoleKey.F:
						if (mainMap.GetSurface(curPos).CurrentUnit != null)
						{
							mainMap.GetSurface(curPos).CurrentUnit.Destroy();
						}
						break;

					default:
						break;
				}

				curPos += new Vector2(x, y);
				curPos = new Vector2(Mathf.Clamp(curPos.x, 0, mapSize.x - 1), Mathf.Clamp(curPos.y, 0, mapSize.y - 1));
				
			}
		}
	}
}
