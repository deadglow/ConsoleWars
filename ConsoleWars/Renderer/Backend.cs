﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRender
{
	public struct Vector2
	{
		public static Vector2 Zero
		{
			get
			{
				return new Vector2(0, 0);
			}
		}
		public static Vector2 One
		{
			get
			{
				return new Vector2(1, 1);
			}
		}

		public float x;
		public float y;

		public Vector2(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//		Operators
		//____________________________________________________

		public static Vector2 operator +(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x + b.x, a.y + b.y);
		}
		public static Vector2 operator -(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x - b.x, a.y - b.y);
		}
		public static Vector2 operator *(Vector2 a, float scalar)
		{
			return new Vector2(a.x * scalar, a.y * scalar);
		}
		public static Vector2 operator *(Vector2 a, double scalar)
		{
			return new Vector2(a.x * (float)scalar, a.y * (float)scalar);
		}
		public static Vector2 operator /(Vector2 a, float scalar)
		{
			if (scalar == 0)
				return Zero;

			return new Vector2(a.x / scalar, a.y / scalar);
		}
		public static Vector2 operator %(Vector2 a, float scalar)
		{
			return new Vector2(a.x % scalar, a.y % scalar);
		}
		public static Vector2 operator -(Vector2 a)
		{
			return new Vector2(-a.x, -a.y);
		}

		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//			Methods
		//______________________________________________________

		public static Vector2 Lerp(Vector2 origin, Vector2 destination, float t)
		{
			return origin + (destination - origin) * t;
		}
	}

	public partial class Mathf
	{
		public static float Clamp(float value, float min, float max)
		{
			return Math.Max(min, Math.Min(value, max));
		}
	}

}
