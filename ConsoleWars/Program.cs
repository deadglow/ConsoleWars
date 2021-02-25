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
		static Vector2 screenSize = new Vector2(240, 160);
		static ConsoleKeyInfo input;
		
		static void Main(string[] args)
		{
			Console.CursorVisible = false;


			bool quit = false;
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

			Renderer.MainSpriteDirectory = @"spr";
			Renderer.InitialiseRenderer((short)screenSize.x, (short)screenSize.y, 2, "Consolas", 6);

			AnimatedSprite newAnimSprite = new AnimatedSprite(new string[] { "infantry_idle_1.spr", "infantry_idle_2.spr", "infantry_idle_1.spr", "infantry_idle_1.spr"});
			newAnimSprite.Speed = 0.2f;

			Vector2 pos = Vector2.One;

			while(!quit)
			{

				switch(input.Key)
				{
					case ConsoleKey.RightArrow:
						++pos.x;
						break;

					case ConsoleKey.LeftArrow:
						--pos.x;
						break;

					case ConsoleKey.DownArrow:
						++pos.y;
						break;

					case ConsoleKey.UpArrow:
						--pos.y;
						break;

					default:
						break;
				}
				input = new ConsoleKeyInfo();

				Renderer.DrawRect(0, 0, (int)screenSize.x - 1, (int)screenSize.y - 1, ' ', ConsoleColor.Green, ConsoleColor.White);
				newAnimSprite.DrawAnimated((int)pos.x, (int)pos.y, 1, 1);

				Renderer.Render();

				Manager.Update();
				System.Threading.Thread.Sleep(1000 / 60);
			}

		}
	}
}
