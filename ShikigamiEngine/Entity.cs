using System;

using DrakeScript;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShikigamiEngine
{
	public class Entity
	{
		float _Angle;
		float _Speed;
		float _RenderAngle;
		Vector2 _Scale = Vector2.One;
		public byte Alpha = 255;
		public byte Glow = 0;
		public float HitboxRadius = 0f;
		public bool RenderAngleRelative;
		public bool Deleted;
		public bool Destroyed;
		public bool CanDelete
		{
			get
			{
				return !Deleted;
			}
		}
		public bool CanDestroy
		{
			get
			{
				return !(Deleted | Destroyed);
			}
		}

		void UpdateScaleRotation()
		{
			ScaleRotation = Matrix.CreateScale(Scale.X, Scale.Y, 1f);

			if (RenderAngleOffsetEnabled)
			{
				ScaleRotation *= Matrix.CreateRotationZ(RenderAngleOffset);
			}

			ScaleRotation *= Matrix.CreateRotationZ(MathHelper.ToRadians((RenderAngleRelative ? Angle + RenderAngle : RenderAngle)));
		}

		public float Angle
		{
			set
			{
				_Angle = Mathx.AngleFix(value);
				if (RenderAngleRelative)
				{
					UpdateScaleRotation();
				}
			}
			get
			{
				return _Angle;
			}
		}

		public float Speed
		{
			set
			{
				_Speed = value;
			}
			get
			{
				return _Speed;
			}
		}

		public float RenderAngle
		{
			set
			{
				_RenderAngle = Mathx.AngleFix(value);
				UpdateScaleRotation();
			}
			get
			{
				return _RenderAngle;
			}
		}

		float _RenderAngleOffset;
		bool RenderAngleOffsetEnabled;
		public float RenderAngleOffset
		{
			set
			{
				_RenderAngleOffset = Mathx.AngleFix(value);
				RenderAngleOffsetEnabled = true;
				UpdateScaleRotation();
			}
			get
			{
				return _RenderAngleOffset;
			}
		}

		public float RenderAngleFinal
		{
			get
			{
				if (RenderAngleRelative)
					return Angle + RenderAngle;
				return RenderAngle;
			}
		}

		public Vector2 Scale
		{
			set
			{
				_Scale = value;
				UpdateScaleRotation();
			}
			get
			{
				return _Scale;
			}
		}

		public float SpeedX
		{
			set
			{
				var speedY = SpeedY;
				Angle = Mathx.DAtan2(speedY, value);
				Speed = (float)Math.Sqrt(value * value + speedY * speedY);
			}
			get
			{
				if (Speed == 0f)
					return 0f;
				return Mathx.DCos(Angle) * Speed;
			}
		}
		public float SpeedY
		{
			set
			{
				var speedX = SpeedX;
				Angle = Mathx.DAtan2(value, speedX);
				Speed = (float)Math.Sqrt(value * value + speedX * speedX);
			}
			get
			{

				if (Speed == 0f)
					return 0f;
				return Mathx.DSin(Angle) * Speed;
			}
		}

		public bool Precise;

		public Vector2 Position;
		public double AnimationFrame = 0.0;
		Animation _Animation;
		Animation LastAnimation;
		public Animation Animation
		{
			set
			{
				if (_Animation != value)
				{
					LastAnimation = _Animation;
					AnimationFrame = 0.0;
					_Animation = value;
				}
			}
			get
			{
				return _Animation;
			}
		}
		public UserPrimitive UserPrimitive = null;
		public Matrix ScaleRotation = Matrix.CreateScale(1f, 1f, 1f);

		public Layer Layer;
		public Task Task;
		public BlendState BlendState = BlendState.AlphaBlend;

		bool TweenTrans;
		TweenTrans2 TweenPosition;
		TweenTrans1 TweenSpeed;
		TweenTrans1 TweenAngle;
		TweenTrans2 TweenScale;
		TweenTrans1 TweenRenderAngle;
		TweenTrans1 TweenAlpha;
		TweenTrans1 TweenGlow;


		public Entity(Function func, Layer layer, Vector2 position)
		{
			Deleted = false;
			Layer = layer;
			layer.Add(this);
			Position = position;
			if (func != null)
			{
				Script.CreateTask(this, func);
			}
		}
			
		public virtual void Update()
		{
			if (Deleted)
				return;
			if (Animation != null)
				AnimationFrame += Animation.Speed;

			bool changedPosition = false;
			if (TweenTrans)
			{
				if (!TweenPosition.Ended)
				{
					changedPosition = true;

					var oldPos = Position;
					Position = TweenPosition.GetValueVector();
					SpeedX = Position.X - oldPos.X;
					SpeedY = Position.Y - oldPos.Y;
				}
				else
				{
					if (!TweenSpeed.Ended)
						Speed = TweenSpeed.GetValue();
					if (!TweenAngle.Ended)
						Angle = TweenAngle.GetValueAngular();
				}
				if (!TweenRenderAngle.Ended)
					RenderAngle = TweenRenderAngle.GetValueAngular();
				if (!TweenScale.Ended)
					Scale = TweenScale.GetValueVector();
				if (!TweenAlpha.Ended)
					Alpha = (byte)TweenAlpha.GetValue();
				if (!TweenGlow.Ended)
					Glow = (byte)TweenGlow.GetValue();
			}

			if (!changedPosition)
			{
				Position += new Vector2(SpeedX, SpeedY);
			}
		}

		public virtual void DrawPrimitive()
		{
			if (Deleted)
				return;
			Graphics.GraphicsDevice.BlendState = BlendState;
			if (UserPrimitive == null)
			{
				if (!Precise)
					Animation?.DrawPrimitive(ScaleRotation * Matrix.CreateTranslation(Position.X, Position.Y, 0f), Alpha, AnimationFrame);
				else
					Animation?.DrawPrimitive(ScaleRotation * Matrix.CreateTranslation((float)Math.Round(Position.X), (float)Math.Round(Position.Y), 0f), Alpha, AnimationFrame);
			}
			else
			{
				UserPrimitive.Draw(Animation, AnimationFrame, ScaleRotation * Matrix.CreateTranslation(Position.X, Position.Y, 0f), Alpha);
			}
		}

		public virtual void DrawSprite(Vector2 center)
		{
			if (Deleted)
				return;
			if (!Precise)
				Animation?.DrawSprite(
					Position,
					MathHelper.ToRadians(RenderAngleFinal),
					Scale,
					Color.White,
					Alpha,
					Glow,
					AnimationFrame,
					center,
					BlendState
				);
			else
				Animation?.DrawSprite(
					new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y)),
					MathHelper.ToRadians(RenderAngleFinal),
					Scale,
					Color.White,
					Alpha,
					Glow,
					AnimationFrame,
					center,
					BlendState
				);
		}

		void _Delete()
		{
			Deleted = true;
			Task?.Stop();
		}

		public virtual void Delete()
		{
			if (!CanDelete)
				return;
			_Delete();
		}

		public virtual void Destroy()
		{
			if (!CanDestroy)
				return;
			Destroyed = true;
			_Delete();
		}

		public void ResetAnimation()
		{
			Animation = LastAnimation;
		}


		public void Move(Vector2 newV, TweenType tween, double time)
		{
			TweenTrans = true;
			TweenPosition = new TweenTrans2(Position.X, newV.X, Position.Y, newV.Y, tween, Engine.Time + time);
		}

		public void Accelerate(float newV, TweenType tween, double time)
		{
			TweenTrans = true;
			TweenSpeed = new TweenTrans1(Speed, newV,  tween, Engine.Time + time);
		}

		public void Turn(float newV, TweenType tween, double time)
		{
			TweenTrans = true;
			TweenAngle = new TweenTrans1(Angle, newV,  tween, Engine.Time + time);
		}

		public void Rotate(float newV, TweenType tween, double time)
		{
			TweenTrans = true;
			TweenRenderAngle = new TweenTrans1(RenderAngle, newV,  tween, Engine.Time + time);
		}

		public void ChangeScale(Vector2 newV, TweenType tween, double time)
		{
			TweenTrans = true;
			TweenScale = new TweenTrans2(Scale.X, newV.X, Scale.Y, newV.Y,  tween, Engine.Time + time);
		}

		public void ChangeAlpha(byte newV, TweenType tween, double time)
		{
			TweenTrans = true;
			TweenAlpha = new TweenTrans1(Alpha, (float)newV,  tween, Engine.Time + time);
		}

		public void ChangeGlow(byte newV, TweenType tween, double time)
		{
			TweenTrans = true;
			TweenGlow = new TweenTrans1(Glow, (float)newV,  tween, Engine.Time + time);
		}
	}
}

