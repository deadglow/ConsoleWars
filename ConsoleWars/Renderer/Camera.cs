using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRender
{
	public class Camera
	{
		public static Camera main;
		public Vector2 Position { get; set; }
		public Vector2 Size { get; set; }
		Vector2 limits;

		public Camera(Vector2 position, Vector2 size, Vector2 canvasSize)
		{
			Position = position;
			Size = size;
			limits = canvasSize - size - Vector2.One;
		}

		public void MoveTo(Vector2 newPos)
		{
			Position = new Vector2(Mathf.Clamp(newPos.x, 0, limits.x), Mathf.Clamp(newPos.y, 0, limits.y));
		}

		public void Move(Vector2 deltaPos)
		{
			MoveTo(Position + deltaPos);
		}

	}
}
