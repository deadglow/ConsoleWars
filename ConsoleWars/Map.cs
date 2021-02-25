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
		int width;
		int height;
		Surface[,] surfaces;
		public int TileSize { get; set; }

		public Map(int width, int height, int tileSize = 16, string name = "Map")
		{
			this.width = width;
			this.height = height;
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
		public void InitialiseMap(Surface fill)
		{
			surfaces = new Surface[width, height];
			for (int i = 0; i < surfaces.GetLength(0); ++i)
			{
				for (int j = 0; j < surfaces.GetLength(1); ++j)
				{
					surfaces[i, j] = (Surface)fill.Clone();
				}
			}
		}

		public void DrawRegion(int tlx, int tly, int rangeX, int rangeY)
		{
			if (tlx >= width || tly >= height)
				return;

			int yLim = Math.Min(height + tly, rangeY) - tly;
			int xLim = Math.Min(width + tlx, rangeX) - tlx;

			for (int y = tly; y < yLim; ++y)
			{
				for (int x = tlx; x < xLim; ++x)
				{
					//surfaces[x, y];
				}
			}
		}
		


	}
}
