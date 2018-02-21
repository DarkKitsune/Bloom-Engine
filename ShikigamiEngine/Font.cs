using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShikigamiEngine
{
	public class Font
	{
		public static Font Small;
		public static Font Medium;

		public static void Init()
		{
			Small = new Font(Resources.ContentManager.Load<SpriteFont>("fontSmall"));
			Medium = new Font(Resources.ContentManager.Load<SpriteFont>("fontMedium"));
		}

		public SpriteFont SpriteFont;

		public Font(SpriteFont spriteFont)
		{
			SpriteFont = spriteFont;
		}
	}
}

