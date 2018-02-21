	using System;

using DrakeScript;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShikigamiEngine
{
	public class Player : LeftRightAnimatedEntity
	{
		public static Vector2 StartPosition = new Vector2(0f, 400f);
		public static Player Object {get; private set;}
		public static bool Exists
		{
			get
			{
				return Object != null && !Object.Deleted;
			}
		}

		public static float HitboxRadiusPlayer = 1.5f;
		public static float MoveSpeed = 4.5f;
		public static float MoveSpeedSlow = 3f;
		static double _TurnTimePlayer = 8.0;
		public static double TurnTimePlayer
		{
			set
			{
				if (Exists)
					Object.TurnTime = value;
				_TurnTimePlayer = value;
			}
			get
			{
				return _TurnTimePlayer;
			}
		}
		static Animation _AnimationIdlePlayer;
		static Animation _AnimationTurnLeftPlayer;
		static Animation _AnimationTurnRightPlayer;
		static Animation _AnimationLeftPlayer;
		static Animation _AnimationRightPlayer;
		public static Animation AnimationIdlePlayer
		{
			set
			{
				if (Exists)
					Object.AnimationIdle = value;
				_AnimationIdlePlayer = value;
			}
			get
			{
				return _AnimationIdlePlayer;
			}
		}
		public static Animation AnimationTurnLeftPlayer
		{
			set
			{
				if (Exists)
					Object.AnimationTurnLeft = value;
				_AnimationTurnLeftPlayer = value;
			}
			get
			{
				return _AnimationTurnLeftPlayer;
			}
		}
		public static Animation AnimationTurnRightPlayer
		{
			set
			{
				if (Exists)
					Object.AnimationTurnRight = value;
				_AnimationTurnRightPlayer = value;
			}
			get
			{
				return _AnimationTurnRightPlayer;
			}
		}
		public static Animation AnimationLeftPlayer
		{
			set
			{
				if (Exists)
					Object.AnimationLeft = value;
				_AnimationLeftPlayer = value;
			}
			get
			{
				return _AnimationLeftPlayer;
			}
		}
		public static Animation AnimationRightPlayer
		{
			set
			{
				if (Exists)
					Object.AnimationRight = value;
				_AnimationRightPlayer = value;
			}
			get
			{
				return _AnimationRightPlayer;
			}
		}
		public static double ReviveTime = 120.0;

	
		public bool Slow;
		public double LastDeathTime;
		public bool Alive;
		bool ShouldShoot;
		bool LastShouldShoot;
		Task ShootTask = null;

		public Player() : base(null, Layer.Player, StartPosition)
		{
			Object = this;
			Position = StartPosition;
			TurnTime = TurnTimePlayer;

			AnimationIdle = AnimationIdlePlayer;
			AnimationLeft = AnimationLeftPlayer;
			AnimationRight = AnimationRightPlayer;
			AnimationTurnLeft = AnimationTurnLeftPlayer;
			AnimationTurnRight = AnimationTurnRightPlayer;

			Precise = true;
			Alive = true;
		}

		public override void Update()
		{
			base.Update();

			if (Position.X < -Frame.Size.X / 2f + 2f)
				Position = new Vector2(-Frame.Size.X / 2f + 2f, Position.Y);
			if (Position.X > Frame.Size.X / 2f - 2f)
				Position = new Vector2(Frame.Size.X / 2f - 2f, Position.Y);
			if (Position.Y < 2f)
				Position = new Vector2(Position.X, 2f);
			if (Position.Y >= Frame.Size.Y - 2f)
				Position = new Vector2(Position.X, Frame.Size.Y - 2f);

			if (!Alive)
			{
				if (Engine.Time >= LastDeathTime + ReviveTime)
				{
					Alive = true;
					Script.InvokeEntityEventTask(Object, "onPlayerRevive");
				}
				ShouldShoot = false;
			}
			else
			{
				ShouldShoot = Input.GetKeyPressed(Keys.Z);
			}

			if (ShouldShoot)
			{
				if (!LastShouldShoot)
				{
					Console.WriteLine("start shooting");
					ShootTask = Script.InvokeEntityEventTask(this, "onPlayerShoot");
				}
			}
			else
			{
				if (LastShouldShoot)
				{
					Console.WriteLine("stop shooting ");
					if (ShootTask != null)
						ShootTask.Stop();
				}
			}


			Slow = !Input.GetKeyPressed(Keys.LeftShift);

			var moveX = (Input.GetKeyPressed(Keys.Right) ? 1f : 0f) - (Input.GetKeyPressed(Keys.Left) ? 1f : 0f);
			var moveY = (Input.GetKeyPressed(Keys.Down) ? 1f : 0f) - (Input.GetKeyPressed(Keys.Up) ? 1f : 0f);
			if (moveX != 0f || moveY != 0f)
			{
				Angle = Mathx.DAtan2(moveY, moveX);
				Speed = (Slow ? MoveSpeed : MoveSpeedSlow);
			}
			else
			{
				Speed = 0f;
			}

			if (moveX != 0f)
				TurnAmount = Mathx.Clamp(TurnAmount + (double)Math.Sign(moveX) / TurnTimePlayer, -1.0, 1.0);
			else
				TurnAmount = Math.Max(
					0.0,
					Math.Abs(TurnAmount) - 1.0 / TurnTimePlayer
				) * (double)Math.Sign(TurnAmount);
					
			if (Alive)
			{
				foreach (var ent in Layer.Bullets.Entities)
				{
					if (ent.Destroyed)
						continue;
					var diffX = ent.Position.X - Position.X;
					var diffY = ent.Position.Y - Position.Y;
					var hitboxCombined = ent.HitboxRadius + HitboxRadiusPlayer;

					if (diffX * diffX + diffY * diffY < hitboxCombined * hitboxCombined)
					{
						LastDeathTime = Engine.Time;
						Alive = false;

						Script.InvokeEntityEventTask(this, "onPlayerDie");
						break;
					}
				}
			}

			LastShouldShoot = ShouldShoot;
		}
	}
}

