using System;

using DrakeScript;

using Microsoft.Xna.Framework;

namespace ShikigamiEngine
{
	public class TaskObject : Entity
	{
		public TaskObject(Function func) : base(func, Layer.TaskObjects, Vector2.Zero)
		{
		}
	}
}

