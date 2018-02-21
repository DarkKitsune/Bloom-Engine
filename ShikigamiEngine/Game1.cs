using System;
using DrakeScript;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ShikigamiEngine
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager GraphicsDeviceManager;
		SpriteBatch SpriteBatch;

		public Game1()
		{
			GraphicsDeviceManager = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
            
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		TaskObject MainTask;
		protected override void LoadContent()
		{
			GraphicsDeviceManager.PreferredBackBufferWidth = 640;
			GraphicsDeviceManager.PreferredBackBufferHeight = 480;
			GraphicsDeviceManager.PreferMultiSampling = false;
			GraphicsDeviceManager.ApplyChanges();

			Resources.ContentManager = Content;

			Engine.Init();
			Font.Init();
			Script.Init();

			Libs.Register();

			SpriteBatch = new SpriteBatch(GraphicsDevice);
			Graphics.GraphicsDeviceManager = GraphicsDeviceManager;
			Graphics.GraphicsDevice = GraphicsDevice;
			Graphics.SpriteBatch = SpriteBatch;
			Graphics.BasicEffect = new BasicEffect(GraphicsDevice);
				Graphics.BasicEffect.TextureEnabled = true;
				Graphics.BasicEffect.LightingEnabled = false;
				Graphics.BasicEffect.VertexColorEnabled = false;
			Graphics.RasterizerStatePrimitive = new RasterizerState();
				Graphics.RasterizerStatePrimitive.CullMode = CullMode.None;
				Graphics.RasterizerStatePrimitive.DepthClipEnable = false;
				Graphics.RasterizerStatePrimitive.ScissorTestEnable = false;
			Graphics.RasterizerStateSprite = new RasterizerState();
				Graphics.RasterizerStateSprite.CullMode = CullMode.None;
				Graphics.RasterizerStateSprite.DepthClipEnable = false;
				Graphics.RasterizerStateSprite.ScissorTestEnable = false;
			Graphics.DepthStencilStatePrimitive = new DepthStencilState();
				Graphics.DepthStencilStatePrimitive.DepthBufferEnable = false;
				Graphics.DepthStencilStatePrimitive.DepthBufferWriteEnable = false;
				Graphics.DepthStencilStatePrimitive.StencilEnable = false;
			Graphics.DepthStencilStateSprite = new DepthStencilState();
				Graphics.DepthStencilStateSprite.DepthBufferEnable = false;
				Graphics.DepthStencilStateSprite.DepthBufferWriteEnable = false;
				Graphics.DepthStencilStateSprite.StencilEnable = false;
			Graphics.SamplerStatePrimitive = new SamplerState();
				Graphics.SamplerStatePrimitive.Filter = TextureFilter.Linear;
				Graphics.SamplerStatePrimitive.AddressU = TextureAddressMode.Wrap;
				Graphics.SamplerStatePrimitive.AddressV = TextureAddressMode.Wrap;
			Graphics.SamplerStateSprite = new SamplerState();
				Graphics.SamplerStateSprite.Filter = TextureFilter.Linear;
				Graphics.SamplerStateSprite.AddressU = TextureAddressMode.Wrap;
				Graphics.SamplerStateSprite.AddressV = TextureAddressMode.Wrap;

			var l = new System.Collections.Generic.List<string>();
			foreach (var p in Graphics.BasicEffect.Parameters)
			{
				l.Add(p.Name);
			}
			//Console.WriteLine(String.Join(", ", l));

			Primitive.CreateBasicPrimitives();
			UserPrimitive.UpdateCircularBar(false);

			Script.Context.DoFile("data/scripts/bulletdef.script");
			Script.Context.DoFile("data/scripts/bulletdefPlayer.script");
			MainTask = new TaskObject(Script.Context.LoadFile("data/scripts/init.script"));
			Stage.NextStage();

			new Player();

		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
			#if !__IOS__ &&  !__TVOS__
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
			#endif

			Input.Update();
            

			Script.ResumeTasks();

			Layer.UpdateAll();

			Engine.Update();

			Engine.EndUpdate();
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			Graphics.SetStates();

			GraphicsDeviceManager.GraphicsDevice.Clear(Color.Black);

			Layer.DrawAll();

			Engine.Draw();


			var infoPosition = new Vector2(
				Graphics.GraphicsDevice.Viewport.Width - 65,
				Graphics.GraphicsDevice.Viewport.Height - 40
			);
			Graphics.DrawTextShadowed(infoPosition, String.Format("BOSSLIFE {0}", Enemy.BossLife), Font.Small, Color.White);

			infoPosition = new Vector2(
				Graphics.GraphicsDevice.Viewport.Width - 60,
				Graphics.GraphicsDevice.Viewport.Height - 30
			);
			Graphics.DrawTextShadowed(infoPosition, String.Format("BULLET {0}", Engine.BulletCount), Font.Small, Color.White);

			infoPosition = new Vector2(
				Graphics.GraphicsDevice.Viewport.Width - 55,
				Graphics.GraphicsDevice.Viewport.Height - 20
			);
			Graphics.DrawTextShadowed(infoPosition, String.Format("UPS {0:#.##}", Engine.UPS), Font.Small, Color.White);

			infoPosition = new Vector2(
				Graphics.GraphicsDevice.Viewport.Width - 55,
				Graphics.GraphicsDevice.Viewport.Height - 10
			);
			Graphics.DrawTextShadowed(infoPosition, String.Format("FPS {0:#.##}", Engine.FPS), Font.Small, Color.White);

			base.Draw(gameTime);
		}
	}
}

