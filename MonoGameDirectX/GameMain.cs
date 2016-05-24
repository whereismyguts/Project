using System;
using System.Linq;
using Core.Objects;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDirectX {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameMain: Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        Renderer renderer;

        int ScreenHeight { get { return renderer.ScreenHeight; } }
        int ScreenWidth { get { return renderer.ScreenWidth; } }
        GameState State {
            get { return MainCore.State; }
            set { MainCore.State = value; }
        }

        public InteractionController Controller { get { return MainCore.Instance.Controller; } }

        public GameMain() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        void AddControl(Control control, GameState state) {
            Controller.Add(control as InteractiveObject, state);
        }
        void InitializeUI() {  
            AddControl(new Label(ScreenWidth / 2 - 100, 50, 200, 30, "main title"), GameState.MainMenu);

            AddControl(new Button(ScreenWidth / 2 - 75, 180, 75, 20, "test"), GameState.MainMenu);

            Button start = new Button(ScreenWidth / 2 - 100, 200, 200, 30, "start");
            start.ButtonClick += SwitchState;
            AddControl(start, GameState.MainMenu);

            Button gotomenu = new Button(10, 10, 100, 50, "menu");
            gotomenu.ButtonClick += SwitchState;
            AddControl(gotomenu, GameState.Space);
        }
        void ProcessInput() {
            if(Keyboard.GetState().IsKeyDown(Keys.Z))
                MainCore.Instance.Viewport.ZoomIn();
            if(Keyboard.GetState().IsKeyDown(Keys.X))
                MainCore.Instance.Viewport.ZoomOut();
            if(Keyboard.GetState().IsKeyDown(Keys.Up))
                MainCore.Instance.Ships[0].AccselerateEngine();

            if(Keyboard.GetState().IsKeyDown(Keys.Left))
                MainCore.Instance.Ships[0].RotateL();
            if(Keyboard.GetState().IsKeyDown(Keys.Right))
                MainCore.Instance.Ships[0].RotateR();

            //if(Keyboard.GetState().IsKeyDown(Keys.Up))
            //    Core.Instance.Viewport.Centerpoint += new CoordPoint(0, -10);
            //if(Keyboard.GetState().IsKeyDown(Keys.Down))
            //    Core.Instance.Viewport.Centerpoint += new CoordPoint(0, 10);
            //if(Keyboard.GetState().IsKeyDown(Keys.Right))
            //    Core.Instance.Viewport.Centerpoint += new CoordPoint(10, 0);
            //if(Keyboard.GetState().IsKeyDown(Keys.Left))
            //    Core.Instance.Viewport.Centerpoint += new CoordPoint(-10, 0);
        }
        void SwitchState(object sender, EventArgs e) {
            State = State == GameState.MainMenu ? GameState.Space : GameState.MainMenu;
        }

        protected override void Draw(GameTime gameTime) {
            renderer.Render(gameTime);
            base.Draw(gameTime);
        }
        protected override void Initialize() {
            renderer = new Renderer(GraphicsDevice);
            MainCore.Instance.Viewport.SetViewportSize(ScreenWidth, ScreenHeight);
            InitializeUI();
            base.Initialize();
        }
        protected override void LoadContent() {
            WinAdapter.LoadContent(Content, GraphicsDevice);
            renderer.Font = Content.Load<SpriteFont>("Arial");

        }
        protected override void UnloadContent() {
            WinAdapter.Unload();
            Content.Unload();
            // TODO: Unload any non ContentManager content here
        }
        protected override void Update(GameTime gameTime) {
            Controller.HitTest(Mouse.GetState().LeftButton == ButtonState.Pressed, Mouse.GetState().Position);
            ProcessInput();
            MainCore.Instance.Update();
            base.Update(gameTime);
        }
    }
}
