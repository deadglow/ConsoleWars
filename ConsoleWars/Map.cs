using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleWars
{
	class Map
	{
		string name;
		public int Width { get; }
		public int Height { get; }
		Surface[,] surfaces;
		public int TileSize { get; set; }

		public Map(int width, int height, int tileSize = 16, string name = "Map")
		{
			Width = width;
			Height = height;
			TileSize = tileSize;
			this.name = name;
			surfaces = new Surface[width, height];
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
					surfaces[i, j] = (Surface)fill[rand.Next(0, fill.Length)].Clone();
				}
			}
		}

		public void DrawRegion(Vector2 topLeft, Vector2 size)
		{
			if (topLeft.x >= Width || topLeft.y >= Height)
				return;

			int yLim = (int)Math.Ceiling(Math.Min(Height + topLeft.y, size.y) - topLeft.y);
			int xLim = (int)Math.Ceiling(Math.Min(Width + topLeft.x, size.x) - topLeft.x);

			for (int y = 0; y < size.y; ++y)
			{
				for (int x = 0; x < size.x; ++x)
				{
					surfaces[(int)topLeft.x + x, (int)topLeft.y + y].Draw(x * TileSize, y * TileSize);
				}
			}
		}
		public void DrawRegion(int tlx, int tly, int rangeX, int rangeY)
		{
			DrawRegion(new Vector2(tlx, tly), new Vector2(rangeX, rangeY));
		}
		


	}
}
