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
		public string Name { get; }
		string[] spriteNames;
		Sprite[] sprites;
		public float Speed { get; set; }


		public AnimatedSprite(string[] spriteNames, string name = "Animated Sprite")
		{
			Name = name;
			this.spriteNames = spriteNames;
			Initialise();
		}
		public AnimatedSprite(Sprite[] sprites, string name = "Animated Sprite")
		{
			Name = name;
			this.sprites = sprites;
		}

		public Sprite GetSprite(int index)
		{
			return sprites[index];
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
			sprites[frame].DrawSpriteToCamera(Camera.main, x, y, flipX, flipY);
		}

		public void DrawAnimated(int x, int y, int flipX, int flipY)
		{
			Draw((int)Math.Floor(Manager.elapsedTicks * Speed % sprites.Length), x, y, flipX, flipY);
		}
		public void DrawAnimated(Vector2 pos, int flipX, int flipY)
		{
			DrawAnimated((int)pos.x, (int)pos.y, flipX, flipY);
		}

	}
}
