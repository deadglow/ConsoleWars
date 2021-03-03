using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRender;

namespace ConsoleWars
{
	class RouteArrow
	{
		int[,] spriteIndex =
		{
			{9, 5, 9, 8 },
			{5, 10, 6, 10 },
			{9, 6, 9, 7 },
			{8, 10, 7, 10 }
		};
		AnimatedSprite sprite;
		Stack<Direction> points = new Stack<Direction>();
		public int TileSize { get; set; } = 8;
		public Vector2 StartPos { get; private set; }
		public Vector2 EndPos { get; private set; }

		public enum Direction
		{
			None = 0,
			East = 1,
			North = 2,
			West = 3,
			South = 4
		}

		public void InitSprites()
		{
			string[] moveArrowNames = new string[14];
			for(int i = 0; i < moveArrowNames.Length; ++i)
			{
				moveArrowNames[i] = string.Concat("movearrow_", i + 1, ".spr");
			}

			sprite = new AnimatedSprite(moveArrowNames, "Move Arrow");
			Graphics.GlobalAnimSpriteList.Add("Move Arrow", sprite);
			sprite.Speed = 0;
		}

		public void AddPoint(Direction dir)
		{
			if (points.Count > 0)
			{
				if (((int)dir + (int)points.Peek()) % 5 == 0)
					points.Pop();
				else
					points.Push(dir);
			}
		}

		public void Clear()
		{
			points.Clear();
		}

		public void SetStart(Vector2 pos)
		{
			StartPos = pos;
			Clear();
			points.Push(Direction.None);
			UpdateEnd();
		}

		void UpdateEnd()
		{
			float x = 0;
			float y = 0;
			if (points.Count > 0)
			{
				foreach(Direction dir in points)
				{
					
				}
			}

			EndPos = StartPos + new Vector2(x, y) * TileSize;
		}

		public void DrawArrow()
		{
			int x = 0;
			int y = 0;
			for(int i = 1; i < points.Count; ++i)
			{
				switch (points.ElementAt(i))
				{
					case Direction.East:
						x++;
						break;
					case Direction.West:
						x--;
						break;
					case Direction.North:
						y--;
						break;
					case Direction.South:
						y++;
						break;
					default:
						break;
				}

				Vector2 newPos = StartPos + new Vector2(x, y) * TileSize;
				sprite.Draw(spriteIndex[(int)points.ElementAt(i), (int)points.ElementAt(i - 1)], (int)newPos.x, (int)newPos.y, 1, 1);
			}
		}
	}
}
