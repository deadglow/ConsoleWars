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
		string name;
		int[] resistanceBonus = {1, 1, 1, 1, 1 };
		int[] damageBonus = { 1, 1, 1, 1, 1 };
		bool canHide;
		bool blockVision;
		Unit.Locomotion moveTypes = Unit.Locomotion.All;
		int[] moveCost = { 1, 1, 1, 1, 1, 1};
		Unit currentUnit = null;

		public int GetDamageBonus(Unit.DamageType damageType)
		{
			return damageBonus[(int)damageType];
		}
		public int GetResistanceBonus(Unit.DamageType damageType)
		{
			return resistanceBonus[(int)damageType];
		}

		public object Clone()
		{
			Surface newSurface = (Surface)MemberwiseClone();
			newSurface.currentUnit = null;

			return newSurface;
		}
	}
}
