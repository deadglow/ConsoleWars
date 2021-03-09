using System;
using ConsoleRender;

namespace ConsoleWars
{
	class Building : Surface
	{
		Team team;
		int capRequirement = 20;
		int currentCap = 0;
		Team cappingTeam;
		int resourcePerTurn;

		public Building(string name, Vector2 position, AnimatedSprite animSprite, Map parentMap) : base(name, position, animSprite, parentMap)
		{
		}

	}
}
