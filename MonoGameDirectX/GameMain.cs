using System;
using System.Linq;
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
        Ship Player { get { return Instance.Ships[0]; } }
        int ScreenHeight { get { return renderer.ScreenHeight; } }
        int ScreenWidth { get { return renderer.ScreenWidth; } }
        GameState State {
            get { return MainCore.State; }
            set { MainCore.State = value; }
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
        Label label;
        void InitializeUI() {
            label = new Label(ScreenWidth / 2 - 100, 50, 200, 30, "main title", renderer.Font);
            AddControl(label, GameState.MainMenu);

            AddControl(new Button(ScreenWidth / 2 - 75, 180, 75, 20, "test", renderer.Font), GameState.MainMenu);

            Button start = new Button(ScreenWidth / 2 - 100, 200, 200, 30, "start", renderer.Font);
            start.ButtonClick += SwitchState;
            AddControl(start, GameState.MainMenu);

            Button gotomenu = new Button(10, 10, 100, 50, "menu", renderer.Font);
            gotomenu.ButtonClick += SwitchState;
            AddControl(gotomenu, GameState.Space);
            ListBox lb = new ListBox(new Point(300, 300), renderer.Font, "text1", "long long text2", "text3");
            lb.ItemClick += Lb_ItemClick;
             AddControl(lb, GameState.MainMenu);

        }

        private void Lb_ItemClick(object sender, EventArgs e) {
            label.Text = (sender as Label).Text;
        }

        void ProcessInput() {
            if(Keyboard.GetState().IsKeyDown(Keys.Z))
                Viewport.ZoomIn();
            if(Keyboard.GetState().IsKeyDown(Keys.X))
                Viewport.ZoomOut();
            if(Keyboard.GetState().IsKeyDown(Keys.Up))
                Player.Accselerate();

            if(Keyboard.GetState().IsKeyDown(Keys.Left))
                Player.RotateL();
            if(Keyboard.GetState().IsKeyDown(Keys.Right))
                Player.RotateR();

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
            renderer = new Renderer(GraphicsDevice) { Font = Content.Load<SpriteFont>("Arial") };
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
            Controller.HitTest(Mouse.GetState().LeftButton == ButtonState.Pressed, Mouse.GetState().Position);
            ProcessInput();
            Instance.Update();
            base.Update(gameTime);
        }
    }
}
