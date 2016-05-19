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
        //Texture2D dummyTexture;
        SpriteFont font;
        GraphicsDeviceManager graphics;
        Point mousePosition;
        DrawPrimitives primitiveDrawer;
        SpriteBatch spriteBatch;
        Rectangle miniMapBorder;
        public GameMain() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        void WriteDebugInformation() {
            spriteBatch.DrawString(font, MainCore.Instance.Viewport.Scale.ToString(), new Vector2(0, 0), Color.White);
        }
        void DrawCursor() {
            spriteBatch.Draw(WinAdapter.GetCursor(), new Rectangle(mousePosition.X, mousePosition.Y, 5, 5), Color.White);
        }
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            foreach(RenderObjectCore obj in MainCore.Instance.RenderObjects) {
                RenderObject renderObject = WinAdapter.CreateRenderObject(obj);
                spriteBatch.Draw(renderObject.Texture, renderObject.TextureRect, null, renderObject.ColorMask, renderObject.Rotation, renderObject.Origin, SpriteEffects.None, 0);
                primitiveDrawer.DrawCircle(renderObject.TextureRect.Location.ToVector2(), renderObject.TextureRect.Width / 2, spriteBatch, Color.Red);
                primitiveDrawer.DrawCircle(new Vector2(GraphicsDevice.Viewport.Width - 100, GraphicsDevice.Viewport.Height - 100) + WinAdapter.CoordPoint2Vector(renderObject.MiniMapBounds.Center) / 10, renderObject.MiniMapBounds.Width / 10f, spriteBatch, Color.Yellow, miniMapBorder);
                primitiveDrawer.DrawRect(miniMapBorder, spriteBatch, 3, Color.GhostWhite);

                if(!string.IsNullOrEmpty(renderObject.Name))
                    spriteBatch.DrawString(font, renderObject.Name, renderObject.TextureRect.Location.ToVector2(), Color.Red);

            }
            //foreach(Ship ship in MainCore.Instance.Ships) {
            //    primitiveDrawer.DrawLine(
            //        WinAdapter.CoordPoint2Vector(ship.GetScreenBounds().Center),
            //        (WinAdapter.CoordPoint2Vector((ship.GetScreenBounds() + ship.Direction * 20).Center)),
            //        spriteBatch, 1, new Color(ship.Color.r, ship.Color.g, ship.Color.b));

            //    //primitiveDrawer.DrawLine(
            //    //    WinAdapter.CoordPoint2Vector(ship.GetScreenBounds().Center),
            //    //    WinAdapter.CoordPoint2Vector(ship.TargetObject.GetScreenBounds().Center),
            //    //    spriteBatch, Color.Yellow);

            //    if(ship.Reactive.Length > 0)
            //        primitiveDrawer.DrawLine(
            //        WinAdapter.CoordPoint2Vector(ship.GetScreenBounds().Center),
            //        (WinAdapter.CoordPoint2Vector((ship.GetScreenBounds() + ship.Reactive).Center)),
            //        spriteBatch, 5, Color.Yellow);
            //}
            WriteDebugInformation();
            DrawCursor();
            spriteBatch.End();
            base.Draw(gameTime);
        }
        protected override void Initialize() {
            primitiveDrawer = new DrawPrimitives(GraphicsDevice);
            MainCore.Instance.Viewport.SetViewportSize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            miniMapBorder = new Rectangle(GraphicsDevice.Viewport.Width - 100, GraphicsDevice.Viewport.Height - 100, 90, 90);
            base.Initialize();
        }
        protected override void LoadContent() {
            WinAdapter.LoadContent(Content, GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);

          

            font = Content.Load<SpriteFont>("Arial");
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



            mousePosition = Mouse.GetState().Position;
            base.Update(gameTime);
        }
    }
}
