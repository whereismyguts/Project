using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonoGameDirectX {
    public class Renderer {
        List<RenderObject> renderObjects;
        DrawPrimitives primitiveDrawer;
        Rectangle miniMapBorder;
        GraphicsDevice graphicsDevice;
        SpriteBatch spriteBatch;


        #region animation
        float time;
        // duration of time to show each frame
        float frameTime = 0.1f;
        // an index of the current frame being shown
        int frameIndex;
        // total number of frames in our spritesheet
        const int totalFrames = 10;
        // define the size of our animation frame
        int frameHeight = 64;
        int frameWidth = 64;
        #endregion

        public SpriteFont Font { get; set; }

        public Renderer(GraphicsDevice gd) {
            graphicsDevice = gd;
            spriteBatch = new SpriteBatch(graphicsDevice);
            primitiveDrawer = new DrawPrimitives(graphicsDevice);
            miniMapBorder = new Rectangle(graphicsDevice.Viewport.Width - 100, graphicsDevice.Viewport.Height - 100, 90, 90);
        }
        public void Render(GameTime gameTime) {
            graphicsDevice.Clear(Color.White);


            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(time > frameTime) {
                // Play the next frame in the SpriteSheet
                frameIndex++;

                // reset elapsed time
                time = 0f;
            }
            if(frameIndex > totalFrames) frameIndex = 1;

            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(frameIndex * frameWidth, 0, frameWidth, frameHeight);

            // Calculate position and origin to draw in the center of the screen
            Vector2 position = new Vector2(100,100);
            Vector2 origin = new Vector2(frameWidth / 2.0f, frameHeight);

            spriteBatch.Begin();

            // Draw the current frame.
            spriteBatch.Draw(WinAdapter.GetTexture("Celebrate"), position, source, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);

            primitiveDrawer.DrawRect(new Rectangle((position-origin).ToPoint(), new Point(frameWidth, frameHeight)), spriteBatch, 1, Color.Red);



            
            WinAdapter.UpdateRenderObjects(ref renderObjects);

            DrawObjects(gameTime);
            DrawMiniMap();
            DrawVisualInfo();
            DrawCursor();
            WriteDebugInformation();
            spriteBatch.End();
        }
        void DrawCursor() {
            Point mPoint = Mouse.GetState().Position;
            spriteBatch.Draw(WinAdapter.GetCursor(), new Rectangle(mPoint.X, mPoint.Y, 5, 5), Color.Black);
        }
        void DrawObjects(GameTime gameTime) {
            foreach(RenderObject renderObject in renderObjects) {
                renderObject.Draw(spriteBatch, gameTime);
                if(!string.IsNullOrEmpty(renderObject.Name))
                    spriteBatch.DrawString(Font, renderObject.Name, WinAdapter.CoordPoint2Vector(renderObject.Bounds.LeftTop), Color.Red);
            }
        }
        void DrawMiniMap() {
            foreach(RenderObject renderObject in renderObjects)
                if(renderObject.MiniMapBounds != null) {
                    Vector2 objLocation = miniMapBorder.Location.ToVector2() + WinAdapter.CoordPoint2Vector(renderObject.MiniMapBounds.Center);
                    primitiveDrawer.DrawCircle(objLocation, renderObject.MiniMapBounds.Width, spriteBatch, Color.Yellow, miniMapBorder);
                }
            primitiveDrawer.DrawRect(miniMapBorder, spriteBatch, 3, Color.GhostWhite);
        }
        void DrawVisualInfo() {
            foreach(Ship ship in MainCore.Instance.Ships) {
                primitiveDrawer.DrawLine(
                    WinAdapter.CoordPoint2Vector(ship.GetScreenBounds().Center),
                    (WinAdapter.CoordPoint2Vector((ship.GetScreenBounds() + ship.Direction * 20).Center)),
                    spriteBatch, 1, new Color(ship.Color.r, ship.Color.g, ship.Color.b));

                //primitiveDrawer.DrawLine(
                //    WinAdapter.CoordPoint2Vector(ship.GetScreenBounds().Center),
                //    WinAdapter.CoordPoint2Vector(ship.TargetObject.GetScreenBounds().Center),
                //    spriteBatch, Color.Yellow);

                if(ship.Reactive.Length > 0)
                    primitiveDrawer.DrawLine(
                    WinAdapter.CoordPoint2Vector(ship.GetScreenBounds().Center),
                    (WinAdapter.CoordPoint2Vector((ship.GetScreenBounds() + ship.Reactive).Center)),
                    spriteBatch, 1, Color.Yellow);
            }
        }
        void WriteDebugInformation() {
            spriteBatch.DrawString(Font, MainCore.Instance.Viewport.Scale.ToString(), new Vector2(0, 0), Color.White);
        }

    }
}
