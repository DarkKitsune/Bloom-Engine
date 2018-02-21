using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShikigamiEngine
{
	public static class Input
	{
		static KeyboardState LastKeyState;
		static KeyboardState KeyState;

		public static void Update()
		{
			LastKeyState = KeyState;
			KeyState = Keyboard.GetState();
		}

		public static bool GetKeyPressed(Keys key)
		{
			return KeyState.IsKeyDown(key);
		}

		public static bool GetKeyReleased(Keys key)
		{
			return KeyState.IsKeyUp(key);
		}

		public static bool GetKeyPress(Keys key)
		{
			return KeyState.IsKeyDown(key) && !LastKeyState.IsKeyDown(key);
		}

		public static bool GetKeyRelease(Keys key)
		{
			return !KeyState.IsKeyDown(key) && !LastKeyState.IsKeyDown(key);
		}
	}
}

