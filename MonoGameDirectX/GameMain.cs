using System;
using System.Linq;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;

namespace MonoGameDirectX {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameMain: Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        int ScreenHeight { get { return Renderer.ScreenHeight; } }
        int ScreenWidth { get { return Renderer.ScreenWidth; } }
        UIState State {
            get { return Instance.State; }
            set { Instance.State = value; }
        }
        GameCore.Viewport Viewport { get { return Instance.Viewport; } }
        //InteractionController Controller { get { return Instance.Controller; } }

        public MainCore Instance { get { return MainCore.Instance; } }

        public GameMain() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        void ProcessInput() {
            var mouseState = Mouse.GetState();
            var keyState = Keyboard.GetState();
            var pressedKeys = keyState.GetPressedKeys();
            InteractionController.SetPressedKeys(keyState.GetPressedKeys().Cast<int>());

            Debugger.Text = mouseState.ScrollWheelValue.ToString() + " " + Viewport.Scale;
            MainCore.Cursor = Viewport.Screen2WorldPoint(new CoordPoint(mouseState.X, mouseState.Y));

            //if(Controller.KeysUp.Count>0)
            //if(Controller.KeysUp.Contains(70) ) {
            //    graphics.IsFullScreen = !graphics.IsFullScreen;

            //        if(!graphics.IsFullScreen) {
            //            width = graphics.PreferredBackBufferWidth;
            //            height = graphics.PreferredBackBufferHeight;
            //        }

            //        graphics.PreferredBackBufferWidth = !graphics.IsFullScreen ? GraphicsDevice.DisplayMode.Width : width;
            //        graphics.PreferredBackBufferHeight = !graphics.IsFullScreen ? GraphicsDevice.DisplayMode.Height : height;

            //        graphics.ApplyChanges();

            //        Viewport.SetViewportSize(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            //    }

            InterfaceController.DoActions();
        }

        int width = 0;
        int height = 0;
        //int mouseWheel = 0;

        protected override void Draw(GameTime gameTime) {

            //   System.Threading.Thread.Sleep(1000);
            //  if(Controller.Keys.ToList().Contains(32))
            Renderer.Render(gameTime);
            base.Draw(gameTime);
        }
        protected override void Initialize() {
            Renderer.Set(GraphicsDevice, Content.Load<SpriteFont>("Arial"));
            //Renderer.SpriteBatch = new SpriteBatch(GraphicsDevice);

            // full screen mode:
            //graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            //graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            //graphics.IsFullScreen = true;
            //graphics.ApplyChanges();

            Viewport.SetViewportSize(ScreenWidth, ScreenHeight);

            base.Initialize();
        }

        protected override void LoadContent() {
            WinAdapter.LoadContent(Content, GraphicsDevice);
        }
        protected override void UnloadContent() {
            WinAdapter.Unload();
            Content.Unload();
            // TODO: Unload any non ContentManager content here
        }
        protected override void Update(GameTime gameTime) {
            if(IsActive)
                ProcessInput();
            Instance.Update();
            //    State = GameState.Space;
            //renderer.TraectoryPath = Player.Calculator.Calculate();
            base.Update(gameTime);
        }
    }
}
