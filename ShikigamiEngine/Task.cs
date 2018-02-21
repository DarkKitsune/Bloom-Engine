using System;

using DrakeScript;

namespace ShikigamiEngine
{
	public class Task
	{
		public Coroutine Coroutine {get; private set;}
		public Entity Parent {get; private set;}
		public double NextResume;
		public bool Stopped {get; private set;}

		public Task(Entity parent, Function function)
		{
			Parent = parent;
			Coroutine = Script.Context.CreateCoroutine(function);
			Coroutine.Interpreter.DynamicLocalConstants.Add("this", Value.Create(parent));
			Coroutine.Interpreter.DynamicLocalConstants.Add("_task", Value.Create(this));
			NextResume = Engine.Time;
		}

		public Value Resume(params Value[] args)
		{
			Stopped = false;
			var ret = Coroutine.Resume(args);
			Stopped = (Coroutine.Status == CoroutineStatus.Stopped);
			return ret;
		}

		public void Stop()
		{
			if (Stopped)
				return;
			Stopped = true;
			Script.Tasks.Remove(this);
		}

		public void Yield()
		{
			if (Stopped)
				return;
			Coroutine.Yield();
		}
	}
}

