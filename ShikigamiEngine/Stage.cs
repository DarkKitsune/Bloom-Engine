using System;
using System.Collections.Generic;

using DrakeScript;

namespace ShikigamiEngine
{
	public struct Stage
	{
		static List<Stage> Stages = new List<Stage>();
		public static int Current {get; private set;} = 0;

		public static void AddStage(string path)
		{
			Stages.Add(new Stage(path));
		}


		public static void NextStage()
		{
			if (Current > 0)
				Stages[Current - 1].Stop();
			Current++;
			if (Current > 0 && Current <= Stages.Count)
			{
				Stages[Current - 1].Start();
			}
			else
				throw new Exception("Stage " + Current + " does not exist!");
		}


		Function Function;
		public TaskObject Task {get; private set;}
		Stage(string path)
		{
			Function = Script.LoadFile(path);
			Task = null;
		}

		void Start()
		{
			if (Task != null)
				Task.Delete();
			Task = new TaskObject(Function);
		}

		void Stop()
		{
			if (Task != null)
				Task.Delete();
		}
	}
}

