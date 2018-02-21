using System;

using DrakeScript;

namespace ShikigamiEngine
{
	public static class Effects
	{
		public static void Register()
		{
			var table = new Table();

			table["test"] = Script.Context.LoadString(@"
"
			);

			Script.Context.SetGlobal("effect", table);
		}
	}
}

