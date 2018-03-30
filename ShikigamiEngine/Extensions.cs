using System;

using DrakeScript;

using Microsoft.Xna.Framework;

namespace ShikigamiEngine
{
	public static class Extensions
	{
		/*
		public static Table ToTable(this Vector2 vec)
		{
			return new Table(new System.Collections.Generic.Dictionary<object, Value> {
				{ "x", vec.X },
				{ "y", vec.Y }
			});
		}*/

        public static bool EqualsSafe(this double a, double b)
        {
            return a > b * 0.9999999 && a < b * 1.0000001;
        }

        public static bool EqualsZeroSafe(this double a)
        {
            return a > -0.00000001 && a < 0.00000001;
        }

        public static bool DoesNotEqualZeroSafe(this double a)
        {
            return a <= -0.00000001 || a >= 0.00000001;
        }
    }
}

