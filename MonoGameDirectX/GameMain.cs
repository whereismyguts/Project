using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

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
            foreach(Character ship in GameCore.Instance.Ships) {
                primitiveDrawer.DrawLine(
                    WinAdapter.CoordPoint2Vector(ship.GetScreenBounds().Center),
                    (WinAdapter.CoordPoint2Vector((ship.GetScreenBounds() + ship.Direction * 20).Center)),
                    spriteBatch, new Color(ship.Color.r, ship.Color.g, ship.Color.b));

                primitiveDrawer.DrawLine(
                    WinAdapter.CoordPoint2Vector(ship.GetScreenBounds().Center),
                    WinAdapter.CoordPoint2Vector(ship.Target.GetScreenBounds().Center),
                    spriteBatch, new Color(ship.Color.r, ship.Color.g, ship.Color.b));
            }



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
                GameCore.Instance.Ships.Last().AccselerateEngine();
            else
                GameCore.Instance.Ships.Last().LowDownEngine();
            //if(Keyboard.GetState().IsKeyDown(Keys.Down))
            //    GameCore.Instance.Ships.Last().LowDownEngine();
            if(Keyboard.GetState().IsKeyDown(Keys.Left))
                GameCore.Instance.Ships.Last().RotateL();
            if(Keyboard.GetState().IsKeyDown(Keys.Right))
                GameCore.Instance.Ships.Last().RotateR();

            if(Keyboard.GetState().IsKeyDown(Keys.Up))
                GameCore.Instance.Viewport.Centerpoint+=new CoordPoint(0,-10);
            if(Keyboard.GetState().IsKeyDown(Keys.Down))
                GameCore.Instance.Viewport.Centerpoint += new CoordPoint(0, 10);
            if(Keyboard.GetState().IsKeyDown(Keys.Right))
                GameCore.Instance.Viewport.Centerpoint += new CoordPoint(10, 0);
            if(Keyboard.GetState().IsKeyDown(Keys.Left))
                GameCore.Instance.Viewport.Centerpoint += new CoordPoint(-10, 0);
            


                mousePosition = Mouse.GetState().Position;
            base.Update(gameTime);
        }
    }
}
