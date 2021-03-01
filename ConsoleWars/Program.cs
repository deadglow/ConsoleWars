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
		static Vector2 camSize = new Vector2(160, 144);
		static Vector2 mapSize = new Vector2(32, 32);
		static Map mainMap;

		static AnimatedSprite cursorSprite;
		static Vector2 curPos;
		static Unit selectedUnit;
		static Vector2 selectedUnitPos;
		
		static void Main(string[] args)
		{
			bool quit = false;

			Console.CursorVisible = false;

			Manager.Graphics.LoadSpritesFromFolder();
			Manager.Graphics.GlobalSpriteList.Add("grass2.spr", Manager.Graphics.GlobalSpriteList["grass.spr"].Colorize(ConsoleColor.Green, ConsoleColor.Blue));

			Manager.Graphics.GlobalAnimSpriteList.Add("grass", new AnimatedSprite(new string[] { "grass.spr" }));
			Manager.Graphics.GlobalAnimSpriteList.Add("grass2", new AnimatedSprite(new string[] { "grass2.spr" }));
			Manager.Graphics.GlobalAnimSpriteList.Add("infantry", new AnimatedSprite(new string[] { "infantry_1.spr", "infantry_1.spr", "infantry_2.spr", "infantry_1.spr"}));


			foreach (Sprite sprite in Manager.Graphics.GlobalSpriteList.Values)
			{
				Console.WriteLine(sprite.Name);
			}
			Console.ReadKey();

			mainMap = new Map((int)mapSize.x, (int)mapSize.y);
			Surface grass = new Surface("Grass", Manager.Graphics.GlobalAnimSpriteList["grass"], mainMap);
			Surface grass2 = new Surface("Grass2", Manager.Graphics.GlobalAnimSpriteList["grass2"], mainMap);

			mainMap.InitialiseMap(grass, grass2);

			Unit infantry = new Unit("Infantry", grass, Manager.Graphics.GlobalAnimSpriteList["infantry"]);
			infantry.sprite.Speed = 0.5f;

			infantry.Clone(mainMap.GetSurface(5, 7));
			infantry.Clone(mainMap.GetSurface(9, 4));
			infantry.Clone(mainMap.GetSurface(1, 1));
			infantry.Clone(mainMap.GetSurface(3, 7));

			Renderer.InitialiseRenderer((short)camSize.x, (short)camSize.y, 2, "Consolas", 6);

			Camera.main = new Camera(Vector2.Zero, camSize, mapSize * mainMap.TileSize);

			cursorSprite = new AnimatedSprite(new string[] { "cursor_1.spr", "cursor_1.spr", "cursor_2.spr" });
			cursorSprite.Speed = 0.5f;

			while(!quit)
			{
				while (!Console.KeyAvailable)
				{
					System.Diagnostics.Stopwatch frameTimer = new System.Diagnostics.Stopwatch();
					frameTimer.Start();

					Camera.main.MoveTo(Vector2.Lerp(Camera.main.Position, curPos * mainMap.TileSize - camSize / 2 + Vector2.One * mainMap.TileSize / 2, 0.7f));

					mainMap.DrawRegion(Camera.main.Position - (Camera.main.Position % mainMap.TileSize), Camera.main.Position / mainMap.TileSize, camSize / mainMap.TileSize);
					
					cursorSprite.DrawAnimated(curPos * mainMap.TileSize, 1, 1);

					if (selectedUnit != null)
					{
						cursorSprite.Draw(2, (int)selectedUnitPos.x * mainMap.TileSize, (int)selectedUnitPos.y * mainMap.TileSize, 1, 1);
					}
					



					Manager.Update();

					Renderer.Render();

					System.Threading.Thread.Sleep((int)Mathf.Clamp((float)(1000.0 / 30.0) - frameTimer.ElapsedMilliseconds, 0, (float)(1000.0 / 30.0)));
				}
				int x = 0;
				int y = 0;
				switch (Console.ReadKey().Key)
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
							if (selectedUnit.MoveUnit((int)curPos.x, (int)curPos.y))
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

					default:
						break;
				}

				curPos += new Vector2(x, y);
				
			}
		}
	}
}
