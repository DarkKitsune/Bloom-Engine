using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShikigamiEngine
{
	public static class Graphics
	{
		public static GraphicsDevice GraphicsDevice;
		public static GraphicsDeviceManager GraphicsDeviceManager;
		public static SpriteBatch SpriteBatch;
		public static BasicEffect BasicEffect;
		public static RasterizerState RasterizerStatePrimitive;
		public static RasterizerState RasterizerStateSprite;
		public static DepthStencilState DepthStencilStatePrimitive;
		public static DepthStencilState DepthStencilStateSprite;
		public static SamplerState SamplerStatePrimitive;
		public static SamplerState SamplerStateSprite;

		public static void SetCamera2D(Vector2 position, Vector2 size, Vector2 zeroPoint)
		{
			BasicEffect.View = Matrix.CreateLookAt(
				new Vector3((float)Math.Round(position.X) + 0.5f, (float)Math.Round(position.Y) + 0.5f, 5000f),
				new Vector3((float)Math.Round(position.X) + 0.5f, (float)Math.Round(position.Y) + 0.5f, 0f),
				-Vector3.UnitY
			);
			BasicEffect.Projection = Matrix.CreateOrthographicOffCenter(
				(float)Math.Round(zeroPoint.X), (float)Math.Round(zeroPoint.X) - (float)Math.Round(size.X), (float)Math.Round(zeroPoint.Y) - (float)Math.Round(size.Y), (float)Math.Round(zeroPoint.Y), 0.1f, 10000f
			);
		}

		public static void SetStates()
		{
			GraphicsDevice.RasterizerState = RasterizerStatePrimitive;
			GraphicsDevice.DepthStencilState = DepthStencilStatePrimitive;
			GraphicsDevice.SamplerStates[0] = SamplerStatePrimitive;
			GraphicsDevice.BlendState = BlendState.AlphaBlend;
		}

		public static void DrawText(Vector2 pos, string text, Font font, Color color)
		{
			SpriteBatch.Begin();
			SpriteBatch.DrawString(font.SpriteFont, text, pos, color);
			SpriteBatch.End();
			SetStates();
		}

		public static void DrawTextShadowed(Vector2 pos, string text, Font font, Color color)
		{
			SpriteBatch.Begin();
			SpriteBatch.DrawString(font.SpriteFont, text, pos + Vector2.One, new Color(63, 63, 63));
			SpriteBatch.DrawString(font.SpriteFont, text, pos, color);
			SpriteBatch.End();
			SetStates();
		}

		public static void DrawTextShadowed(Vector2 pos, string text, Font font, Color color, Color shadowColor)
		{
			SpriteBatch.Begin();
			SpriteBatch.DrawString(font.SpriteFont, text, pos + Vector2.One, shadowColor);
			SpriteBatch.DrawString(font.SpriteFont, text, pos, color);
			SpriteBatch.End();
			SetStates();
		}
	}
}

