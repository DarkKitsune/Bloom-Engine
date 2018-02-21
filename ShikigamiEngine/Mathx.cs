using System;

namespace ShikigamiEngine
{
	public static class Mathx
	{
		public static Random Rand;
		public static int RandSeed;

		static Mathx()
		{
			RandSeed = (int)DateTime.Now.Ticks;
			Rand = new Random(RandSeed);
		}

		public static void Init()
		{
			
		}

		public static float AngleSubtract(float a, float b)
		{
			return ((((a - b) % 360f) + 540f) % 360f) - 180f;
		}
		public static float AngleFix(float ang)
		{
			ang = ang % 360f;
			if (ang < 0f)
				return ang + 360f;
			return ang;
		}

		public static float DCos(float angle)
		{
			return (float)Math.Cos(angle * 0.0174533);
		}
		public static float DSin(float angle)
		{
			return (float)Math.Sin(angle * 0.0174533);
		}
		public static double DCos(double angle)
		{
			return Math.Cos(angle * 0.0174533);
		}
		public static double DSin(double angle)
		{
			return Math.Sin(angle * 0.0174533);
		}
		public static float DAtan2(float y, float x)
		{
			return (float)Math.Atan2((double)y, (double)x) / 0.0174533f;
		}
		public static int GetRandom(int n)
		{
			return Rand.Next(n);
		}
		public static double GetRandom(double a, double b)
		{
			return a + (b - a) * Rand.NextDouble();
		}
		public static void GenSeed()
		{
			RandSeed = (int)DateTime.Now.Ticks;
			Rand = new Random(RandSeed);
		}
		public static void GenSeed(int seed)
		{
			RandSeed = seed;
			Rand = new Random(RandSeed);
		}
		public static float Lerp(float a, float b, float x)
		{
			return a + (b - a) * x;
		}

		public static float Clamp(float a, float min, float max)
		{
			return (a < min ? min : (a > max ? max : a));
		}

		public static double Clamp(double a, double min, double max)
		{
			return (a < min ? min : (a > max ? max : a));
		}
	}
}

