using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Core;
using System;
using System.Collections.Generic;

namespace MonoGameDirectX {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameMain : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D dummyTexture;
        SpriteFont font;
        Point mousePosition;
        DrawPrimitives primitiveDrawer;
        ContentLoader contentLoader;

        public GameMain() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize() {
            primitiveDrawer = new DrawPrimitives(GraphicsDevice);
            base.Initialize();
        }
        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            contentLoader = new ContentLoader(Content);
            contentLoader.SetTexture("planet1");
            contentLoader.SetTexture("ship1");
            
            font = Content.Load<SpriteFont>("Arial");
        }
        //protected override void UnloadContent() {
        //    // TODO: Unload any non ContentManager content here
        //}

        protected override void Update(GameTime gameTime) {
            GameCore.Instance.Update();
            mousePosition = Mouse.GetState().Position;
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            
            foreach(RenderObject obj in GameCore.Instance.RenderObjects)
              {
                
                
                Texture2D texture = contentLoader.GetTexture(obj.ContentString);
                Vector2 origin = new Vector2(texture.Width/2, texture.Height/2);
                Rectangle boundsRect = WinAdapter.ToRectangle(obj.ScreenBounds);
                Rectangle textureRect = new Rectangle(boundsRect.Location + new Point(boundsRect.Width / 2, boundsRect.Height / 2), boundsRect.Size);
                //spriteBatch.Draw(texture, rect, null, Color.White, obj.Rotation, null, SpriteEffects.None, 0f);
                spriteBatch.Draw(texture, textureRect, null, Color.White, obj.Rotation, origin, SpriteEffects.None, 0);
               // primitiveDrawer.DrawRect(boundsRect, spriteBatch);
            }

            spriteBatch.DrawString(font, GameCore.Instance.Viewport.ToString(), new Vector2(0, 0), Color.Red);
            spriteBatch.Draw(dummyTexture, new Rectangle(mousePosition.X, mousePosition.Y, 5, 5), Color.White);
            //    spriteBatch.Draw(shipTexture, Helper.ToRectangle(ship.GetLocalBounds()), null, Color.White, ship.GetRotation(),null, SpriteEffects.None, 0f);

            spriteBatch.End();
            //base.Draw(gameTime);
        }


    }
}
