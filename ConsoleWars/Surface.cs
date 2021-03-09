using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRender;

namespace ConsoleWars
{
	class Surface : ICloneable
	{
		public string Name { get; }
		public Vector2 position = new Vector2();
		protected bool canHide;
		protected bool blockVision;
		protected Unit.Locomotion allowedLocomotion = Unit.Locomotion.All;
		protected int[] moveCost = { 1, 1, 1, 1, 1, 1};

		public AnimatedSprite AnimSprite { get; }
		public Map ParentMap { get; }
		public int DamageBonus = 0;
		public int Defence { get; }
		public Unit CurrentUnit { get; set; } = null;
		public bool drawUnit = true;

		public Surface(string name, Vector2 position, AnimatedSprite animSprite, Map parentMap)
		{
			Name = name;
			this.position = position;
			AnimSprite = animSprite;
			ParentMap = parentMap;
		}


		public object Clone(Vector2 newPos)
		{
			Surface newSurface = (Surface)MemberwiseClone();
			newSurface.CurrentUnit = null;
			newSurface.position = newPos;

			return newSurface;
		}
		public object Clone()
		{
			return Clone(position);
		}

		public void Draw(int x, int y)
		{
			AnimSprite.DrawAnimated(x, y, 1, 1);
			if (drawUnit && CurrentUnit != null)
				CurrentUnit.Draw();
		}

		//Returns -1 if can't move on that square
		public int GetMoveCost(Unit.Locomotion moveType)
		{
			if ((moveType & allowedLocomotion) == 0)
				return -1;

			return moveCost[(int)moveType];
		}
	}
}
