using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleRender
{
	public class Sprite
	{
		public string Name { get; }
		public string FileName { get; }
		private Pixel[] pixels;
		public int Width { get; private set; }
		public int Height { get; private set; }


		public Sprite(Pixel[] pixels, string fileName, string name = "Sprite")
		{
			Name = name;
			FileName = fileName;
			this.pixels = pixels;
		}
		public Sprite(string fileName, string name)
		{
			Name = name;
			FileName = fileName;
		}
		public Sprite(string fileName)
		{
			FileName = fileName;
			Name = fileName;
		}

		public Sprite Clone()
		{
			Sprite newSprite = (Sprite)MemberwiseClone();
			newSprite.pixels = (Pixel[])pixels.Clone();

			return newSprite;
		}

		public void ConvertTextToSprite(string text)
		{
			//Split text into rows
			string[] textRows = text.Split('\n');

			List<Pixel> newPixels = new List<Pixel>();

			for (int y = 0; y < textRows.Length; ++y)
			{
				//Split into columns
				string[] splitText = textRows[y].Split(' ');

				for (int x = 0; x < splitText.Length; ++x)
				{
					//Ignore if value given isn't an integer
					if (int.TryParse(splitText[x], out int returnedIndex))
					{
						//Run number below 1, since 0 index in file is 'transparent' and should be -1
						returnedIndex--;

						//Ignore 'transparent' pixels
						if (returnedIndex < 0)
							continue;

						//Updates height/width
						if (x >= Width)
							Width = x + 1;
						if (y >= Height)
							Height = y + 1;

						newPixels.Add(new Pixel(x, y, ' ', (ConsoleColor)returnedIndex, ConsoleColor.White));
					}
				}
			}

			pixels = newPixels.ToArray();
		}
		public void LoadSpriteFromFile(string directory)
		{
			string newText = File.ReadAllText(Path.Combine(directory, FileName));
			ConvertTextToSprite(newText);
		}
		public void LoadSpriteFromFile()
		{
			LoadSpriteFromFile(Renderer.MainSpriteDirectory);
		}


		public void DrawSprite(int x, int y, int flipX = 1, int flipY = 1)
		{
			//Changes drawing corner if flipped
			int newX = x + (Width - 1) * ((-flipX + 1) / 2);
			int newY = y + (Height - 1) * ((-flipY + 1) / 2);

			for (int i = 0; i < pixels.Length; ++i)
			{
				pixels[i].Draw(newX, newY, flipX, flipY);
			}
		}
		public void DrawSpriteToCamera(Camera cam, int x, int y, int flipX = 1, int flipY = 1)
		{
			DrawSprite(x - (int)cam.Position.x, y - (int)cam.Position.y, flipX, flipY);
		}

		public Sprite Colorize(params KeyValuePair<ConsoleColor, ConsoleColor>[] colourPairs)
		{
			Sprite newSprite = Clone();
			foreach (KeyValuePair<ConsoleColor, ConsoleColor> colourPair in colourPairs)
			{
				for (int i = 0; i < newSprite.pixels.Length; ++i)
				{
					if (pixels[i].bgCol == colourPair.Key)
						newSprite.pixels[i].bgCol = colourPair.Value;
				}
			}
			return newSprite;
		}

		public struct Pixel
		{
			int x;
			int y;
			char text;
			public ConsoleColor bgCol { get; set; }
			public ConsoleColor fgCol { get; set; }

			public Pixel(int x, int y, char text, ConsoleColor bgCol, ConsoleColor fgCol)
			{
				this.x = x;
				this.y = y;
				this.text = text;
				this.bgCol = bgCol;
				this.fgCol = fgCol;
			}

			public void Draw(int xOffset, int yOffset, int flipX, int flipY)
			{
				//Draw backwards if flipped on each axis
				Renderer.DrawPixel(xOffset + x * flipX, yOffset + y * flipY, text, bgCol, fgCol);
			}
		}
	}
}
