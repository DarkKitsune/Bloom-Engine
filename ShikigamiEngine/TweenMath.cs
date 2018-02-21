using System;

namespace ShikigamiEngine
{
	public static class TweenMath
	{
		public static float Eval(float a, float b, float x, TweenType tween)
		{
			if (x < 0f)
				x = 0f;
			if (x > 1f)
				x = 1f;
			var diff = (b - a);
			switch (tween)
			{
				case (TweenType.Linear):
					return a + diff * x;
				case (TweenType.Accelerate):
					//n = (x * x) * (1f - x);
					//return a + diff * (n * n * n + x * x);
					return a + diff * (-(x * x * x) + 2 * x * x);
				case (TweenType.Decelerate):
					return a + diff * (x * x - x * x * x + x);
				case (TweenType.Smooth):
					if (x > 0.5f)
						return a + diff * Eval(0.5f, 1f, (x - 0.5f) * 2f, TweenType.Decelerate);
					return a + diff * Eval(0f, 0.5f, x * 2f, TweenType.Accelerate);
			}
			return 0f;
		}

		public static float EvalAngular(float a, float b, float x, TweenType tween)
		{
			if (x < 0f)
				x = 0f;
			if (x > 1f)
				x = 1f;
			var diff = Mathx.AngleSubtract(b, a);
			switch (tween)
			{
				case (TweenType.Linear):
					return a + diff * x;
				case (TweenType.Accelerate):
					//n = (x * x) * (1f - x);
					//return a + diff * (n * n * n + x * x);
					return a + diff * (-(x * x * x) + 2 * x * x);
				case (TweenType.Decelerate):
					return a + diff * (x * x - x * x * x + x);
				case (TweenType.Smooth):
					if (x > 0.5f)
						return a + diff * Eval(0.5f, 1f, (x - 0.5f) * 2f, TweenType.Decelerate);
					return a + diff * Eval(0f, 0.5f, x * 2f, TweenType.Accelerate);
			}
			return 0f;
		}
	}
}

