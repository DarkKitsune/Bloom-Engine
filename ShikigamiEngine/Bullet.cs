using System;

using Microsoft.Xna.Framework;
using DrakeScript;

namespace ShikigamiEngine
{
	public class Bullet : Entity
	{
		BulletDefinition _Definition;
		public BulletDefinition Definition
		{
			set
			{
				_Definition = value;
				Animation = _Definition.Animation;
				Damage = _Definition.Damage;
				BlendState = _Definition.Blend;
				HitboxRadius = value.HitboxRadius;
				if (!value.Fixed)
				{
					RenderAngle = 90f;
					RenderAngleRelative = true;
				}
			}
			get
			{
				return _Definition;
			}
		}
		public float Damage = 0f;
		public bool PlayerTeam;

		public static Bullet PlayerBullet(Vector2 position, float speed, float angle, BulletDefinition definition, Function function)
		{
			return new Bullet(position, speed, angle, definition, function, 0);
		}

		//enemy bullet constructor
		public Bullet(Vector2 position, float speed, float angle, BulletDefinition definition, Function function) : base(null, Layer.Bullets, position)
		{
			Speed = speed;
			Angle = angle;
			Definition = definition;
			Script.InvokeEntityEventTask(this, "onBulletCreate");
			if (function != null)
				Script.CreateTask(this, function);
		}

		//player bullet constructor
		Bullet(Vector2 position, float speed, float angle, BulletDefinition definition, Function function, int dummy) : base(function, Layer.PlayerBullets, position)
		{
			PlayerTeam = true;
			Speed = speed;
			Angle = angle;
			Definition = definition;
		}

		public override void Update()
		{
			base.Update();

			if (
				Position.X < -Frame.Size.X / 2f - Frame.DeleteMargin ||
				Position.Y < -Frame.DeleteMargin ||
				Position.X > Frame.Size.X / 2f + Frame.DeleteMargin ||
				Position.Y > Frame.Size.Y + Frame.DeleteMargin
			)
			{
				Delete();
			}
		}

		public override void Destroy()
		{
			if (!CanDestroy)
				return;
			
			if (PlayerTeam)
			{
				if (Script.InvokeEntityEventTask(this, "onPlayerBulletDestroy") == null)
					base.Destroy();
			}
			else
			{
				if (Script.InvokeEntityEventTask(this, "onBulletDestroy") == null)
					base.Destroy();
			}
		}
	}
}

