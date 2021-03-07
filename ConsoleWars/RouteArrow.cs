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
			{0, 11, 12, 13, 14 },
			{11, 9, 6, 0, 7 },
			{12, 8, 10, 7, 0 },
			{13, 0, 5, 9, 8 },
			{14, 5, 0, 6, 10 }
		};
		AnimatedSprite sprite;
		List<Direction> points = new List<Direction>();
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
			for (int i = 0; i < moveArrowNames.Length; ++i)
			{
				moveArrowNames[i] = string.Concat("movearrow_", i + 1, ".spr");
			}

			sprite = new AnimatedSprite(moveArrowNames, "Move Arrow");
			Graphics.GlobalAnimSpriteList.Add("Move Arrow", sprite);
			sprite.Speed = 0;
		}

		public void AddPoint(Direction dir)
		{
			if (dir == Direction.None)
				return;

			if (points.Count > 0)
			{
				if (points.Last() != Direction.None && Math.Abs((int)dir - (int)points.Last()) == 2)
					points.RemoveAt(points.Count - 1);
				else
					points.Add(dir);
			}
		}
		public void AddPoint(Vector2 delta)
		{
			int newDir = 0;
			if (delta.x != 0)
			{
				newDir = 2 + Math.Sign(-delta.x);
			}
			if (delta.y != 0)
			{
				newDir = 3 + Math.Sign(-delta.y);
			}

			AddPoint((Direction)newDir);
		}

		public void Clear()
		{
			points.Clear();
		}

		public void SetStart(Vector2 pos)
		{
			StartPos = pos;
			Clear();
			points.Add(Direction.None);
		}

		public void DrawArrow()
		{
			int x = 0;
			int y = 0;
			for (int i = 1; i < points.Count; ++i)
			{
				switch (points[i])
				{
					case Direction.East:
						x++;
						break;
					case Direction.West:
						x--;
						break;
					case Direction.North:
						y++;
						break;
					case Direction.South:
						y--;
						break;
					default:
						break;
				}


				if (points[i] != Direction.None)
				{
					Vector2 newPos = StartPos * TileSize + new Vector2(x, y) * TileSize;
					int index = spriteIndex[(int)points[i], (int)points[i - 1]];
					if (index != 0)
						sprite.Draw(index - 1, (int)newPos.x, (int)newPos.y, 1, 1);
				}
			}
		}
	}
}
