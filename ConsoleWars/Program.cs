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

		static Vector2 curPos;
		
		static void Main(string[] args)
		{
			bool quit = false;

			Console.CursorVisible = false;

			Manager.Graphics.LoadSpritesFromFolder();
			Manager.Graphics.GlobalSpriteList.Add("grass2.spr", Manager.Graphics.GlobalSpriteList["grass.spr"].Colorize(ConsoleColor.Green, ConsoleColor.Blue));

			foreach(Sprite sprite in Manager.Graphics.GlobalSpriteList.Values)
			{
				Console.WriteLine(sprite.Name);
			}
			Console.ReadKey();

			Manager.Graphics.GlobalAnimSpriteList.Add("grass", new AnimatedSprite(new string[] { "grass.spr" }));
			Manager.Graphics.GlobalAnimSpriteList.Add("grass2", new AnimatedSprite(new string[] { "grass2.spr" }));
			Manager.Graphics.GlobalAnimSpriteList.Add("infantry", new AnimatedSprite(new string[] { "infantry_1.spr", "infantry_1.spr", "infantry_2.spr", "infantry_1.spr"}));
			Surface grass = new Surface("Grass", Manager.Graphics.GlobalAnimSpriteList["grass"], mainMap);
			Surface grass2 = new Surface("Grass2", Manager.Graphics.GlobalAnimSpriteList["grass2"], mainMap);

			mainMap = new Map((int)mapSize.x, (int)mapSize.y);
			mainMap.InitialiseMap(grass, grass2);

			Unit infantry = new Unit("Infantry", grass, Manager.Graphics.GlobalAnimSpriteList["infantry"]);
			infantry.sprite.Speed = 1f;

			infantry.Clone(mainMap.GetSurface(5, 7));
			infantry.Clone(mainMap.GetSurface(9, 4));
			infantry.Clone(mainMap.GetSurface(1, 1));
			infantry.Clone(mainMap.GetSurface(3, 7));

			Renderer.InitialiseRenderer((short)camSize.x, (short)camSize.y, 2, "Consolas", 6);

			Camera.main = new Camera(Vector2.Zero, camSize, mapSize * mainMap.TileSize);

			while(!quit)
			{
				while (!Console.KeyAvailable)
				{
					mainMap.DrawRegion(Camera.main.Position, camSize / mainMap.TileSize);

					Manager.Update();

					Renderer.Render();
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

					default:
						break;
				}

				Camera.main.Move(new Vector2(x, y));
			}
		}
	}
}
