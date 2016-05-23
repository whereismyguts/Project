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
    public class GameMain: Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        Renderer renderer;
        public InteractionController Controller { get; } = new InteractionController();
        List<Control> controls = new List<Control>();
        public GameMain() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


      

        protected override void Draw(GameTime gameTime) {


            switch(MainCore.State) {
                case (StateEnum.MainMenu):
                renderer.RenderMenu(controls);
                    break;
                case (StateEnum.Space):
                    renderer.Render(gameTime);
                    break;
            }

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
            AddControl(new Label(100, 100, 200, 30, "main title"));
            Button start = new Button(100, 200, 75, 20, "start");
            start.Click += Start_Click;
            AddControl(start);
            AddControl(new Button(100,180,75,20,"test"));
        }

        private void Start_Click(object sender, EventArgs e) {
            MainCore.State = MainCore.State == StateEnum.MainMenu ? StateEnum.Space : StateEnum.MainMenu;
        }

        void AddControl(Control control) {
            Controller.Add(control as InteractiveObject);
            controls.Add(control);
        }

        protected override void LoadContent() {
            WinAdapter.LoadContent(Content, GraphicsDevice);
            renderer.Font = Content.Load<SpriteFont>("Arial");
            
        }
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }
        protected override void Update(GameTime gameTime) {
            

            Controller.HitTest(Mouse.GetState());


            ProcessInput();
            if(MainCore.State == StateEnum.Space)
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
