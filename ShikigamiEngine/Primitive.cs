using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShikigamiEngine
{
	public class Primitive : IDisposable
	{
		static VertexBuffer CurrentVB = null;

		public static Primitive Empty;
		public static Primitive Square;
		public static Primitive SquareFacing;
		public static Primitive Block;
		public static void CreateBasicPrimitives()
		{
			Empty = new Primitive();
			Empty.Begin();
			Empty.AddVertex(Vector3.Zero, Color.White, Vector2.Zero);
			Empty.AddVertex(Vector3.Zero, Color.White, Vector2.Zero);
			Empty.AddVertex(Vector3.Zero, Color.White, Vector2.Zero);
			Empty.End();

			Square = new Primitive();
			Square.Begin();
			Square.AddVertex(new Vector3(-0.5f, -0.5f, 0f), Color.White, Vector2.Zero);
			Square.AddVertex(new Vector3(0.5f, -0.5f, 0f), Color.White, new Vector2(1f, 0f));
			Square.AddVertex(new Vector3(0.5f, 0.5f, 0f), Color.White, Vector2.One);
			Square.AddVertex(new Vector3(0.5f, 0.5f, 0f), Color.White, Vector2.One);
			Square.AddVertex(new Vector3(-0.5f, 0.5f, 0f), Color.White, new Vector2(0f, 1f));
			Square.AddVertex(new Vector3(-0.5f, -0.5f, 0f), Color.White, Vector2.Zero);
			Square.End();

			SquareFacing = new Primitive();
			SquareFacing.Begin();
			SquareFacing.AddVertex(new Vector3(0f, -0.5f, 0.5f), Color.White, Vector2.Zero);
			SquareFacing.AddVertex(new Vector3(0f, 0.5f, 0.5f), Color.White, new Vector2(1f, 0f));
			SquareFacing.AddVertex(new Vector3(0f, 0.5f, -0.5f), Color.White, Vector2.One);
			SquareFacing.AddVertex(new Vector3(0f, 0.5f, -0.5f), Color.White, Vector2.One);
			SquareFacing.AddVertex(new Vector3(0f, -0.5f, -0.5f), Color.White, new Vector2(0f, 1f));
			SquareFacing.AddVertex(new Vector3(0f, -0.5f, 0.5f), Color.White, Vector2.Zero);
			SquareFacing.End();
		}



		VertexBuffer VertexBuffer = null;
		public int Count {get; private set;}
		List<VertexPositionColorTexture> Vertices = new List<VertexPositionColorTexture>();

		public Primitive()
		{
			
		}

		~Primitive()
		{
			VertexBuffer?.Dispose();
		}

		public void Dispose()
		{
			VertexBuffer?.Dispose();
			Vertices.Clear();
			VertexBuffer = null;
		}

		public void Begin()
		{
			Vertices.Clear();
		}

		public void AddVertex(Vector3 position, Color color, Vector2 uv)
		{
			Vertices.Add(new VertexPositionColorTexture(position, color, uv));
		}
			
		static Vector3 AddB3 = new Vector3(1f, 1f, 0f);
		static Vector3 AddT1 = new Vector3(0f, 0f, 1f);
		static Vector3 AddT2 = new Vector3(1f, 0f, 1f);
		static Vector3 AddT4 = new Vector3(0f, 1f, 1f);
		public void AddBlock(
			Vector3 position,
			Color colorW,
			Color colorE,
			Color colorN,
			Color colorS,
			Color colorB,
			Color colorT,
			Vector2 uvW1, Vector2 uvW2,
			Vector2 uvE1, Vector2 uvE2,
			Vector2 uvN1, Vector2 uvN2,
			Vector2 uvS1, Vector2 uvS2,
			Vector2 uvB1, Vector2 uvB2,
			Vector2 uvT1, Vector2 uvT2,
			bool west, bool east, bool north, bool south, bool bottom, bool top
		)
		{
			var posb1 = position;
			var posb2 = position + Vector3.UnitX;
			var posb3 = position + AddB3;
			var posb4 = position + Vector3.UnitY;
			var post1 = position + AddT1;
			var post2 = position + AddT2;
			var post3 = position + Vector3.One;
			var post4 = position + AddT4;

			var uvWf1 = uvW1;
			var uvWf2 = new Vector2(uvW2.X, uvW1.Y);
			var uvWf3 = uvW2;
			var uvWf4 = new Vector2(uvW1.X, uvW2.Y);

			var uvEf1 = uvE1;
			var uvEf2 = new Vector2(uvE2.X, uvE1.Y);
			var uvEf3 = uvE2;
			var uvEf4 = new Vector2(uvE1.X, uvE2.Y);

			var uvNf1 = uvN1;
			var uvNf2 = new Vector2(uvN2.X, uvN1.Y);
			var uvNf3 = uvN2;
			var uvNf4 = new Vector2(uvN1.X, uvN2.Y);

			var uvSf1 = uvS1;
			var uvSf2 = new Vector2(uvS2.X, uvS1.Y);
			var uvSf3 = uvS2;
			var uvSf4 = new Vector2(uvS1.X, uvS2.Y);

			var uvBf1 = uvB1;
			var uvBf2 = new Vector2(uvB2.X, uvB1.Y);
			var uvBf3 = uvB2;
			var uvBf4 = new Vector2(uvB1.X, uvB2.Y);

			var uvTf1 = uvT1;
			var uvTf2 = new Vector2(uvT2.X, uvT1.Y);
			var uvTf3 = uvT2;
			var uvTf4 = new Vector2(uvT1.X, uvT2.Y);

			//bottom
			if (bottom)
			{
				Vertices.Add(new VertexPositionColorTexture(posb2, colorB, uvBf1));
				Vertices.Add(new VertexPositionColorTexture(posb1, colorB, uvBf2));
				Vertices.Add(new VertexPositionColorTexture(posb4, colorB, uvBf3));
				Vertices.Add(new VertexPositionColorTexture(posb4, colorB, uvBf3));
				Vertices.Add(new VertexPositionColorTexture(posb3, colorB, uvBf4));
				Vertices.Add(new VertexPositionColorTexture(posb2, colorB, uvBf1));
			}

			//top
			if (top)
			{
				Vertices.Add(new VertexPositionColorTexture(post1, colorT, uvTf1));
				Vertices.Add(new VertexPositionColorTexture(post2, colorT, uvTf2));
				Vertices.Add(new VertexPositionColorTexture(post3, colorT, uvTf3));
				Vertices.Add(new VertexPositionColorTexture(post3, colorT, uvTf3));
				Vertices.Add(new VertexPositionColorTexture(post4, colorT, uvTf4));
				Vertices.Add(new VertexPositionColorTexture(post1, colorT, uvTf1));
			}

			//east
			if (east)
			{
				Vertices.Add(new VertexPositionColorTexture(post1, colorE, uvEf2));
				Vertices.Add(new VertexPositionColorTexture(post4, colorE, uvEf1));
				Vertices.Add(new VertexPositionColorTexture(posb4, colorE, uvEf4));
				Vertices.Add(new VertexPositionColorTexture(posb4, colorE, uvEf4));
				Vertices.Add(new VertexPositionColorTexture(posb1, colorE, uvEf3));
				Vertices.Add(new VertexPositionColorTexture(post1, colorE, uvEf2));
			}

			//west
			if (west)
			{
				Vertices.Add(new VertexPositionColorTexture(post3, colorW, uvWf2));
				Vertices.Add(new VertexPositionColorTexture(post2, colorW, uvWf1));
				Vertices.Add(new VertexPositionColorTexture(posb2, colorW, uvWf4));
				Vertices.Add(new VertexPositionColorTexture(posb2, colorW, uvWf4));
				Vertices.Add(new VertexPositionColorTexture(posb3, colorW, uvWf3));
				Vertices.Add(new VertexPositionColorTexture(post3, colorW, uvWf2));
			}

			//north
			if (north)
			{
				Vertices.Add(new VertexPositionColorTexture(post2, colorN, uvNf2));
				Vertices.Add(new VertexPositionColorTexture(post1, colorN, uvNf1));
				Vertices.Add(new VertexPositionColorTexture(posb1, colorN, uvNf4));
				Vertices.Add(new VertexPositionColorTexture(posb1, colorN, uvNf4));
				Vertices.Add(new VertexPositionColorTexture(posb2, colorN, uvNf3));
				Vertices.Add(new VertexPositionColorTexture(post2, colorN, uvNf2));
			}

			//south
			if (south)
			{
				Vertices.Add(new VertexPositionColorTexture(post4, colorS, uvSf2));
				Vertices.Add(new VertexPositionColorTexture(post3, colorS, uvSf1));
				Vertices.Add(new VertexPositionColorTexture(posb3, colorS, uvSf4));
				Vertices.Add(new VertexPositionColorTexture(posb3, colorS, uvSf4));
				Vertices.Add(new VertexPositionColorTexture(posb4, colorS, uvSf3));
				Vertices.Add(new VertexPositionColorTexture(post4, colorS, uvSf2));
			}
		}

		public void End()
		{
			VertexBuffer?.Dispose();

			if (Vertices.Count == 0)
			{
				VertexBuffer = null;
				return;
			}

			VertexBuffer = new VertexBuffer(
				Graphics.GraphicsDevice,
				typeof(VertexPositionColorTexture),
				Vertices.Count,
				BufferUsage.WriteOnly
			);
			VertexBuffer.SetData<VertexPositionColorTexture>(Vertices.ToArray());
			Vertices.Clear();
			Count = Vertices.Count;
		}

		public void Draw(Matrix matrix, byte alpha, Texture2D texture)
		{
			if (VertexBuffer == null)
				return;
			if (CurrentVB != VertexBuffer)
			{
				Graphics.GraphicsDevice.SetVertexBuffer(VertexBuffer);
				CurrentVB = VertexBuffer;
			}
			Graphics.BasicEffect.Alpha = (float)alpha / 255f;
			Graphics.BasicEffect.World = matrix;
			Graphics.BasicEffect.Texture = texture;
			foreach(EffectPass pass in Graphics.BasicEffect.CurrentTechnique.
				Passes)
			{
				pass.Apply();
				Graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, VertexBuffer.VertexCount / 3);
			}
		}
	}
}

