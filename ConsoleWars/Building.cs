using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleWars
{
	class Building : Surface
	{
		Team team;
		int capRequirement = 10;
		int currentCap = 0;
		Team cappingTeam;
		int resourcePerTurn;

		public Building(string name, AnimatedSprite animSprite, Map parentMap) : base(name, animSprite, parentMap)
		{
		}

	}
}
