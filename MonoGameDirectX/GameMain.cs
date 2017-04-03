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
            get { return Instance.CurrentState; }
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

            //Debugger.Text = mouseState.ScrollWheelValue.ToString() + " " + Viewport.Scale;
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

            InterfaceController.AddState(new MenuState(), new GameState()); // order is matters

            var b1 = new Button(100, 50, 100, 40, "b1");
            var b2 = new Button(100, 100, 100, 40, "b2");
            var b3 = new Button(100, 150, 100, 40, "b3");
            var b4 = new Button(100, 200, 100, 40, "b4");

            b1.ButtonClick += ButtonClicked;
            b2.ButtonClick += ButtonClicked;
            b3.ButtonClick += ButtonClicked;
            b4.ButtonClick += ButtonClicked;

            InterfaceController.AddControl(0, b1);
            InterfaceController.AddControl(0, b2);
            InterfaceController.AddControl(0, b3);
            InterfaceController.AddControl(0, b4);

            InterfaceController.AddKeyBinding(Keys.Up, 1, PlayerAction.Up);
            InterfaceController.AddKeyBinding(Keys.Down, 1, PlayerAction.Down);
            InterfaceController.AddKeyBinding(Keys.Left, 1, PlayerAction.Left);
            InterfaceController.AddKeyBinding(Keys.Right, 1, PlayerAction.Right);
            InterfaceController.AddKeyBinding(Keys.RightControl, 1, PlayerAction.Yes);
            InterfaceController.AddKeyBinding(Keys.Tab, 1, PlayerAction.Tab);

            InterfaceController.AddKeyBinding(Keys.W,2, PlayerAction.Up);
            InterfaceController.AddKeyBinding(Keys.S, 2, PlayerAction.Down);
            InterfaceController.AddKeyBinding(Keys.A, 2, PlayerAction.Left);
            InterfaceController.AddKeyBinding(Keys.D, 2, PlayerAction.Right);
            InterfaceController.AddKeyBinding(Keys.M, 2, PlayerAction.Yes);

            base.Initialize();
        }

        private void ButtonClicked(object sender, EventArgs e) {
            Debugger.Text += "," + (sender as Button).Text;
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
