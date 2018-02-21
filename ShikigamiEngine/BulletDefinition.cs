using System;

using Microsoft.Xna.Framework.Graphics;

namespace ShikigamiEngine
{
	public class BulletDefinition
	{
		public Animation Animation;
		public float HitboxRadius;
		public float Damage;
		public string Type;
		public BlendState Blend;
		public bool Fixed;

		public BulletDefinition(Animation animation, float hitboxRadius, BlendState blend, string type, float damage = 0f, bool fixedAngle = false)
		{
			HitboxRadius = hitboxRadius;
			Animation = animation;
			Blend = blend;
			Type = type;
			Damage = damage;
			Fixed = fixedAngle;
		}
	}
}

