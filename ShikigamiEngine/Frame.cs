using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShikigamiEngine
{
	public static class Frame
	{
		public static Vector2 Position = new Vector2(32f, 16f), Size = new Vector2(384f, 448f);
		public static Vector2 Center = new Vector2(192f, 0f);
		public static float DeleteMargin = 48f;

		public static void Target()
		{
			Graphics.GraphicsDevice.Viewport = new Viewport(
				(int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y
			);
			Graphics.SetCamera2D(Vector2.Zero, Size, Center);
		}

		public static void Untarget()
		{
			Graphics.GraphicsDevice.Viewport = new Viewport(
				0,
				0,
				Graphics.GraphicsDeviceManager.PreferredBackBufferWidth,
				Graphics.GraphicsDeviceManager.PreferredBackBufferHeight
			);
			Graphics.SetCamera2D(
				Vector2.Zero,
				new Vector2(
					Graphics.GraphicsDeviceManager.PreferredBackBufferWidth,
					Graphics.GraphicsDeviceManager.PreferredBackBufferHeight
				),
				Vector2.Zero
			); 
		}
	}
}

