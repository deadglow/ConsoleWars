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
		
		public static void Update()
		{
			++elapsedTicks;
		}

		public static class Graphics
		{
			public static Dictionary<string,Sprite> globalSpriteList = new Dictionary<string, Sprite>();

			public static void LoadSpritesFromFolder()
			{
				List<string> fileNames = Directory.GetFiles(Renderer.MainSpriteDirectory).ToList<string>();
				fileNames.Sort();
				

				for (int i = 0; i < fileNames.Count; ++i)
				{
					if (fileNames[i].Contains(".spr"))
					{
						globalSpriteList.Add(fileNames[i], new Sprite(fileNames[i]));
					}
				}
			}
		}

	}
}
