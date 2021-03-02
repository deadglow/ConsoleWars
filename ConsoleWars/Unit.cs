using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRender;

namespace ConsoleWars
{


	class Unit : ICloneable
	{
		public Surface ParentSurface { get; private set; }

		public string Name { get; } = "Unit";
		Team team = Team.White;

		public AnimatedSprite sprite;
		public float SpriteSpeed { get; set; } = 1f;
		float hp = 10;
		float maxHp = 10;
		DamageType damageType;
		float[] resistances = { 1, 1, 1, 1, 1 };
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

		public int FlipX { get; set; } = 1;
		public int FlipY { get; set; } = 1;
		//Stat mods
		float moveMod;
		float damageMod;
		float fuelUsageMod;
		float visionMod;
		float maxAttackRangeMod;

		public Unit(string name, Surface parentSurface, AnimatedSprite sprite)
		{
			Name = name;
			ParentSurface = parentSurface;
			this.sprite = sprite;
		}


		public object Clone(Surface newParentSurface)
		{
			Unit newUnit = (Unit)MemberwiseClone();
			newUnit.SetParentSurface(newParentSurface);

			return newUnit;
		}
		public object Clone()
		{
			return Clone(null);
		}

		public void Destroy()
		{
			SetParentSurface(null);
		}


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

		public void SetParentSurface(Surface newSurface)
		{
			//Checks first to see if not null
			if (ParentSurface != null)
				ParentSurface.CurrentUnit = null;

			if (newSurface != null)
				newSurface.CurrentUnit = this;

			ParentSurface = newSurface;
		}

		public bool MoveUnit(int newX, int newY)
		{
			//Checks to see if new coords are within map boundaries
			if (newX >= 0 && newX < ParentSurface.ParentMap.Width && newY >= 0 && newY < ParentSurface.ParentMap.Height)
			{
				Unit foundUnit = ParentSurface.ParentMap.GetSurface(newX, newY).CurrentUnit;
				//Prevents moving unit to an occupied surface
				if (foundUnit == null)
				{
					//Assign parent surface to surface at newX, newY
					SetParentSurface(ParentSurface.ParentMap.GetSurface(newX, newY));
					return true;
				}
				else if (foundUnit == this)
					return true;
			}

			return false;
		}

		public float GetDamageTaken(float damage, DamageType damageType)
		{
			return damage * resistances[(int)damageType] * (1 - ParentSurface.Defence * 0.05f);
		}

		public float GetDamageDealt(bool counterAttack)
		{
			float dam = baseDamage * (1 - ParentSurface.DamageBonus * 0.05f) * (counterAttack ? 0.7f : 1);

			return dam * (0.5f + hp / maxHp * 0.5f);
		}

		public void Draw(int x, int y)
		{
			sprite.DrawAnimated(x, y, FlipX, FlipY, SpriteSpeed);
		}

	}
}
