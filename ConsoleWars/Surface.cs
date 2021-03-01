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
		protected bool canHide;
		protected bool blockVision;
		protected Unit.Locomotion moveTypes = Unit.Locomotion.All;
		protected int[] moveCost = { 1, 1, 1, 1, 1, 1};

		public AnimatedSprite AnimSprite { get; }
		public Map ParentMap { get; }
		public int DamageBonus = 0;
		public int Defence { get; }
		public Unit CurrentUnit { get; set; } = null;

		public Surface(string name, AnimatedSprite animSprite, Map parentMap)
		{
			Name = name;
			AnimSprite = animSprite;
			ParentMap = parentMap;
		}


		public object Clone()
		{
			Surface newSurface = (Surface)MemberwiseClone();
			newSurface.CurrentUnit = null;

			return newSurface;
		}

		public void Draw(int x, int y)
		{
			AnimSprite.DrawAnimated(x, y, 1, 1);
			if (CurrentUnit != null)
				CurrentUnit.Draw(x, y);
		}

	}
}
