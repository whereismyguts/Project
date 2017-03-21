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
        GameState State {
            get { return Instance.State; }
            set { Instance.State = value; }
        }
        GameCore.Viewport Viewport { get { return Instance.Viewport; } }
        InteractionController Controller { get { return Instance.Controller; } }

        public MainCore Instance { get { return MainCore.Instance; } }

        public GameMain() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        void AddControl(Control control, GameState state) {
            Controller.Add(control as InteractiveObject, state);
        }

        void InitializeUI() {
            // mainmenu
            //   label = new Label(ScreenWidth / 2 - 100, 50, 200, 30, "main title", renderer.Font);
            //    AddControl(label, GameState.MainMenu);

            //  AddControl(new Button(ScreenWidth / 2 - 75, 180, 75, 20, "test", renderer.Font), GameState.MainMenu);

            Button startButton = new Button(ScreenWidth / 2 - 100, 200, 200, 30, "start");
            startButton.ButtonClick += StartBattle;
            AddControl(startButton, GameState.MainMenu);
            AddControl(new TextBox(ScreenWidth / 2 - 100, 300, 200, 30, "input"), GameState.MainMenu);


            //inventoryListBox = new ListBox(new Point(100, 300), "");

            //AddControl(inventoryListBox, GameState.MainMenu);

            imageBox = new ImageBox(new Rectangle(400, 250, 200, 200));
            AddControl(imageBox, GameState.MainMenu);
        }




        ImageBox imageBox;


        void ProcessInput() {
            var mouseState = Mouse.GetState();
            var keyState = Keyboard.GetState();
            var pressedKeys = keyState.GetPressedKeys();
            Controller.HitTest(mouseState.LeftButton == ButtonState.Pressed, mouseState.Position, pressedKeys.Length > 0 ? (int)(pressedKeys[0]) : -1);
            Controller.SetPressedKeys(keyState.GetPressedKeys().Cast<int>());

            //if(keyState.IsKeyDown(Keys.Q))
            //    Exit();
            //if(keyState.IsKeyDown(Keys.Z))
            //    Viewport.ZoomIn();
            //if(keyState.IsKeyDown(Keys.X))
            //    Viewport.ZoomOut();
            //if(keyState.IsKeyDown(Keys.Up))
            //    Player.Accselerate();

            //if(mouseState.ScrollWheelValue - mouseWheel < 0) {
            //    Viewport.ZoomIn();
            //}
            //if(mouseState.ScrollWheelValue - mouseWheel > 0) {
            //    Viewport.ZoomOut();
            //}
            //mouseWheel = mouseState.ScrollWheelValue;
            //if(keyState.IsKeyDown(Keys.Right))
            //    Player.RotateR();
            //if(keyState.IsKeyDown(Keys.Left))
            //    Player.RotateL();

            Debugger.Text = mouseState.ScrollWheelValue.ToString() + " " + Viewport.Scale;
            MainCore.Cursor = Viewport.Screen2WorldPoint(new CoordPoint(mouseState.X, mouseState.Y));
            if(mouseState.LeftButton == ButtonState.Pressed)
                MainCore.Pressed(new CoordPoint(mouseState.X, mouseState.Y));

            if(Controller.KeysUp.Count>0)
            if(Controller.KeysUp.Contains(70) ) {
                graphics.IsFullScreen = !graphics.IsFullScreen;

                    if(!graphics.IsFullScreen) {
                        width = graphics.PreferredBackBufferWidth;
                        height = graphics.PreferredBackBufferHeight;
                    }


                    graphics.PreferredBackBufferWidth = !graphics.IsFullScreen ? GraphicsDevice.DisplayMode.Width : width;
                    graphics.PreferredBackBufferHeight = !graphics.IsFullScreen ? GraphicsDevice.DisplayMode.Height : height;

                    graphics.ApplyChanges();


                    Viewport.SetViewportSize(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                }

        }

        int width =0;
        int height = 0;
        //int mouseWheel = 0;
        void StartBattle(object sender, EventArgs e) {
            State = GameState.Space;
        }

        protected override void Draw(GameTime gameTime) {

         //   System.Threading.Thread.Sleep(1000);
          //  if(Controller.Keys.ToList().Contains(32))
            Renderer.Render(gameTime);
            base.Draw(gameTime);
        }
        protected override void Initialize() {
            Renderer.Set(GraphicsDevice,Content.Load<SpriteFont>("Arial"));
            //Renderer.SpriteBatch = new SpriteBatch(GraphicsDevice);

            // full screen mode:
            //graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            //graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            //graphics.IsFullScreen = true;
            //graphics.ApplyChanges();

            Viewport.SetViewportSize(ScreenWidth, ScreenHeight);
            InitializeUI();

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
