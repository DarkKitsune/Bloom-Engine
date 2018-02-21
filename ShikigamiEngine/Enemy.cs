using System;
using System.Collections.Generic;

using DrakeScript;

using Microsoft.Xna.Framework;

namespace ShikigamiEngine
{
	public class Enemy : LeftRightAnimatedEntity
	{
		public static List<Enemy> BossObjects = new List<Enemy>();
		static double _BossLife;
		public static double BossLife
		{
			set
			{
				_BossLife = value;
				UserPrimitive.UpdateCircularBar(true);
			}
			get
			{
				return _BossLife;
			}
		}
		static double _MaxBossLife;
		public static double MaxBossLife
		{
			set
			{
				_MaxBossLife = value;
				UserPrimitive.UpdateCircularBar(true);
			}
			get
			{
				return _MaxBossLife;
			}
		}
		public static bool BossExists
		{
			get
			{
				return BossObjects.Count > 0;
			}
		}
		public static Animation BossMarker = null;
		public static Animation HealthBarBack = null;
		public static Animation HealthBarFront = null;
		static float _BossCircularHealthBarRadius = 48;
		public static float BossCircularHealthBarRadius
		{
			set
			{
				_BossCircularHealthBarRadius = value;
				UserPrimitive.UpdateCircularBar(false);
			}
			get
			{
				return _BossCircularHealthBarRadius;
			}
		}

		static float _BossCircularHealthBarThickness = 16;
		public static float BossCircularHealthBarThickness
		{
			set
			{
				_BossCircularHealthBarThickness = value;
				UserPrimitive.UpdateCircularBar(false);
			}
			get
			{
				return _BossCircularHealthBarThickness;
			}
		}

		double _MaxLife = float.MaxValue;
		public double MaxLife
		{
			set
			{
				if (IsBoss)
					MaxBossLife = Math.Max(value, 0);
				else
					_MaxLife = Math.Max(value, 0);
			}
			get
			{
				if (IsBoss)
				{
					return MaxBossLife;
				}
				return _MaxLife;
			}
		}
		double _Life = float.MaxValue;
		public double Life
		{
			set
			{
				if (IsBoss)
					BossLife = Math.Max(value, 0);
				else
					_Life = Math.Max(value, 0);
			}
			get
			{
				if (IsBoss)
				{
					return BossLife;
				}
				return _Life;
			}
		}
		bool _IsBoss;
		public bool IsBoss
		{
			set
			{
				_IsBoss = value;
				if (value)
				{
					if (BossObjects.Count == 0)
					{
						BossLife = _Life;
						MaxBossLife = _MaxLife;
					}
					if (!BossObjects.Contains(this))
						BossObjects.Add(this);
				}
				else
				{
					BossObjects.Remove(this);
				}
			}
			get
			{
				return _IsBoss;
			}
		}

		public Enemy(Function func, Vector2 pos) : base(func, Layer.Enemies, pos)
		{
			Precise = true;
			HitboxRadius = 16;
		}

		public override void Delete()
		{
			base.Delete();

			BossObjects.Remove(this);
		}

		public override void Update()
		{
			base.Update();

			foreach (var ent in Layer.PlayerBullets.Entities)
			{
				if (ent.Destroyed)
					continue;
				var diffX = ent.Position.X - Position.X;
				var diffY = ent.Position.Y - Position.Y;
				var hitboxCombined = ent.HitboxRadius + HitboxRadius;

				if (diffX * diffX + diffY * diffY < hitboxCombined * hitboxCombined)
				{
					Life -= (float)((Bullet)ent).Damage;
					ent.Destroy();
					if (IsBoss)
					{
						if (BossLife <= 0.0)
						{
							Script.InvokeEntityEventTask(this, "onBossDie");
						}
					}
					else
					{
						if (Life <= 0.0)
						{
							Destroy();
						}
					}
				}
			}
		}
	}
}

