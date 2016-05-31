using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDirectX {
    public class Renderer {
        GraphicsDevice graphicsDevice;
        Rectangle miniMapBorder;
        DrawPrimitives primitiveDrawer;
        List<RenderObject> renderObjects;
        SpriteBatch spriteBatch;

        InteractionController Controller { get { return MainCore.Instance.Controller; } }
        GameState GameState { get { return MainCore.State; } }
        GameCore.Viewport Viewport { get { return MainCore.Instance.Viewport; } }

        public SpriteFont Font { get; set; }
        public int ScreenHeight { get { return graphicsDevice.Viewport.Height; } }
        public int ScreenWidth { get { return graphicsDevice.Viewport.Width; } }

        public Renderer(GraphicsDevice gd) {
            graphicsDevice = gd;
            spriteBatch = new SpriteBatch(graphicsDevice);
            primitiveDrawer = new DrawPrimitives(graphicsDevice);
            miniMapBorder = new Rectangle(ScreenWidth - 100, ScreenHeight - 100, 90, 90);
        }

        void DrawCursor() {
            Point mPoint = Mouse.GetState().Position;
            primitiveDrawer.DrawRect(new Rectangle(mPoint.X, mPoint.Y, 10, 10), spriteBatch, 1, Color.White, Color.Red);
            //spriteBatch.Draw(WinAdapter.GetCursor(), new Rectangle(mPoint.X, mPoint.Y, 5, 5), Color.Black);
        }
        void DrawDebugInfo() {
            foreach(Ship ship in MainCore.Instance.Ships) {
                Bounds shipBounds = ship.GetScreenBounds();
                primitiveDrawer.DrawLine(
                    WinAdapter.CoordPoint2Vector(shipBounds.Center),
                    (WinAdapter.CoordPoint2Vector((shipBounds + ship.Direction * 20).Center)),
                    spriteBatch, 1, new Color(ship.Color.r, ship.Color.g, ship.Color.b));

                //primitiveDrawer.DrawLine(
                //    WinAdapter.CoordPoint2Vector(shipBounds.Center),
                //    WinAdapter.CoordPoint2Vector(ship.TargetObject.GetScreenBounds().Center),
                //    spriteBatch, Color.Yellow);

                //if(ship.Reactive.Length > 0)
                //    primitiveDrawer.DrawLine(
                //    WinAdapter.CoordPoint2Vector(shipBounds.Center),
                //    (WinAdapter.CoordPoint2Vector((shipBounds + ship.Reactive).Center)),
                //    spriteBatch, 1, Color.Yellow);
            }
        }
        void DrawMiniMap() {
            primitiveDrawer.DrawRect(miniMapBorder, spriteBatch, 3, Color.GhostWhite, Color.Green);
            foreach(RenderObject renderObject in renderObjects)
                if(renderObject.MiniMapLocation != null) {
                    Vector2 objLocation = miniMapBorder.Center.ToVector2() + renderObject.MiniMapLocation;
                    primitiveDrawer.DrawCircle(objLocation, 3, spriteBatch, Color.Yellow, miniMapBorder);
                }
        }
        void DrawObjects(GameTime gameTime) {
            WinAdapter.UpdateRenderObjects(ref renderObjects);
            foreach(RenderObject renderObject in renderObjects)
                if(Viewport.Bounds.Contains(renderObject.GameObject.Position)) {
                    renderObject.Draw(spriteBatch, gameTime);
                    primitiveDrawer.DrawRectDotted(WinAdapter.Bounds2Rectangle(renderObject.GameObject.GetScreenBounds()), spriteBatch, 2, Color.Yellow); // TODO remove
                }
        }
       
        void WriteDebugInfo() {
            spriteBatch.DrawString(Font, Viewport.Scale.ToString(), new Vector2(0, 0), Color.White);
        }

        public void DrawInterface(IEnumerable<Control> controls) {
            foreach(Control c in controls) 
                c.Draw(primitiveDrawer, spriteBatch);
            
        }
        public void Render(GameTime gameTime) {
            graphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin();


            if(GameState == GameState.Space) {

                DrawObjects(gameTime);
                DrawMiniMap();
                DrawDebugInfo();
            }
            DrawInterface(Controller.GetActualInterface().Cast<Control>());
            DrawCursor();
            WriteDebugInfo();

            spriteBatch.End();
        }
    }
}
