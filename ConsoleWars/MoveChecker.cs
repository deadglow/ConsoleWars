using System;
using System.Collections.Generic;
using ConsoleRender;

namespace ConsoleWars
{
	class MoveChecker
	{
		public Move[,] PossibleMoves { get; protected set; }
		public Map mainMap;

		public void FindPossibleMoves(Vector2 pos, Unit unit)
		{
			for (int i = 0; i < PossibleMoves.Length; i++)
			{
				PossibleMoves.SetValue(null, i);
			}
			//CheckTile(pos, unit.MoveDistance, -1);
		}

		public static Vector2 DirToVec(int i)
		{
			Vector2 newVec = new Vector2();
			switch (i)
			{
				case 0:
					newVec.x++;
					break;
				case 1:
					newVec.y--;
					break;
				case 2:
					newVec.x--;
					break;
				case 3:
					newVec.y++;
					break;
				default:
					break;
			}

			return newVec;
		}
		public static int VecToDir(Vector2 a)
		{
			int newDir = 0;
			if (a.x != 0)
				newDir = 2 + Math.Sign(a.x);

			if (a.y != 0)
				newDir = 3 + Math.Sign(a.y);

			return newDir;
		}

		//public void CheckTile(Vector2 pos, int moveDist, int dirToIgnore)
		//{
		//	//Skips if out of bounds
		//	if (pos.x < 0 && pos.y < 0 && pos.y >= mainMap.Height && pos.x >= mainMap.Width)
		//		return;

			//	//Get cost 
			//	int moveCost = mainMap.GetSurface(pos).GetMoveCost(locomotion);
			//	int newMoveDist = moveDist - moveCost;
			//	if (moveCost > -1 && newMoveDist > 0)
			//	{
			//		PossibleMoves.Add(pos);
			//		if (newMoveDist > 0)
			//		{
			//			for (int i = 0; i < 4; ++i)
			//			{
			//				if (i == dirToIgnore)
			//					continue;

			//				Vector2 newTile = new Vector2();
			//				

			//				CheckTile(pos + newTile, newMoveDist, (i + 2) % 3);
			//			}
			//		}
			//		return;
			//	}
			//	return;
			//}

		public struct Move
		{
			bool spawned;
			int[] previousMoves;
			int moveRemaining;

			public Move(int moveRemaining, int[] previousMoves)
			{
				this.previousMoves = previousMoves;
				this.moveRemaining = moveRemaining;
				spawned = false;
			}
		}
	}
}
