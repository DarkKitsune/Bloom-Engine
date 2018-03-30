using System;
using System.Collections.Generic;

using DrakeScript;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShikigamiEngine
{
	public static class Libs
	{
		public static void Register()
		{
			LibSystem.Register();
			LibMath.Register();
			LibAnimations.Register();
			LibObjects.Register();
			LibBullets.Register();
			LibPatterns.Register();
			LibPlayer.Register();
			LibEnemies.Register();
			LibBosses.Register();
			Effects.Register();
		}


		public static class LibSystem
		{
			public static void Register()
			{
				Script.Context.SetGlobal("task", Script.Context.CreateFunction(TaskCreate, 1));
                Script.Context.AddMethod(typeof(Task), "stop", Script.Context.CreateFunction(TaskStop, 1));

                Script.Context.SetGlobal("wait", Script.Context.CreateFunction(Wait, 1));
				Script.Context.SetGlobal("choose", Script.Context.CreateFunction(Choose, 1));
				//Script.Context.SetGlobal("next", Script.Context.CreateFunction(Next, 1));
				Script.Context.SetGlobal("directory", Script.Context.CreateFunction(CurrentDirectory, 0));
				Script.Context.SetGlobal("addStage", Script.Context.CreateFunction(AddStage, 0));
                Script.Context.SetGlobal("loadScript", Script.Context.CreateFunction(LoadScript, 0));
                Script.Context.SetGlobal("runScript", Script.Context.CreateFunction(RunScript, 0));

                Script.Context.SetGlobal("BLEND_ALPHA", Value.Create(BlendState.AlphaBlend));
                Script.Context.SetGlobal("BLEND_ADD", Value.Create(BlendState.Additive));

                Script.Context.SetGlobal("TWEEN_LINEAR", Value.Create(TweenType.Linear));
                Script.Context.SetGlobal("TWEEN_ACCELERATE", Value.Create(TweenType.Accelerate));
                Script.Context.SetGlobal("TWEEN_DECELERATE", Value.Create(TweenType.Decelerate));
                Script.Context.SetGlobal("TWEEN_SMOOTH", Value.Create(TweenType.Smooth));
			}

			public static Value TaskCreate(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = ((Entity)interpreter.DynamicLocalConstants["this"].Object);
				return Value.Create(Script.CreateTask(obj, args[0].VerifyType(Value.ValueType.Function, location).Function));
			}

            public static Value TaskStop(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
            {
                args[0].VerifyType<Task>(location).ObjectAs<Task>().Stop();
                return Value.Nil;
            }

            public static Value Wait(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				if (args[0].Number <= 0)
					return Value.Nil;
				Value temp;
				if (!interpreter.DynamicLocalConstants.TryGetValue("_task", out temp))
					throw new InterpreterException("wait() can only be called within a task or event task", location);
				var task = temp.ObjectAs<Task>();
				task.Yield();
				task.NextResume = Engine.Time + args[0].Number;
				return Value.Nil;
			}

			public static Value Choose(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var arr = args[0].VerifyType(Value.ValueType.Array, location).Array;
				return arr[Mathx.GetRandom(arr.Count)];
			}

            //static Dictionary<Tuple<SourceRef, Interpreter>, int> nextPosition = new Dictionary<Tuple<SourceRef, Interpreter>, int>();
            /*public static Value Next(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var arr = args[0].VerifyType(Value.ValueType.Array, location).Array;
				if (arr.Count == 0)
					return Value.Nil;
				int pos = 0;
				Dictionary<SourceRef, int> nextPos;
				Value temp;
				if (!interpreter.DynamicLocalConstants.TryGetValue("nextPos", out temp))
				{
					nextPos = new Dictionary<SourceRef, int>();
					interpreter.DynamicLocalConstants.Add("nextPos", Value.Create(nextPos));
				}
				else
					nextPos = (Dictionary<SourceRef, int>)temp.Object;
				nextPos.TryGetValue(location, out pos);
				nextPos[location] = pos + 1;
				return arr[pos % arr.Count];
			}*/

            public static Value CurrentDirectory(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
            {
                try
                {
                    return System.IO.Path.GetDirectoryName(interpreter.CallLocation.Source.Name) + "/";
                }
                catch (Exception e)
                {
                    return "";
                }
            }

            public static Value AddStage(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				Stage.AddStage(args[0].VerifyType(Value.ValueType.String, location));
				return Value.Nil;
			}

			public static Value LoadScript(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				return Script.LoadFile(args[0].VerifyType(Value.ValueType.String, location));
			}

            public static Value RunScript(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
            {
                return Script.LoadFile(args[0].VerifyType(Value.ValueType.String, location)).Invoke();
            }
		}

		public static class LibMath
		{
			public static void Register()
			{
				Script.Context.SetGlobal("rand", Script.Context.CreateFunction(Rand, 1));
			}

			public static Value Rand(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				if (argCount > 1)
					return Mathx.GetRandom(args[0].VerifyType(Value.ValueType.Number, location).Number, args[1].VerifyType(Value.ValueType.Number, location).Number);
				return Mathx.GetRandom(0.0, args[0].VerifyType(Value.ValueType.Number, location).Number);
			}


		}

		public static class LibObjects
		{
			public static void Register()
			{
				Script.Context.SetGlobal("object.create", Script.Context.CreateFunction(Create, 3));
				Script.Context.SetGlobal("frameObject.create", Script.Context.CreateFunction(CreateFrame, 3));
				Script.Context.SetGlobal("delete", Script.Context.CreateFunction(Delete, 0));
				Script.Context.SetGlobal("destroy", Script.Context.CreateFunction(Destroy, 0));

				Script.Context.SetGlobal("setAnimation", Script.Context.CreateFunction(SetAnimation, 1));
				Script.Context.SetGlobal("getAnimation", Script.Context.CreateFunction(GetAnimation, 0));
				Script.Context.SetGlobal("resetAnimation", Script.Context.CreateFunction(ResetAnimation, 0));
				Script.Context.SetGlobal("setScale", Script.Context.CreateFunction(SetScale, 2));
				Script.Context.SetGlobal("setAlpha", Script.Context.CreateFunction(SetAlpha, 1));
				Script.Context.SetGlobal("getAlpha", Script.Context.CreateFunction(GetAlpha, 0));
				Script.Context.SetGlobal("setGlow", Script.Context.CreateFunction(SetGlow, 1));
				Script.Context.SetGlobal("getGlow", Script.Context.CreateFunction(GetGlow, 0));
				Script.Context.SetGlobal("setPosition", Script.Context.CreateFunction(SetPosition, 2));
				Script.Context.SetGlobal("getX", Script.Context.CreateFunction(GetX, 0));
				Script.Context.SetGlobal("getY", Script.Context.CreateFunction(GetY, 0));
				Script.Context.SetGlobal("setSpeed", Script.Context.CreateFunction(SetSpeed, 1));
				Script.Context.SetGlobal("setSpeedX", Script.Context.CreateFunction(SetSpeedX, 1));
				Script.Context.SetGlobal("setSpeedY", Script.Context.CreateFunction(SetSpeedY, 1));
				Script.Context.SetGlobal("getSpeed", Script.Context.CreateFunction(GetSpeed, 0));
				Script.Context.SetGlobal("getSpeedX", Script.Context.CreateFunction(GetSpeedX, 0));
				Script.Context.SetGlobal("getSpeedY", Script.Context.CreateFunction(GetSpeedY, 0));
				Script.Context.SetGlobal("setAngle", Script.Context.CreateFunction(SetAngle, 1));
				Script.Context.SetGlobal("setRenderAngle", Script.Context.CreateFunction(SetRenderAngle, 1));
				Script.Context.SetGlobal("setRenderAngleOffset", Script.Context.CreateFunction(SetRenderAngleOffset, 1));
				Script.Context.SetGlobal("setBlend", Script.Context.CreateFunction(SetBlend, 1));
				Script.Context.SetGlobal("setHitbox", Script.Context.CreateFunction(SetHitbox, 1));
				Script.Context.SetGlobal("destroyBullets", Script.Context.CreateFunction(DestroyBullets, 1));

				Script.Context.SetGlobal("move", Script.Context.CreateFunction(Move, 4));
				Script.Context.SetGlobal("accelerate", Script.Context.CreateFunction(Accelerate, 3));
				Script.Context.SetGlobal("turn", Script.Context.CreateFunction(Turn, 3));
				Script.Context.SetGlobal("rotate", Script.Context.CreateFunction(Rotate, 3));
				Script.Context.SetGlobal("changeScale", Script.Context.CreateFunction(ChangeScale, 4));
				Script.Context.SetGlobal("changeAlpha", Script.Context.CreateFunction(ChangeAlpha, 3));
				Script.Context.SetGlobal("changeGlow", Script.Context.CreateFunction(ChangeGlow, 3));
			}

			public static Value Create(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				return Value.Create(
					new Entity(
						args[0].Function,
						Layer.Misc,
						new Vector2(
							(float)args[1].VerifyType(Value.ValueType.Number, location).Number,
							(float)args[2].VerifyType(Value.ValueType.Number, location).Number
						)
					)
				);
			}

			public static Value CreateFrame(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				return Value.Create(
					new Entity(
						args[0].Function,
						Layer.FrameObjects,
						new Vector2(
							(float)args[1].VerifyType(Value.ValueType.Number, location).Number,
							(float)args[2].VerifyType(Value.ValueType.Number, location).Number
						)
					)
				);
			}

			public static Value Delete(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.Delete();
				return Value.Nil;
			}

			public static Value Destroy(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.Destroy();
				return Value.Nil;
			}

			public static Value SetAnimation(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.Animation = args[0].VerifyType<Animation>(location).Object as Animation;
				return Value.Nil;
			}

			public static Value GetAnimation(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				return Value.Create(obj.Animation);
			}

			public static Value ResetAnimation(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.ResetAnimation();
				return Value.Nil;
			}

			public static Value SetScale(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.Scale = new Vector2(
					(float)args[0].VerifyType(Value.ValueType.Number, location).Number,
					(float)args[1].VerifyType(Value.ValueType.Number, location).Number
				);
				return Value.Nil;
			}

			public static Value SetAlpha(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.Alpha = (byte)Mathx.Clamp(
					args[0].VerifyType(Value.ValueType.Number, location).Number,
					0.0,
					255.0
				);
				return Value.Nil;
			}

			public static Value GetAlpha(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				return obj.Alpha;
			}

			public static Value SetGlow(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.Glow = (byte)Mathx.Clamp(
					args[0].VerifyType(Value.ValueType.Number, location).Number,
					0.0,
					255.0
				);
				return Value.Nil;
			}

			public static Value GetGlow(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				return obj.Glow;
			}

			public static Value SetPosition(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.Position = new Vector2(
					(float)args[0].VerifyType(Value.ValueType.Number, location).Number,
					(float)args[1].VerifyType(Value.ValueType.Number, location).Number
				);
				return Value.Nil;
			}

			public static Value GetX(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				return obj.Position.X;
			}

			public static Value GetY(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				return obj.Position.Y;
			}

			public static Value SetSpeed(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.Speed = (float)args[0].VerifyType(Value.ValueType.Number, location).Number;
				return Value.Nil;
			}

			public static Value SetSpeedX(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.SpeedX = (float)args[0].VerifyType(Value.ValueType.Number, location).Number;
				return Value.Nil;
			}

			public static Value SetSpeedY(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.SpeedY = (float)args[0].VerifyType(Value.ValueType.Number, location).Number;
				return Value.Nil;
			}

			public static Value GetSpeedX(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				return obj.SpeedX;
			}

			public static Value GetSpeedY(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				return obj.SpeedY;
			}

			public static Value GetSpeed(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				return obj.Speed;
			}

			public static Value SetAngle(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.Angle = (float)args[0].VerifyType(Value.ValueType.Number, location).Number;
				return Value.Nil;
			}

			public static Value SetRenderAngle(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.RenderAngle = (float)args[0].VerifyType(Value.ValueType.Number, location).Number;
				if (argCount > 1)
					obj.RenderAngleOffset = (float)args[1].VerifyType(Value.ValueType.Number, location).Number;
				return Value.Nil;
			}

			public static Value SetRenderAngleOffset(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.RenderAngleOffset = (float)args[0].VerifyType(Value.ValueType.Number, location).Number;
				return Value.Nil;
			}

			public static Value SetBlend(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.BlendState = (BlendState)args[0].VerifyType<BlendState>(location).Object;
				return Value.Nil;
			}

			public static Value SetHitbox(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.HitboxRadius = (float)args[0].VerifyType(Value.ValueType.Number, location).Number;
				return Value.Nil;
			}

			public static Value DestroyBullets(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				var x = obj.Position.X;
				var y = obj.Position.Y;
				var radius = (float)args[0].VerifyType(Value.ValueType.Number,location).Number;
				radius *= radius;
				foreach (var bullet in Layer.Bullets.Entities)
				{
					if (!bullet.CanDestroy)
						continue;
					var diffX = bullet.Position.X - x;
					var diffY = bullet.Position.Y - y;
					if (diffX * diffX + diffY * diffY <= radius)
						bullet.Destroy();
				}
				return Value.Nil;
			}


			public static Value Move(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.Move(
					new Vector2(
						(float)args[0].VerifyType(Value.ValueType.Number, location).Number,
						(float)args[1].VerifyType(Value.ValueType.Number, location).Number
					),
					(TweenType)args[2].VerifyType<TweenType>(location).Object,
					args[3].VerifyType(Value.ValueType.Number, location).Number
				);
				return Value.Nil;
			}

			public static Value Accelerate(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.Accelerate(
					(float)args[0].VerifyType(Value.ValueType.Number, location).Number,
					(TweenType)args[1].VerifyType<TweenType>(location).Object,
					args[2].VerifyType(Value.ValueType.Number, location).Number
				);
				return Value.Nil;
			}

			public static Value Turn(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.Turn(
					(float)args[0].VerifyType(Value.ValueType.Number, location).Number,
					(TweenType)args[1].VerifyType<TweenType>(location).Object,
					args[2].VerifyType(Value.ValueType.Number, location).Number
				);
				return Value.Nil;
			}

			public static Value Rotate(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.Rotate(
					(float)args[0].VerifyType(Value.ValueType.Number, location).Number,
					(TweenType)args[1].VerifyType<TweenType>(location).Object,
					args[2].VerifyType(Value.ValueType.Number, location).Number
				);
				return Value.Nil;
			}

			public static Value ChangeScale(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.ChangeScale(
					new Vector2(
						(float)args[0].VerifyType(Value.ValueType.Number, location).Number,
						(float)args[1].VerifyType(Value.ValueType.Number, location).Number
					),
					(TweenType)args[2].VerifyType<TweenType>(location).Object,
					args[3].VerifyType(Value.ValueType.Number, location).Number
				);
				return Value.Nil;
			}

			public static Value ChangeAlpha(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.ChangeAlpha(
					(byte)args[0].VerifyType(Value.ValueType.Number, location).Number,
					(TweenType)args[1].VerifyType<TweenType>(location).Object,
					args[2].VerifyType(Value.ValueType.Number, location).Number
				);
				return Value.Nil;
			}

			public static Value ChangeGlow(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var obj = interpreter.DynamicLocalConstants["this"].Object as Entity;
				obj.ChangeGlow(
					(byte)args[0].VerifyType(Value.ValueType.Number, location).Number,
					(TweenType)args[1].VerifyType<TweenType>(location).Object,
					args[2].VerifyType(Value.ValueType.Number, location).Number
				);
				return Value.Nil;
			}
		}

		public static class LibBullets
		{
			public static void Register()
			{
				Script.Context.SetGlobal("bulletDef", Script.Context.CreateFunction(CreateBulletDefinitions, 1));
				Script.Context.SetGlobal("bullet", new Table());
				Script.Context.SetGlobal("bullet.create", Script.Context.CreateFunction(CreateBullet, 5));
				Script.Context.SetGlobal("bullet.destroyInCircle", Script.Context.CreateFunction(DestroyInCircle, 3));

				Script.Context.SetGlobal("getBulletType", Script.Context.CreateFunction(GetBulletType, 0));
			}

			public static Value CreateBulletDefinitions(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var defs = new Table();
				args[0].VerifyType(Value.ValueType.Table, location);
				var table = args[0].Table;
				var tex = table["texture"].String;
				var speed = table["speed"].Number;
				var hitbox = (float)table["hitbox"].Number;
				var x = (int)table["x"].Number;
				var y = (int)table["y"].Number;
				var width = (int)table["width"].Number;
				var height = (int)table["height"].Number;
				var frames = (int)table["frames"].Number;
				var variations = table["types"].VerifyType(Value.ValueType.Array, location).Array;
				float damage = 1f;
				Value temp;
				if (table.TryGetValue("damage", out temp))
				{
					damage = (float)temp.Number;
				}
				bool fixedAngle = false;
				if (table.TryGetValue("fixedAngle", out temp))
				{
					fixedAngle = temp.Number != 0.0;
				}
				BlendState blend = BlendState.AlphaBlend;
				if (table.TryGetValue("blend", out temp))
				{
					blend = (BlendState)temp.VerifyType<BlendState>(location).Object;
				}
				foreach (var variation in variations)
				{
					var animation = new Animation(tex, speed, x, y, width, height, frames);
					var def = new BulletDefinition(animation, hitbox, blend, variation.String, damage, fixedAngle);
					defs[variation] = Value.Create(def);
					x += width * frames;
				}
				return defs;
			}

			public static Value CreateBullet(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				return Value.Create(new Bullet(new Vector2((float)args[0].Number, (float)args[1].Number), (float)args[2].Number, (float)args[3].Number, args[4].VerifyType<BulletDefinition>(location).Object as BulletDefinition, null));
			}

			public static Value DestroyInCircle(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var x = (float)args[0].VerifyType(Value.ValueType.Number,location).Number;
				var y = (float)args[1].VerifyType(Value.ValueType.Number,location).Number;
				var radius = (float)args[2].VerifyType(Value.ValueType.Number,location).Number;
				radius *= radius;
				foreach (var bullet in Layer.Bullets.Entities)
				{
					if (!bullet.CanDestroy)
						continue;
					var diffX = bullet.Position.X - x;
					var diffY = bullet.Position.Y - y;
					if (diffX * diffX + diffY * diffY <= radius)
						bullet.Destroy();
				}
				return Value.Nil;
			}

			public static Value GetBulletType(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var tobj = interpreter.DynamicLocalConstants["this"].Object;
				if (tobj is Bullet)
				{
					var obj = (Bullet)tobj;
					return obj.Definition.Type;
				}
				return "";
			}
		}


		public static class LibPatterns
		{
			public static void Register()
			{
                Script.Context.SetGlobal("pattern", new Table());
                Script.Context.SetGlobal("pattern.create", Script.Context.CreateFunction(Create, 0));
				Script.Context.AddMethod(typeof(Pattern), "fire", Script.Context.CreateFunction(Fire, 1));
				Script.Context.AddMethod(typeof(Pattern), "setPosition", Script.Context.CreateFunction(SetPosition, 3));
				Script.Context.AddMethod(typeof(Pattern), "setBullet", Script.Context.CreateFunction(SetBullet, 2));
				Script.Context.AddMethod(typeof(Pattern), "setCount", Script.Context.CreateFunction(SetCount, 2));
				Script.Context.AddMethod(typeof(Pattern), "setSpeed", Script.Context.CreateFunction(SetSpeed, 2));
				Script.Context.AddMethod(typeof(Pattern), "setAngle", Script.Context.CreateFunction(SetAngle, 2));
				Script.Context.AddMethod(typeof(Pattern), "setAngleOffset", Script.Context.CreateFunction(SetAngleOffset, 2));
				Script.Context.AddMethod(typeof(Pattern), "setRadius", Script.Context.CreateFunction(SetRadius, 2));
				Script.Context.AddMethod(typeof(Pattern), "setTask", Script.Context.CreateFunction(SetTask, 2));
			}

			public static Value Create(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var tobj = interpreter.DynamicLocalConstants["this"].Object;
				var pat = new Pattern(interpreter.DynamicLocalConstants["this"].Object as Entity);
				if (tobj is Player)
					pat.PlayerTeam = true;
				return Value.Create(pat);
			}

			public static Value Fire(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				(args[0].VerifyType<Pattern>(location).Object as Pattern).Fire();

				return Value.Nil;
			}

			public static Value SetPosition(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var pattern = (args[0].VerifyType<Pattern>(location).Object as Pattern);
				pattern.Position = new Vector2(
					(float)args[1].VerifyType(Value.ValueType.Number, location).Number,
					(float)args[2].VerifyType(Value.ValueType.Number, location).Number
				);

				return Value.Nil;
			}

			public static Value SetBullet(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var pattern = (args[0].VerifyType<Pattern>(location).Object as Pattern);
				pattern.BulletDefinition = args[1].VerifyType<BulletDefinition>(location).Object as BulletDefinition;

				return Value.Nil;
			}

			public static Value SetCount(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var pattern = (args[0].VerifyType<Pattern>(location).Object as Pattern);
				pattern.Count1 = (int)args[1].VerifyType(Value.ValueType.Number, location).Number;
				if (argCount > 2)
					pattern.Count2 = (int)args[2].VerifyType(Value.ValueType.Number, location).Number;
				else
					pattern.Count2 = 1;

				return Value.Nil;
			}

			public static Value SetSpeed(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var pattern = (args[0].VerifyType<Pattern>(location).Object as Pattern);
				pattern.Speed1 = (float)args[1].VerifyType(Value.ValueType.Number, location).Number;
				if (argCount > 2)
					pattern.Speed2 = (float)args[2].VerifyType(Value.ValueType.Number, location).Number;
				else
					pattern.Speed2 = pattern.Speed1 / 3f;

				return Value.Nil;
			}

			public static Value SetAngle(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var pattern = (args[0].VerifyType<Pattern>(location).Object as Pattern);
				pattern.Angle1 = (float)args[1].VerifyType(Value.ValueType.Number, location).Number;
				if (argCount > 2)
					pattern.Angle2 = (float)args[2].VerifyType(Value.ValueType.Number, location).Number;
				else
					pattern.Angle2 = 360f;

				return Value.Nil;
			}

			public static Value SetAngleOffset(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var pattern = (args[0].VerifyType<Pattern>(location).Object as Pattern);
				pattern.Angle3 = (float)args[1].VerifyType(Value.ValueType.Number, location).Number;

				return Value.Nil;
			}

			public static Value SetRadius(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var pattern = (args[0].VerifyType<Pattern>(location).Object as Pattern);
				pattern.Radius = (float)args[1].VerifyType(Value.ValueType.Number, location).Number;

				return Value.Nil;
			}

			public static Value SetTask(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var pattern = (args[0].VerifyType<Pattern>(location).Object as Pattern);
				pattern.Function = args[1].VerifyType(Value.ValueType.Function, location).Function;

				return Value.Nil;
			}
		}

		public static class LibAnimations
		{
			public static void Register()
			{
                Script.Context.SetGlobal("animation", new Table());
                Script.Context.SetGlobal("animation.create", Script.Context.CreateFunction(Create, 7));
				Script.Context.SetGlobal("animation.createMirrored", Script.Context.CreateFunction(CreateMirrored, 7));
			}

			public static Value Create(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var count = (int)args[6].VerifyType(Value.ValueType.Number, location).Number;
				if (count <= 0)
					throw new InterpreterException("Animation must have at least 1 frame", location);
				return Value.Create(new Animation(
					args[0].VerifyType(Value.ValueType.String, location).String,
					args[1].VerifyType(Value.ValueType.Number, location).Number,
					(int)args[2].VerifyType(Value.ValueType.Number, location).Number,
					(int)args[3].VerifyType(Value.ValueType.Number, location).Number,
					(int)args[4].VerifyType(Value.ValueType.Number, location).Number,
					(int)args[5].VerifyType(Value.ValueType.Number, location).Number,
					count
				));
			}

			public static Value CreateMirrored(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var count = (int)args[6].VerifyType(Value.ValueType.Number, location).Number;
				if (count <= 0)
					throw new InterpreterException("Animation must have at least 1 frame", location);
				return Value.Create(new Animation(
					args[0].VerifyType(Value.ValueType.String, location).String,
					args[1].VerifyType(Value.ValueType.Number, location).Number,
					(int)args[2].VerifyType(Value.ValueType.Number, location).Number,
					(int)args[3].VerifyType(Value.ValueType.Number, location).Number,
					(int)args[4].VerifyType(Value.ValueType.Number, location).Number,
					(int)args[5].VerifyType(Value.ValueType.Number, location).Number,
					count,
					true
				));
			}
		}

		public static class LibPlayer
		{
			public static void Register()
			{
				Script.Context.SetGlobal("player.setStartPosition", Script.Context.CreateFunction(SetStartPosition, 2));
				Script.Context.SetGlobal("player.setSpeed", Script.Context.CreateFunction(SetSpeed, 2));
				Script.Context.SetGlobal("player.setHitbox", Script.Context.CreateFunction(SetHitbox, 1));
				Script.Context.SetGlobal("player.setTurnTime", Script.Context.CreateFunction(SetTurnTime, 1));
				Script.Context.SetGlobal("player.setReviveTime", Script.Context.CreateFunction(SetReviveTime, 1));
				Script.Context.SetGlobal("player.setAnimationTurnLeft", Script.Context.CreateFunction(SetAnimationTurnLeft, 1));
				Script.Context.SetGlobal("player.setAnimationTurnRight", Script.Context.CreateFunction(SetAnimationTurnRight, 1));
				Script.Context.SetGlobal("player.setAnimationLeft", Script.Context.CreateFunction(SetAnimationLeft, 1));
				Script.Context.SetGlobal("player.setAnimationRight", Script.Context.CreateFunction(SetAnimationRight, 1));
				Script.Context.SetGlobal("player.setAnimationIdle", Script.Context.CreateFunction(SetAnimationIdle, 1));
			}

			public static Value SetStartPosition(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				Player.StartPosition = new Vector2(
					(float)args[0].VerifyType(Value.ValueType.Number, location).Number,
					(float)args[1].VerifyType(Value.ValueType.Number, location).Number
				);
				return Value.Nil;
			}

			public static Value SetSpeed(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				Player.MoveSpeed = (float)args[0].VerifyType(Value.ValueType.Number, location).Number;
				Player.MoveSpeedSlow = (float)args[1].VerifyType(Value.ValueType.Number, location).Number;
				return Value.Nil;
			}

			public static Value SetHitbox(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				Player.HitboxRadiusPlayer = (float)args[0].VerifyType(Value.ValueType.Number, location).Number;
				return Value.Nil;
			}

			public static Value SetTurnTime(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				Player.TurnTimePlayer = args[0].VerifyType(Value.ValueType.Number, location).Number;
				return Value.Nil;
			}

			public static Value SetReviveTime(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				Player.ReviveTime = args[0].VerifyType(Value.ValueType.Number, location).Number;
				return Value.Nil;
			}

			public static Value SetAnimationTurnLeft(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				Player.AnimationTurnLeftPlayer = args[0].VerifyType<Animation>(location).Object as Animation;
				return Value.Nil;
			}

			public static Value SetAnimationTurnRight(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				Player.AnimationTurnRightPlayer = args[0].VerifyType<Animation>(location).Object as Animation;
				return Value.Nil;
			}

			public static Value SetAnimationLeft(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				Player.AnimationLeftPlayer = args[0].VerifyType<Animation>(location).Object as Animation;
				return Value.Nil;
			}

			public static Value SetAnimationRight(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				Player.AnimationRightPlayer = args[0].VerifyType<Animation>(location).Object as Animation;
				return Value.Nil;
			}

			public static Value SetAnimationIdle(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				Player.AnimationIdlePlayer = args[0].VerifyType<Animation>(location).Object as Animation;
				return Value.Nil;
			}
		}

		public static class LibEnemies
		{
			public static void Register()
			{
				Script.Context.SetGlobal("enemy.create", Script.Context.CreateFunction(Create, 3));
				Script.Context.SetGlobal("setAnimationTurnLeft", Script.Context.CreateFunction(SetAnimationTurnLeft, 1));
				Script.Context.SetGlobal("setAnimationTurnRight", Script.Context.CreateFunction(SetAnimationTurnRight, 1));
				Script.Context.SetGlobal("setAnimationLeft", Script.Context.CreateFunction(SetAnimationLeft, 1));
				Script.Context.SetGlobal("setAnimationRight", Script.Context.CreateFunction(SetAnimationRight, 1));
				Script.Context.SetGlobal("setAnimationIdle", Script.Context.CreateFunction(SetAnimationIdle, 1));
				Script.Context.SetGlobal("setLife", Script.Context.CreateFunction(SetLife, 1));
				Script.Context.SetGlobal("setBoss", Script.Context.CreateFunction(SetBoss, 1));
			}

			public static Value Create(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				return Value.Create(new Enemy(
					args[0].VerifyType(Value.ValueType.Function, location).Function,
					new Vector2(
						(float)args[1].VerifyType(Value.ValueType.Number, location).Number,
						(float)args[2].VerifyType(Value.ValueType.Number, location).Number
					)
				));
			}

			public static Value SetAnimationIdle(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var tobj = interpreter.DynamicLocalConstants["this"].Object;
				if (tobj is LeftRightAnimatedEntity)
				{
					var obj = (LeftRightAnimatedEntity)tobj;
					obj.AnimationIdle = (Animation)args[0].VerifyType<Animation>(location).Object;
				}
				return Value.Nil;
			}

			public static Value SetAnimationLeft(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var tobj = interpreter.DynamicLocalConstants["this"].Object;
				if (tobj is LeftRightAnimatedEntity)
				{
					var obj = (LeftRightAnimatedEntity)tobj;
					obj.AnimationLeft = (Animation)args[0].VerifyType<Animation>(location).Object;
				}
				return Value.Nil;
			}

			public static Value SetAnimationRight(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var tobj = interpreter.DynamicLocalConstants["this"].Object;
				if (tobj is LeftRightAnimatedEntity)
				{
					var obj = (LeftRightAnimatedEntity)tobj;
					obj.AnimationRight = (Animation)args[0].VerifyType<Animation>(location).Object;
				}
				return Value.Nil;
			}

			public static Value SetAnimationTurnLeft(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var tobj = interpreter.DynamicLocalConstants["this"].Object;
				if (tobj is LeftRightAnimatedEntity)
				{
					var obj = (LeftRightAnimatedEntity)tobj;
					obj.AnimationTurnLeft = (Animation)args[0].VerifyType<Animation>(location).Object;
				}
				return Value.Nil;
			}

			public static Value SetAnimationTurnRight(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var tobj = interpreter.DynamicLocalConstants["this"].Object;
				if (tobj is LeftRightAnimatedEntity)
				{
					var obj = (LeftRightAnimatedEntity)tobj;
					obj.AnimationTurnRight = (Animation)args[0].VerifyType<Animation>(location).Object;
				}
				return Value.Nil;
			}

			public static Value SetLife(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var tobj = interpreter.DynamicLocalConstants["this"].Object;
				if (tobj is Enemy)
				{
					var obj = (Enemy)tobj;
					obj.Life = args[0].VerifyType(Value.ValueType.Number, location).Number;
					if (argCount > 1)
						obj.MaxLife = args[1].VerifyType(Value.ValueType.Number, location).Number;
					else
						obj.MaxLife = obj.Life;
				}
				return Value.Nil;
			}

			public static Value SetBoss(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				var tobj = interpreter.DynamicLocalConstants["this"].Object;
				if (tobj is Enemy)
				{
					var obj = (Enemy)tobj;
					obj.IsBoss = args[0].VerifyType(Value.ValueType.Number, location).Bool;
				}
				return Value.Nil;
			}
		}

		public static class LibBosses
		{
			public static void Register()
			{
				Script.Context.SetGlobal("boss.setMarkerAnimation", Script.Context.CreateFunction(SetMarkerAnimation, 1));
				Script.Context.SetGlobal("boss.setHealthBarAnimations", Script.Context.CreateFunction(SetHealthbarAnimations, 2));
			}

			public static Value SetMarkerAnimation(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				Enemy.BossMarker = (Animation)args[0].VerifyType<Animation>(location).Object;
				return Value.Nil;
			}

			public static Value SetHealthbarAnimations(Interpreter interpreter, SourceRef location, Value[] args, int argCount)
			{
				Enemy.HealthBarBack = (Animation)args[0].VerifyType<Animation>(location).Object;
				Enemy.HealthBarFront = (Animation)args[1].VerifyType<Animation>(location).Object;
				return Value.Nil;
			}
		}
	}
}

