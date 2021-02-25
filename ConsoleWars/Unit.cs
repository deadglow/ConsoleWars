using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleWars
{


	class Unit : ICloneable
	{
		Map parentMap;
		Vector2 position = Vector2.Zero;
		Team team = Team.White;
		float hp = 10;
		float maxHp = 10;
		DamageType damageType;
		//Not the same as locomotion, dictates basically where (height and surface wise) the character is moving
		MovementType movementType = MovementType.None;
		float baseDamage = 1;
		//Movement
		Locomotion locomotion = Locomotion.None;
		int moveDistance = 5;
		int vision = 2;
		int minAttackRange = 1;
		int maxAttackRange;
		bool useFuel = false;
		bool useAmmo = false;
		int fuel = 50;
		int ammo = 20;

		//Stat mods
		float moveMod;
		float damageMod;
		float fuelUsageMod;
		float visionMod;
		float maxAttackRangeMod;

		//=-------------------------------------|
		//			Enums
		//_____________________________________/
		public enum DamageType
		{
			None,
			Bullet,
			Rocket,
			AntiArmour,
			AntiInfantry
		}

		[Flags]
		public enum Locomotion
		{
			None = 0,
			Legs = 1,
			Tires = 2,
			Treads = 4,
			Naval = 8,
			Aerial = 16,
			Amphibious = Treads | Naval,
			All = None | Legs | Treads | Naval | Aerial
		}

		public enum MovementType
		{
			None,
			Land,
			Mountain,
			Water,
			Air
		}

		//=-------------------------------------|
		//			Methods
		//_____________________________________/

		public object Clone()
		{
			Unit newUnit = (Unit)MemberwiseClone();
			newUnit.parentMap = null;

			return newUnit;
		}
		
		public float GetDamageTaken(float damage, DamageType damageType)
		{
			return damage * (hp / maxHp) * parentMap.GetSurface(position).GetResistanceBonus(damageType);
		}

		public float GetDamageDealt(bool firstTurnBonus)
		{
			float dam = baseDamage + (firstTurnBonus ? 1 : 0);

			return dam * hp / maxHp * parentMap.GetSurface(position).GetDamageBonus(damageType);
		}
	}
}
