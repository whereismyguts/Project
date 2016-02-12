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
    public class GameMain : Microsoft.Xna.Framework.Game {
        ContentLoader contentLoader;
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

            foreach (RenderObject obj in GameCore.Instance.RenderObjects) {
                Texture2D texture = contentLoader.GetTexture(obj.ContentString);
                Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
                //Rectangle boundsRect = WinAdapter.ToRectangle(obj.ScreenBounds);
                //var textureRect = new Rectangle(boundsRect.Location + new Point(boundsRect.Width / 2, boundsRect.Height / 2), boundsRect.Size);
                //   spriteBatch.Draw(texture, textureRect, null, Color.White, obj.Rotation, origin, SpriteEffects.None, 0);
                Vector2 location = WinAdapter.ToVector2(obj.ScreenBounds);
                spriteBatch.Draw(texture, location, null, Color.Red, 1f, origin, GameCore.Instance.Viewport.Scale, SpriteEffects.None, 0f);
            }

            primitiveDrawer.DrawLine(
                WinAdapter.ToRectangle(GameCore.Instance.Ship.GetScreenBounds()).Center.ToVector2(),
                (WinAdapter.ToRectangle(GameCore.Instance.Ship.GetScreenBounds() + GameCore.Instance.Ship.direction * 50).Center.ToVector2()),
                spriteBatch);

            spriteBatch.DrawString(font, GameCore.Instance.Ship.direction.ToString(), new Vector2(0, 0), Color.Red);
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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            contentLoader = new ContentLoader(Content);
            contentLoader.SetTexture("planet1");
            contentLoader.SetTexture("planet2");
            contentLoader.SetTexture("planet3");
            contentLoader.SetTexture("planet4");
            contentLoader.SetTexture("planet5");
            contentLoader.SetTexture("ship1");

            font = Content.Load<SpriteFont>("Arial");
        }
        //protected override void UnloadContent() {
        //    // TODO: Unload any non ContentManager content here
        //}
        protected override void Update(GameTime gameTime) {
            GameCore.Instance.Update();
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
                GameCore.Instance.Viewport.ZoomIn();
            if (Keyboard.GetState().IsKeyDown(Keys.X))
                GameCore.Instance.Viewport.ZoomOut();
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                GameCore.Instance.Ship.AccselerateF();
            else
                GameCore.Instance.Ship.Stop();
            //if(Keyboard.GetState().IsKeyDown(Keys.Down))
            //    GameCore.Instance.Ship.AccselerateB();
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                GameCore.Instance.Ship.RotateL();
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                GameCore.Instance.Ship.RotateR();


            mousePosition = Mouse.GetState().Position;
            base.Update(gameTime);
        }
    }
}
