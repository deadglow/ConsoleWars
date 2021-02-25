using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRender;

namespace ConsoleWars
{
	class AnimatedSprite
	{
		string[] spriteNames;
		Sprite[] sprites;
		public float Speed { get; set; }


		public AnimatedSprite(string[] spriteNames)
		{
			this.spriteNames = spriteNames;
			Initialise();
		}
		public AnimatedSprite(Sprite[] sprites)
		{
			this.sprites = sprites;
		}

		public void Initialise()
		{
			sprites = new Sprite[spriteNames.Length];

			for (int i = 0; i < sprites.Length; ++i)
			{
				sprites[i] = Manager.Graphics.GlobalSpriteList[spriteNames[i]];
			}
		}

		
		public void Draw(int frame, int x, int y, int flipX, int flipY)
		{
			sprites[frame].DrawSprite(x, y, flipX, flipY);
		}

		public void DrawAnimated(int x, int y, int flipX, int flipY)
		{
			Draw((int)Math.Floor(Manager.elapsedTicks * Speed % sprites.Length), x, y, flipX, flipY);
		}

	}
}
