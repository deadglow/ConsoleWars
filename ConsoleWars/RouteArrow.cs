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
		//	Reference
		// 5 = L, 6 = ⅃, 7 = ⅂, 8 = Γ, 9 = -, 10 = |, 11 = >, 12 = ^, 13 = <, 14 = V
		//___________________________________________________________________________

		protected static int[,] spriteTable =
		{
			{0, 11, 12, 13, 14 },
			{11, 9, 6, 0, 7 },
			{12, 8, 10, 7, 0 },
			{13, 0, 5, 9, 8 },
			{14, 5, 0, 6, 10 }
		};
		protected AnimatedSprite sprite;
		protected List<Direction> points = new List<Direction>();
		public int TileSize { get; set; } = 8;
		public Vector2 StartPos { get; private set; }
		public Vector2 EndPos { get; private set; }

		public enum Direction
		{
			East = 1,
			North = 2,
			West = 3,
			South = 4
		}

		//--------------------------------------------------------------------------
		//	Creates a new anim sprite based on all movearrow sprites in folder
		//________________________________________________________________________
		public static void InitialiseArrowSprite()
		{
			//Creates an array of names following the form "movearrow_n.spr" where n ranges from 1-14 inclusive
			string[] moveArrowNames = new string[14];
			for (int i = 0; i < moveArrowNames.Length; ++i)
			{
				moveArrowNames[i] = string.Concat("movearrow_", i + 1, ".spr");
			}
			//Creates new animsprite with given sprite names
			AnimatedSprite newSprite = new AnimatedSprite(moveArrowNames, "Move Arrow");
			Graphics.GlobalAnimSpriteList.Add("Move Arrow", newSprite);
			newSprite.Speed = 0;
		}

		//--------------------------------------------------------------------------
		//	Simply assigns animSprite "Move Arrow" to this
		//________________________________________________________________________

		public void Initialise()
		{
			sprite = Graphics.GlobalAnimSpriteList["Move Arrow"];
		}

		//--------------------------------------------------------------------------
		//	Adds a new point with given direction
		//________________________________________________________________________
		public void AddPoint(Direction dir)
		{
			//If at least 1 element and if the previous direction is the opposite of the new direction, then remove it
			//Otherwise add the given direction
			if (points.Count > 0 && Math.Abs((int)dir - (int)points.Last()) == 2)
				points.RemoveAt(points.Count - 1);
			else
				points.Add(dir);
		}
		public void AddPoint(Vector2 delta)
		{
			//Assuming 0-4 represent east north west south
			//Offset rotation based on change in x or y. Y takes precedence
			//Wont add a point if no direction is given
			int newDir = 0;
			if (delta.x != 0)
			{
				newDir = 2 - Math.Sign(delta.x);
			}
			if (delta.y != 0)
			{
				newDir = 3 + Math.Sign(delta.y);
			}

			if (newDir > 0)
				AddPoint((Direction)newDir);
		}

		//--------------------------------------------------------------------------
		//	Clear all points
		//________________________________________________________________________
		public void Clear()
		{
			points.Clear();
		}

		//--------------------------------------------------------------------------
		//	Use when selecting a new unit
		//________________________________________________________________________
		public void SetStart(Vector2 pos)
		{
			StartPos = pos;
			Clear();
		}

		//--------------------------------------------------------------------------
		//	Loops through points, comparing previous and current direction and
		//	draws respective sprite index
		//________________________________________________________________________
		public void DrawArrow()
		{
			//Failsafe
			if (points.Count < 1)
				return;

			//Init draw pos to 0 and iterate after every draw
			Vector2 pos = new Vector2();
			Vector2 newPos;
			int oldDir, index;
			for (int i = 0; i <= points.Count; ++i)
			{
				newPos = (StartPos + pos) * TileSize;

				//Will only draw a end cap point based on the last direction in the list
				if (i == points.Count)
				{
					sprite.Draw(spriteTable[(int)points[i - 1], 0] - 1, (int)newPos.x, (int)newPos.y, 1, 1);
				}
				else
				{
					//Will set oldDir to 0 if its the first point on the array
					oldDir = (i == 0)? 0 : (int)points[i - 1];

					//Assigns to index from the sprite table, where x is the old direction and y is the current direction
					index = spriteTable[oldDir, (int)points[i]];

					//Changes from caps to start sprites
					if (index > 10)
						index -= 10;

					//Ignores if the index on the table is 0
					if (index != 0)
						sprite.Draw(index - 1, (int)newPos.x, (int)newPos.y, 1, 1);

					//Change x and y based on dir
					switch (points[i])
					{
						case Direction.East:
							pos.x++;
							break;
						case Direction.West:
							pos.x--;
							break;
						case Direction.North:
							pos.y--;
							break;
						case Direction.South:
							pos.y++;
							break;
						default:
							break;
					}
				}
			}
		}
	}
}
