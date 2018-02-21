using System;

using Microsoft.Xna.Framework;

namespace ShikigamiEngine
{
	public struct TweenTrans1
	{
		public bool Enabled;
		public double StartTime;
		public double EndTime;
		public TweenType Tween;

		public bool Ended
		{
			get
			{
				return !Enabled || Engine.Time > EndTime;
			}
		}

		public float A1;
		public float A2;

		public TweenTrans1(float a1, float a2, TweenType tween, double endTime)
		{
			Enabled = true;
			A1 = a1;
			A2 = a2;
			StartTime = Engine.Time - 1.0;
			EndTime = endTime - 1.0;
			Tween = tween;
		}

		public float GetValue()
		{
			double timeRatio;
			var diff = EndTime - StartTime;
			if (diff < 0.000001)
				timeRatio = 1.0;
			else
				timeRatio = (Engine.Time - StartTime) / diff;
			return TweenMath.Eval(A1, A2, (float)timeRatio, Tween);
		}

		public float GetValueAngular()
		{
			double timeRatio;
			var diff = EndTime - StartTime;
			if (diff < 0.000001)
				timeRatio = 1.0;
			else
				timeRatio = (Engine.Time - StartTime) / diff;
			return TweenMath.EvalAngular(A1, A2, (float)timeRatio, Tween);
		}
	}

	public struct TweenTrans2
	{
		public bool Enabled;
		public double StartTime;
		public double EndTime;
		public TweenType Tween;

		public bool Ended
		{
			get
			{
				return !Enabled || Engine.Time > EndTime;
			}
		}

		public float A1;
		public float A2;
		public float B1;
		public float B2;

		public TweenTrans2(float a1, float a2, float b1, float b2, TweenType tween, double endTime)
		{
			Enabled = true;
			A1 = a1;
			A2 = a2;
			B1 = b1;
			B2 = b2;
			StartTime = Engine.Time - 1.0;
			EndTime = endTime - 1.0;
			Tween = tween;
		}

		public Tuple<float, float> GetValue()
		{
			double timeRatio;
			var diff = EndTime - StartTime;
			if (diff < 0.000001)
				timeRatio = 1.0;
			else
				timeRatio = (Engine.Time - StartTime) / diff;
			return new Tuple<float, float>(
				TweenMath.Eval(A1, A2, (float)timeRatio, Tween),
				TweenMath.Eval(B1, B2, (float)timeRatio, Tween)
			);
		}

		public Vector2 GetValueVector()
		{
			double timeRatio;
			var diff = EndTime - StartTime;
			if (diff < 0.000001)
				timeRatio = 1.0;
			else
				timeRatio = (Engine.Time - StartTime) / diff;
			return new Vector2(
				TweenMath.Eval(A1, A2, (float)timeRatio, Tween),
				TweenMath.Eval(B1, B2, (float)timeRatio, Tween)
			);
		}
	}

	public struct TweenTrans3
	{
		public bool Enabled;
		public double StartTime;
		public double EndTime;
		public TweenType Tween;

		public bool Ended
		{
			get
			{
				return !Enabled || Engine.Time > EndTime;
			}
		}

		public float A1;
		public float A2;
		public float B1;
		public float B2;
		public float C1;
		public float C2;

		public TweenTrans3(float a1, float a2, float b1, float b2, float c1, float c2, TweenType tween, double endTime)
		{
			Enabled = true;
			A1 = a1;
			A2 = a2;
			B1 = b1;
			B2 = b2;
			C1 = c1;
			C2 = c2;
			StartTime = Engine.Time - 1.0;
			EndTime = endTime - 1.0;
			Tween = tween;
		}

		public Tuple<float, float, float> GetValue()
		{
			double timeRatio;
			var diff = EndTime - StartTime;
			if (diff < 0.000001)
				timeRatio = 1.0;
			else
				timeRatio = (Engine.Time - StartTime) / diff;
			return new Tuple<float, float, float>(
				TweenMath.Eval(A1, A2, (float)timeRatio, Tween),
				TweenMath.Eval(B1, B2, (float)timeRatio, Tween),
				TweenMath.Eval(C1, C2, (float)timeRatio, Tween)
			);
		}

		public Vector3 GetValueVector()
		{
			double timeRatio;
			var diff = EndTime - StartTime;
			if (diff < 0.000001)
				timeRatio = 1.0;
			else
				timeRatio = (Engine.Time - StartTime) / diff;
			return new Vector3(
				TweenMath.Eval(A1, A2, (float)timeRatio, Tween),
				TweenMath.Eval(B1, B2, (float)timeRatio, Tween),
				TweenMath.Eval(C1, C2, (float)timeRatio, Tween)
			);
		}
	}
}

