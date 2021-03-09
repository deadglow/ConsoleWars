using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleWars;

namespace ConsoleRender
{
	public class AnimatedSprite
	{
		public string Name { get; }
		string[] spriteNames;
		Sprite[] sprites;
		public float Speed { get; set; }
		public int FrameOffset { get; set; }


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

		public AnimatedSprite Clone()
		{
			AnimatedSprite newSprite = (AnimatedSprite)this.MemberwiseClone();
			newSprite.spriteNames = (string[])spriteNames.Clone();
			newSprite.sprites = (Sprite[])sprites.Clone();

			return newSprite;
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
				sprites[i] = Graphics.GlobalSpriteList[spriteNames[i]];
			}
		}

		public AnimatedSprite Colorize(string name, params KeyValuePair<ConsoleColor, ConsoleColor>[] colourPairs)
		{
			AnimatedSprite newSprite = Clone();
			
			for (int i = 0; i < newSprite.sprites.Length; ++i)
			{
				newSprite.sprites[i] = sprites[i].Colorize(colourPairs);
				Graphics.GlobalSpriteList.Add(string.Concat(name, "_", i + 1, ".spr"), newSprite.sprites[i]);
			}

			return newSprite;
		}

		
		public void Draw(int frame, int x, int y, int flipX, int flipY)
		{
			sprites[frame].DrawSpriteToCamera(Camera.main, x, y, flipX, flipY);
		}

		public void DrawAnimated(int x, int y, int flipX, int flipY, float speed)
		{
			Draw((int)Math.Floor((Manager.elapsedTicks * speed + FrameOffset) % sprites.Length), x, y, flipX, flipY);
		}
		public void DrawAnimated(Vector2 pos, int flipX, int flipY, float speed)
		{
			DrawAnimated((int)pos.x, (int)pos.y, flipX, flipY, speed);
		}
		public void DrawAnimated(int x, int y, int flipX, int flipY)
		{
			DrawAnimated(x, y, flipX, flipY, Speed);
		}
		public void DrawAnimated(Vector2 pos, int flipX, int flipY)
		{
			DrawAnimated((int)pos.x, (int)pos.y, flipX, flipY);
		}

	}
}
