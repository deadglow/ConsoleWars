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
		static Vector2 camSize = new Vector2(240, 160);
		static Vector2 mapSize = new Vector2(64, 64);
		static ConsoleKeyInfo input;
		static Vector2 camPos;
		static Map mainMap;
		
		static void Main(string[] args)
		{
			bool quit = false;
			

			Console.CursorVisible = false;

			Renderer.MainSpriteDirectory = "spr";
			Manager.Graphics.LoadSpritesFromFolder();
			Manager.Graphics.GlobalSpriteList.Add("grass2.spr", Manager.Graphics.GlobalSpriteList["grass.spr"].Colorize(ConsoleColor.Green, ConsoleColor.Blue));
			//AnimatedSprite newAnimSprite = new AnimatedSprite(new string[] { "infantry_idle_1.spr", "infantry_idle_2.spr", "infantry_idle_1.spr", "infantry_idle_1.spr" });
			//newAnimSprite.Speed = 0.2f;

			foreach(Sprite sprite in Manager.Graphics.GlobalSpriteList.Values)
			{
				Console.WriteLine(sprite.Name);
			}
			Console.ReadKey();

			Manager.Graphics.GlobalAnimSpriteList.Add("grass", new AnimatedSprite(new string[] { "grass.spr" }));
			Manager.Graphics.GlobalAnimSpriteList.Add("grass2", new AnimatedSprite(new string[] { "grass2.spr" }));
			Manager.Graphics.GlobalAnimSpriteList.Add("infantry", new AnimatedSprite(new string[] { "infantry_1.spr" }));
			Surface grass = new Surface("Grass", Manager.Graphics.GlobalAnimSpriteList["grass"], mainMap);
			Surface grass2 = new Surface("Grass2", Manager.Graphics.GlobalAnimSpriteList["grass2"], mainMap);


			mainMap = new Map((int)mapSize.x, (int)mapSize.y);
			mainMap.InitialiseMap(grass, grass2);

			Unit infantry = new Unit("Infantry", grass, Manager.Graphics.GlobalAnimSpriteList["infantry"]);
			Unit infantry1 = (Unit)infantry.Clone(mainMap.GetSurface(5, 7));
			Unit infantry2 = (Unit)infantry.Clone(mainMap.GetSurface(9, 40));
			Unit infantry3 = (Unit)infantry.Clone(mainMap.GetSurface(1, 1));
			Unit infantry4 = (Unit)infantry.Clone(mainMap.GetSurface(16, 7));


			Renderer.InitialiseRenderer((short)camSize.x, (short)camSize.y, 2, "Consolas", 6);


			new System.Threading.Thread(() =>
			{
				System.Threading.Thread.CurrentThread.IsBackground = true;
				input = Console.ReadKey();
				while (input.Key != ConsoleKey.Q)
				{
					input = Console.ReadKey();

				}
				quit = true;
			}).Start();

			while (!quit)
			{

				switch(input.Key)
				{
					case ConsoleKey.RightArrow:
						++camPos.x;
						break;

					case ConsoleKey.LeftArrow:
						--camPos.x;
						break;

					case ConsoleKey.DownArrow:
						++camPos.y;
						break;

					case ConsoleKey.UpArrow:
						--camPos.y;
						break;

					default:
						break;
				}
				input = new ConsoleKeyInfo();

				//newAnimSprite.DrawAnimated((int)camPos.x, (int)camPos.y, 1, 1);

				mainMap.DrawRegion(camPos, camSize / mainMap.TileSize);
				Manager.Update();

				Renderer.Render();

				System.Threading.Thread.Sleep(1000 / 60);
			}

		}
	}
}
