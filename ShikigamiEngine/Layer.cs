using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ShikigamiEngine
{
	public class Layer
	{
		public static List<Layer> Layers = new List<Layer>();

		public static Layer TaskObjects = new Layer();
		public static Layer FrameObjects = new Layer();
		public static Layer Misc = new Layer();
		public static Layer Enemies = new Layer();
		public static Layer Player = new Layer();
		public static Layer PlayerBullets = new Layer();
		public static Layer Bullets = new Layer();
		public static Layer Info = new Layer();

		public static void UpdateAll()
		{
			foreach (var layer in Layers)
			{
				layer.Update();
			}
		}

		public static void DrawAll()
		{
			Frame.Untarget();
			Layer.FrameObjects.DrawPrimitive();

			//draw enemy markers
			if (Enemy.BossMarker != null)
			{
				Layer.Info.DrawSpriteOnBegin(BlendState.AlphaBlend);
				foreach (var enm in Enemy.BossObjects)
				{
					Layer.Info.DrawSpriteOn(Enemy.BossMarker, Frame.Position + new Vector2((float)Math.Round(Frame.Center.X + enm.Position.X), (float)Math.Round(Frame.Size.Y + Enemy.BossMarker.Size.Y / 2)), 0f, Vector2.One, Color.White, (byte)255, (byte)0, Vector2.Zero);
				}
				Layer.Info.DrawSpriteOnEnd();
			}

			Frame.Target();

			Layer.Misc.DrawPrimitive();
			Layer.Enemies.DrawSprite(Frame.Center);
			Layer.Player.DrawSprite(Frame.Center);
			Layer.PlayerBullets.DrawSprite(Frame.Center);
			Layer.Bullets.DrawSprite(Frame.Center);
			Layer.Info.DrawSprite(Frame.Center);

			if (Enemy.HealthBarBack != null && Enemy.HealthBarFront != null)
			{
				foreach (var enm in Enemy.BossObjects)
				{
					Layer.Info.DrawUserPrimitiveOn(
						UserPrimitive.CircularBarBack,
						Enemy.HealthBarBack,
						enm.Position,
						0f,
						Vector2.One,
						Color.White,
						(byte)255
					);
					Layer.Info.DrawUserPrimitiveOn(
						UserPrimitive.CircularBarFront,
						Enemy.HealthBarFront,
						enm.Position,
						0f,
						Vector2.One,
						Color.White,
						(byte)255
					);
				}
			}

			Frame.Untarget();
		}



		public List<Entity> Entities = new List<Entity>();

		public Layer()
		{
			Layers.Add(this);
		}

		public void Add(Entity ent)
		{
			if (ent.Layer == null)
				ent.Layer = this;
			ent.Layer = this;
			Entities.Add(ent);
		}

		public void Remove(Entity ent)
		{
			if (ent.Layer == this)
				ent.Layer = null;
			Entities.Remove(ent);
		}

		public void Update()
		{
			for (var i = 0; i < Entities.Count; i++)
			{
				var ent = Entities[i];
				ent.Update();
				if (ent.Deleted)
				{
					Entities.RemoveAt(i);
					ent.Layer = this;
					i--;
				}
			}
		}

		public void DrawPrimitive()
		{
			for (var i = 0; i < Entities.Count; i++)
			{
				var ent = Entities[i];
				ent.DrawPrimitive();
			}
		}

		public void DrawSprite(Vector2 center)
		{
			var blendState = BlendState.AlphaBlend;
			Graphics.SpriteBatch.Begin(
				SpriteSortMode.Deferred,
				blendState,
				Graphics.SamplerStateSprite,
				Graphics.DepthStencilStateSprite,
				Graphics.RasterizerStateSprite
			);
			for (var i = 0; i < Entities.Count; i++)
			{
				var ent = Entities[i];
				/*if (ent.BlendState != blendState)
				{
					Graphics.SpriteBatch.End();
					blendState = ent.BlendState;
					Graphics.SpriteBatch.Begin(
						SpriteSortMode.Deferred,
						blendState,
						Graphics.SamplerStateSprite,
						Graphics.DepthStencilStateSprite,
						Graphics.RasterizerStateSprite
					);
				}*/
				ent.DrawSprite(center);
			}
			Graphics.SpriteBatch.End();
		}

		BlendState CurrentBlend;
		public void DrawSpriteOnBegin(BlendState blendState)
		{
			CurrentBlend = blendState;
			Graphics.SpriteBatch.Begin(
				SpriteSortMode.Deferred,
				blendState,
				Graphics.SamplerStateSprite,
				Graphics.DepthStencilStateSprite,
				Graphics.RasterizerStateSprite
			);
		}

		public void DrawSpriteOn(Animation anim, Vector2 pos, float renderAngle, Vector2 scale, Color color, byte alpha, byte glow, Vector2 center)
		{
			anim.DrawSprite(pos, renderAngle, scale, color, alpha, glow, Engine.Time * anim.Speed, center, CurrentBlend);
		}

		public void DrawSpriteOnEnd()
		{
			Graphics.SpriteBatch.End();
		}

		public void DrawUserPrimitiveOn(UserPrimitive uPrim, Animation anim, Vector2 pos, float renderAngle, Vector2 scale, Color color, byte alpha)
		{
			if (uPrim == null || anim == null)
				return;
			uPrim.Draw(anim, Engine.Time * anim.Speed, Matrix.CreateRotationZ(renderAngle) * Matrix.CreateScale(new Vector3(scale, 1f)) * Matrix.CreateTranslation(new Vector3(pos, 0f)), alpha);
		}
	}
}

