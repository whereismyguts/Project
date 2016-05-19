using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace MonoGameDirectX {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameMain: Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        Renderer renderer;

        public GameMain() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Draw(GameTime gameTime) {
            renderer.Render();
            base.Draw(gameTime);
        }
        protected override void Initialize() {
            renderer = new Renderer(GraphicsDevice);
            MainCore.Instance.Viewport.SetViewportSize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            
            base.Initialize();
        }
        protected override void LoadContent() {
            WinAdapter.LoadContent(Content, GraphicsDevice);
            renderer.Font = Content.Load<SpriteFont>("Arial");
        }
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }
        protected override void Update(GameTime gameTime) {
            MainCore.Instance.Update();
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
            
            base.Update(gameTime);
        }
    }
}
