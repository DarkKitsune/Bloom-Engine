using System;

namespace ShikigamiEngine
{
	public static class Engine
	{
		static double LastFrameMS;
		static double LastUpdateMS;
		public static int DrawFrame {get; private set;}
		public static double Time {get; private set;}
		public static double UPS {get; private set;}
		public static double FPS {get; private set;}
		public static int BulletCount {get; private set;}

		public static void Init()
		{
			LastFrameMS = (double)DateTime.Now.Ticks / (double)TimeSpan.TicksPerMillisecond;
		}

		public static void Update()
		{
			
		}

		public static void EndUpdate()
		{
			if (Time % 10 == 0)
			{
				BulletCount = Layer.Bullets.Entities.Count;
			}

			if (Time % 30 == 0)
			{
				var nowMS = (double)DateTime.Now.Ticks / (double)TimeSpan.TicksPerMillisecond;

				var msPerFrame = (nowMS - LastUpdateMS) / 30.0;
				var sPerFrame = msPerFrame / 1000.0;
				UPS = 1.0 / sPerFrame;

				LastUpdateMS = nowMS;
			}

			Time++;
		}

		public static void Draw()
		{
			if (DrawFrame % 30 == 0)
			{
				var nowMS = (double)DateTime.Now.Ticks / (double)TimeSpan.TicksPerMillisecond;

				var msPerFrame = (nowMS - LastFrameMS) / 30.0;
				var sPerFrame = msPerFrame / 1000.0;
				FPS = 1.0 / sPerFrame;

				LastFrameMS = nowMS;
			}

			DrawFrame++;
		}
	}
}

