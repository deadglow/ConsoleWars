using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRender;

namespace ConsoleWars
{
	class Map
	{
		public string Name { get; }
		public int Width { get; }
		public int Height { get; }
		Surface[,] surfaces;
		public int TileSize { get; set; }

		public Map(int width, int height, int tileSize = 16, string name = "Map")
		{
			Width = width;
			Height = height;
			TileSize = tileSize;
			Name = name;
		}

		//Getters
		public Surface GetSurface(int x, int y)
		{
			return surfaces[x, y];
		}
		public Surface GetSurface(float x, float y)
		{
			return GetSurface((int)x, (int)y);
		}
		public Surface GetSurface(Vector2 pos)
		{
			return GetSurface(pos.x, pos.y);
		}


		//Creates a 2d array of surfaces and fills it with given surface
		public void InitialiseMap(params Surface[] fill)
		{
			Random rand = new Random();
			surfaces = new Surface[Width, Height];
			for (int i = 0; i < surfaces.GetLength(0); ++i)
			{
				for (int j = 0; j < surfaces.GetLength(1); ++j)
				{
					surfaces[i, j] = (Surface)fill[rand.Next(0, fill.Length)].Clone(new Vector2(i, j));
				}
			}
		}

		public void DrawRegion(Vector2 position, Vector2 topLeft, Vector2 size)
		{
			if (topLeft.x >= Width || topLeft.y >= Height)
				return;

			Vector2 roundedTopLeft = new Vector2((int)topLeft.x, (int)topLeft.y);
			

			for (int y = 0; y < Math.Min(size.y + 1, Height - roundedTopLeft.y); ++y)
			{
				for (int x = 0; x < Math.Min(size.x + 1, Width - roundedTopLeft.x); ++x)
				{
					surfaces[(int)(roundedTopLeft.x + x), (int)(roundedTopLeft.y + y)].Draw((int)position.x + x * TileSize, (int)position.y + y * TileSize);
				}
			}
		}
		public void DrawRegion(int posX, int posY, int tlx, int tly, int rangeX, int rangeY)
		{
			DrawRegion(new Vector2(posX, posY), new Vector2(tlx, tly), new Vector2(rangeX, rangeY));
		}
		


	}
}
