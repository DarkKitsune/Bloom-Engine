using System;

using DrakeScript;

using Microsoft.Xna.Framework;

namespace ShikigamiEngine
{
	public class Pattern
	{
		public Vector2 Position;
		public int Count1, Count2;
		public PatternType PatternType;
		public Entity Parent;
		public BulletDefinition BulletDefinition;
		public Function Function;
		public float Speed1, Speed2, Angle1, Angle2, Angle3, Radius;
		public bool PlayerTeam;

		public Pattern(Entity parent)
		{
			Parent = parent;
		}

		public void Fire()
		{
			if (Parent == null || Parent.Deleted || BulletDefinition == null || Count1 <= 0 || Count2 <= 0)
				return;

			switch (PatternType)
			{
				case (PatternType.Normal):
					var angTotal = (float)(Count1 - 1) * (Angle2 / (float)Count1);
					var angAdd = angTotal / (float)(Count1 - 1);
					var speedAdd = (Speed2 - Speed1) / (float)(Math.Max(1, Count2 - 1));
					var ang = Angle1 - angTotal / 2f;
					for (var column = 0; column < Count1; column++)
					{
						var speed = Speed1;
						var shootPos = Position + Parent.Position + new Vector2(
							Mathx.DCos(ang) * Radius,
							Mathx.DSin(ang) * Radius
						);

						for (var bullet = 0; bullet < Count2; bullet++)
						{
							if (PlayerTeam)
								Bullet.PlayerBullet(shootPos, speed, ang + Angle3, BulletDefinition, Function);
							else
								new Bullet(shootPos, speed, ang + Angle3, BulletDefinition, Function);
							speed += speedAdd;
						}
						ang += angAdd;
					}
					break;
			}
		}
	}
}

