using System;
using System.Linq;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MonoGameDirectX {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameMain : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        Renderer renderer;
        
        public GameMain() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }




        protected override void Draw(GameTime gameTime) {

            
                    renderer.Render(gameTime);
            base.Draw(gameTime);
        }
        protected override void Initialize() {
            renderer = new Renderer(GraphicsDevice);
            MainCore.Instance.Viewport.SetViewportSize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            InitializeUI();
            base.Initialize();
        }
        // TODO dictionary with game screens
        void InitializeUI() {
            Dictionary<GameState, List<InteractiveObject>> intrefaces = new Dictionary<GameState, List<InteractiveObject>>();
            AddControl(new Label(100, 100, 200, 30, "main title"), GameState.MainMenu);
            Button start = new Button(100, 200, 75, 20, "start");
            start.ButtonClick += SwitchState;
            AddControl(start, GameState.MainMenu);
            AddControl(new Button(100, 180, 75, 20, "test"), GameState.MainMenu);
            Button gotomenu = new Button(10, 10, 100, 50, "menu");
            gotomenu.ButtonClick += SwitchState;
            AddControl(gotomenu, GameState.Space);
        }

        private void SwitchState(object sender, EventArgs e) {
            State = State == GameState.MainMenu ? GameState.Space : GameState.MainMenu;
        }
        GameState State
        {
            get { return MainCore.State; }
            set
            {
                if(MainCore.State == value)
                    return;
                MainCore.State = value;
                Controller.UpdateState(MainCore.State);
            }
        }

        public InteractionController Controller { get { return MainCore.Instance.Controller; } }

        void AddControl(Control control, GameState state) {
            Controller.Add(control as InteractiveObject, state);

        }

        protected override void LoadContent() {
            WinAdapter.LoadContent(Content, GraphicsDevice);
            renderer.Font = Content.Load<SpriteFont>("Arial");

        }
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }
        protected override void Update(GameTime gameTime) {


            Controller.HitTest(Mouse.GetState().LeftButton == ButtonState.Pressed, Mouse.GetState().Position);


            ProcessInput();
            if(MainCore.State == GameState.Space)
                MainCore.Instance.Update();
            base.Update(gameTime);
        }

        private static void ProcessInput() {
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
    }
}
