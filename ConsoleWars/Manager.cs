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
		
		public static void Update()
		{
			++elapsedTicks;
		}

		public static void LoadUnitListFromFolder()
		{

		}

		public static class Graphics
		{
			public static Dictionary<string, Sprite> GlobalSpriteList { get; } = new Dictionary<string, Sprite>();
			public static Dictionary<string, AnimatedSprite> GlobalAnimSpriteList { get; } = new Dictionary<string, AnimatedSprite>();

			public static void LoadSpritesFromFolder()
			{
				List<string> fileNames = Directory.GetFiles(Renderer.MainSpriteDirectory).ToList();
				fileNames.Sort();
				

				for (int i = 0; i < fileNames.Count; ++i)
				{
					if (fileNames[i].Contains(".spr"))
					{
						fileNames[i] = fileNames[i].Replace(Renderer.MainSpriteDirectory + "\\", "");
						Sprite newSprite = new Sprite(fileNames[i]);
						newSprite.LoadSpriteFromFile();
						GlobalSpriteList.Add(fileNames[i], newSprite);
					}
				}
			}

			public static void LoadAnimSpritesFromFolder()
			{

			}
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
