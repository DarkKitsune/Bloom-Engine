using System;

using DrakeScript;

using Microsoft.Xna.Framework;

namespace ShikigamiEngine
{
	public class LeftRightAnimatedEntity : Entity
	{
		public Animation AnimationIdle;
		public Animation AnimationTurnLeft;
		public Animation AnimationTurnRight;
		public Animation AnimationLeft;
		public Animation AnimationRight;
		public double TurnAmount;
		public double TurnTime = 8.0;

		public LeftRightAnimatedEntity(Function func, Layer layer, Vector2 pos) : base(func, layer, pos)
		{
			
		}

		public override void Update()
		{
			base.Update();

			if (TurnAmount > 0.0)
			{
				if (TurnAmount < 1.0 && AnimationTurnRight != null)
				{
					Animation = AnimationTurnRight;
					AnimationFrame = TurnAmount * (double)Animation.Length;
				}
				else
				{
					Animation = AnimationRight;
				}
			}
			else if (TurnAmount < 0.0)
			{
				if (TurnAmount > -1.0 && AnimationTurnLeft != null)
				{
					Animation = AnimationTurnLeft;
					AnimationFrame = -TurnAmount * (double)Animation.Length;
				}
				else
				{
					Animation = AnimationLeft;
				}
			}
			else
			{
				Animation = AnimationIdle;
			}
		}
	}
}

