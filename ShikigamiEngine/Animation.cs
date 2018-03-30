using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShikigamiEngine
{
	public class Animation
	{
		static Dictionary<string, Texture2D> TextureCache = new Dictionary<string, Texture2D>();
		public static Texture2D LoadTexture(string path)
		{
			var actualPath = Path.GetFullPath(Paths.Textures + path);
			Texture2D ret;
			if (!TextureCache.TryGetValue(actualPath, out ret))
			{
				using (var stream = File.OpenRead(actualPath))
				{
					ret = Texture2DPremultipliedFromStream(Graphics.GraphicsDevice, stream);
					TextureCache.Add(actualPath, ret);
				}
			}
			return ret;
		}

        public static Texture2D Texture2DPremultipliedFromStream(GraphicsDevice graphics, Stream stream)
        {
            var tex = Texture2D.FromStream(graphics, stream);
            var data = new Color[tex.Width * tex.Height];
            tex.GetData(data);
            for (var i = 0; i < data.Length; i++)
                data[i] = Color.FromNonPremultiplied(data[i].R, data[i].G, data[i].B, data[i].A);
            tex.SetData(data);
            return tex;
        }


        public Texture2D Texture {get; private set;}
		public double Speed {get; private set;}
		public Rectangle[] Frames;
		Primitive[] FramePrimitives;
		public Vector2 Size {get; private set;}
		public bool Mirrored {get; private set;}

		public int Length
		{
			get
			{
				return Frames.Length;
			}
		}

		public Animation(string texturePath, double speed, Rectangle[] frames)
		{
			Size = frames[0].Size.ToVector2();
			Texture = LoadTexture(texturePath);
			Speed = speed;
			SetFrames(frames);
		}

		public Animation(string texturePath, double speed, int x, int y, int width, int height, int number, bool mirrored = false)
		{
			Size = new Vector2(width, height);
			Texture = LoadTexture(texturePath);
			Speed = speed;
			Mirrored = mirrored;

			var frames = new Rectangle[number];
			if (mirrored)
			{
				for (var i = 0; i < number; i++)
				{
					frames[i] = new Rectangle(x + width * i, y, -width, height);
				}
			}
			else
			{
				for (var i = 0; i < number; i++)
				{
					frames[i] = new Rectangle(x + width * i, y, width, height);
				}
			}

			SetFrames(frames);
		}

		void SetFrames(Rectangle[] frames)
		{
			FramePrimitives = new Primitive[frames.Length];
			Frames = frames;
			var tw = (float)Texture.Width;
			var th = (float)Texture.Height;
			var n = 0;
			foreach (var frame in frames)
			{
				var prim = new Primitive();
				var x = (float)frame.X;
				var y = (float)frame.Y;
				var w = (float)frame.Width;
				var absW = (float)Math.Abs(frame.Width);
				var h = (float)frame.Height;
				var x1 = (float)Math.Floor(-w / 2f);
				var x2 = (float)Math.Floor(w / 2f);
				var y1 = (float)Math.Floor(-h / 2f);
				var y2 = (float)Math.Floor(h / 2f);
				prim.Begin();
				prim.AddVertex(
					new Vector3(x1, y1, 0f), Color.White, new Vector2(x / tw, y / th)
				);
				prim.AddVertex(
					new Vector3(x2, y1, 0f), Color.White, new Vector2((x + absW) / tw, y / th)
				);
				prim.AddVertex(
					new Vector3(x2, y2, 0f), Color.White, new Vector2((x + absW) / tw, (y + h) / th)
				);
				prim.AddVertex(
					new Vector3(x2, y2, 0f), Color.White, new Vector2((x + absW) / tw, (y + h) / th)
				);
				prim.AddVertex(
					new Vector3(x1, y2, 0f), Color.White, new Vector2(x / tw, (y + h) / th)
				);
				prim.AddVertex(
					new Vector3(x1, y1, 0f), Color.White, new Vector2(x / tw, y / th)
				);
				prim.End();
				FramePrimitives[n++] = prim;
			}
		}

		public void DrawPrimitive(Matrix matrix, byte alpha, double frameNumber)
		{
			int frame = (int)Math.Floor(frameNumber) % Frames.Length;
			if (frame < 0)
				frame += Frames.Length;

			FramePrimitives[frame].Draw(
				matrix,
				alpha,
				Texture
			);
		}

		static Vector2 CenterOrigin = new Vector2(0.5f, 0.5f);
		static Vector2 MirrorVec = new Vector2(-1f, 1f);
		public void DrawSprite(Vector2 pos, float renderAngle, Vector2 scale, Color color, byte alpha, byte glow, double frameNumber, Vector2 center, BlendState blendUsed)
		{
			int frame = (int)Math.Floor(frameNumber) % Frames.Length;
			if (frame < 0)
				frame += Frames.Length;

			var floatAlpha = (float)alpha / 255f;
			Color col = new Color((int)(color.R * floatAlpha), (int)(color.G * floatAlpha), (int)(color.B * floatAlpha), (int)alpha);
			if (blendUsed == BlendState.Additive)
			{
				col = new Color((int)(color.R * floatAlpha), (int)(color.G * floatAlpha), (int)(color.B * floatAlpha), 0);
			}
			else
			{
				var floatGlowMult = 1f - ((float)glow / 255f);
				col = new Color((int)(color.R * floatAlpha), (int)(color.G * floatAlpha), (int)(color.B * floatAlpha), (int)((float)alpha * floatGlowMult));
			}
			Graphics.SpriteBatch.Draw(Texture, center + pos, Frames[frame], col, renderAngle, CenterOrigin * Frames[frame].Size.ToVector2(), (Mirrored ? scale * MirrorVec : scale), SpriteEffects.None, 0);
			/*
			FramePrimitives[frame].Draw(
				matrix,
				alpha,
				Texture
			);*/
		}
	}
}

