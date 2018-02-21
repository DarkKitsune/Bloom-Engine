using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShikigamiEngine
{
	public class UserPrimitive
	{
		public static UserPrimitive CircularBarBack {get; private set;} = new UserPrimitive();
		public static UserPrimitive CircularBarFront {get; private set;} = new UserPrimitive();

		public void GenerateRing(float radius, float thickness, float startAngle, float endAngle, int steps)
		{
			Begin();
			var angAdd = (endAngle - startAngle) / (float)steps;
			var cos = Mathx.DCos(startAngle);
			var sin = Mathx.DSin(startAngle);
			var lastPosOuter = new Vector3(
				cos * (radius + thickness / 2f),
				sin * (radius + thickness / 2f),
				0f
			);
			var lastPosInner = new Vector3(
				cos * (radius - thickness / 2f),
				sin * (radius - thickness / 2f),
				0f
			);
			var lastU = 0f;
			float u, angle;
			Vector3 posOuter, posInner;
			for (angle = startAngle + angAdd; angle < endAngle; angle += angAdd)
			{
				cos = Mathx.DCos(angle);
				sin = Mathx.DSin(angle);
				u = (angle - startAngle) / (endAngle - startAngle);
				posOuter = new Vector3(
					cos * (radius + thickness / 2f),
					sin * (radius + thickness / 2f),
					0f
				);
				posInner = new Vector3(
					cos * (radius - thickness / 2f),
					sin * (radius - thickness / 2f),
					0f
				);

				AddVertex(lastPosOuter, Color.White, new Vector2(lastU, 0f));
				AddVertex(posOuter, Color.White, new Vector2(u, 0f));
				AddVertex(lastPosInner, Color.White, new Vector2(lastU, 1f));

				AddVertex(lastPosInner, Color.White, new Vector2(lastU, 1f));
				AddVertex(posOuter, Color.White, new Vector2(u, 0f));
				AddVertex(posInner, Color.White, new Vector2(u, 1f));

				lastPosOuter = posOuter;
				lastPosInner = posInner;
				lastU = u;
			}
			angle = endAngle;
			cos = Mathx.DCos(angle);
			sin = Mathx.DSin(angle);
			u = 1f;
			posOuter = new Vector3(
				cos * (radius + thickness / 2f),
				sin * (radius + thickness / 2f),
				0f
			);
			posInner = new Vector3(
				cos * (radius - thickness / 2f),
				sin * (radius - thickness / 2f),
				0f
			);

			AddVertex(lastPosOuter, Color.White, new Vector2(lastU, 0f));
			AddVertex(posOuter, Color.White, new Vector2(u, 0f));
			AddVertex(lastPosInner, Color.White, new Vector2(lastU, 1f));

			AddVertex(lastPosInner, Color.White, new Vector2(lastU, 1f));
			AddVertex(posOuter, Color.White, new Vector2(u, 0f));
			AddVertex(posInner, Color.White, new Vector2(u, 1f));
			End();
		}
		public static void UpdateCircularBar(bool justFront)
		{
			if (!justFront)
			{
				CircularBarBack.GenerateRing(Enemy.BossCircularHealthBarRadius, Enemy.BossCircularHealthBarThickness, 0f, 360f, 30);
			}
			CircularBarFront.GenerateRing(Enemy.BossCircularHealthBarRadius, Enemy.BossCircularHealthBarThickness, 270f - (float)(Enemy.BossLife / Enemy.MaxBossLife) * 360f, 270f, 45);
		}


		VertexPositionColorTexture[] VertexArray = new VertexPositionColorTexture[] {};
		VertexPositionColorTexture[] VertexArrayTemplate = new VertexPositionColorTexture[] {};
		List<VertexPositionColorTexture> TempVertexList = new List<VertexPositionColorTexture>();

		public UserPrimitive()
		{
		}

		public void Begin()
		{
			TempVertexList.Clear();
		}

		public void AddVertex(Vector3 position, Color color, Vector2 uv)
		{
			TempVertexList.Add(new VertexPositionColorTexture(position, color, uv));
		}

		static bool Changed;
		public void End()
		{
			VertexArray = TempVertexList.ToArray();
			VertexArrayTemplate = TempVertexList.ToArray();
			TempVertexList.Clear();
			Changed = true;
		}

		Animation LastAnim = null;
		int LastFrame = int.MinValue;
		void UpdateVertices(Animation anim, int frame)
		{
			if (!Changed && LastAnim == anim && LastFrame == frame)
				return;
			var tsize = anim.Texture.Bounds.Size.ToVector2();
			var rectTopLeft = anim.Frames[frame].Location.ToVector2() / tsize;
			var rectSize = anim.Frames[frame].Size.ToVector2() / tsize;
			for (var i = 0; i < VertexArray.Length; i++)
			{
				VertexArray[i] = new VertexPositionColorTexture(VertexArrayTemplate[i].Position, VertexArrayTemplate[i].Color, rectTopLeft + VertexArrayTemplate[i].TextureCoordinate * rectSize);
			}

			LastAnim = anim;
			LastFrame = frame;
		}

		public void Draw(Animation anim, double frameNumber, Matrix matrix, byte alpha)
		{
			if (anim == null)
				return;
			if (VertexArray.Length == 0)
				return;

			int frame = (int)Math.Floor(frameNumber) % anim.Length;
			if (frame < 0)
				frame += anim.Length;

			UpdateVertices(anim, frame);

			Graphics.BasicEffect.Alpha = (float)alpha / 255f;
			Graphics.BasicEffect.World = matrix;
			Graphics.BasicEffect.Texture = anim.Texture;
			foreach(EffectPass pass in Graphics.BasicEffect.CurrentTechnique.
				Passes)
			{
				pass.Apply();
				Graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, VertexArray, 0, VertexArray.Length / 3);
			}
		}
	}
}

