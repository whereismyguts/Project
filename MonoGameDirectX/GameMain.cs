using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonoGameDirectX {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameMain: Microsoft.Xna.Framework.Game {

        Texture2D dummyTexture;
        SpriteFont font;
        GraphicsDeviceManager graphics;
        Point mousePosition;
        DrawPrimitives primitiveDrawer;
        SpriteBatch spriteBatch;

        public GameMain() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            foreach(RenderObjectCore obj in GameCore.Instance.RenderObjects) {
                RenderObject renderObject = WinAdapter.CreateRenderObject(obj);
                spriteBatch.Draw(renderObject.Texture, renderObject.TextureRect, null, renderObject.ColorMask, renderObject.Rotation, renderObject.Origin, SpriteEffects.None, 0);
                primitiveDrawer.DrawCircle(renderObject.TextureRect.Location.ToVector2(), renderObject.TextureRect.Width/2, spriteBatch, Color.Red);
                primitiveDrawer.DrawCircle(new Vector2(GraphicsDevice.Viewport.Width-100, GraphicsDevice.Viewport.Height-100)+ WinAdapter.CoordPoint2Vector(renderObject.MiniMapBounds.Center) / 10,renderObject.MiniMapBounds.Width/10f, spriteBatch, Color.Yellow);
               
            }

            primitiveDrawer.DrawLine(
                WinAdapter.Bounds2Rectangle(GameCore.Instance.Ship.GetScreenBounds()).Center.ToVector2(),
                (WinAdapter.Bounds2Rectangle(GameCore.Instance.Ship.GetScreenBounds() + GameCore.Instance.Ship.Direction * 20).Center.ToVector2()),
                spriteBatch, Color.White);
            




            spriteBatch.DrawString(font, GameCore.Instance.Viewport.Scale.ToString(), new Vector2(0, 0), Color.White);
            spriteBatch.Draw(dummyTexture, new Rectangle(mousePosition.X, mousePosition.Y, 5, 5), Color.White);
            
            spriteBatch.End();
            //base.Draw(gameTime);
        }
        protected override void Initialize() {
            primitiveDrawer = new DrawPrimitives(GraphicsDevice);
            GameCore.Instance.Viewport.SetViewportSize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            base.Initialize();
        }
        protected override void LoadContent() {
            WinAdapter.LoadContent(Content);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            font = Content.Load<SpriteFont>("Arial");
        }
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }
        protected override void Update(GameTime gameTime) {
            GameCore.Instance.Update();
            if(Keyboard.GetState().IsKeyDown(Keys.Z))
                GameCore.Instance.Viewport.ZoomIn();
            if(Keyboard.GetState().IsKeyDown(Keys.X))
                GameCore.Instance.Viewport.ZoomOut();
            if(Keyboard.GetState().IsKeyDown(Keys.Up))
                GameCore.Instance.Ship.Accselerate();
            else
                GameCore.Instance.Ship.StopEngine();
            //if(Keyboard.GetState().IsKeyDown(Keys.Down))
            //    GameCore.Instance.Ship.AccselerateB();
            if(Keyboard.GetState().IsKeyDown(Keys.Left))
                GameCore.Instance.Ship.RotateL();
            if(Keyboard.GetState().IsKeyDown(Keys.Right))
                GameCore.Instance.Ship.RotateR();


            mousePosition = Mouse.GetState().Position;
            base.Update(gameTime);
        }
    }
}
