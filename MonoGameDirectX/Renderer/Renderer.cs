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
        // DrawPrimitives primitiveDrawer;
        List<RenderObject> renderObjects;
        SpriteBatch spriteBatch;

        InteractionController Controller { get { return MainCore.Instance.Controller; } }
        GameState GameState { get { return MainCore.Instance.State; } }
        GameCore.Viewport Viewport { get { return MainCore.Instance.Viewport; } }

        public SpriteFont Font { get; set; }
        public int ScreenHeight { get { return graphicsDevice.Viewport.Height; } }
        public int ScreenWidth { get { return graphicsDevice.Viewport.Width; } }

        public List<CoordPoint> TraectoryPath { get; internal set; }

        public Renderer(GraphicsDevice gd) {
            graphicsDevice = gd;
            spriteBatch = new SpriteBatch(graphicsDevice);
            miniMapBorder = new Rectangle(ScreenWidth - 100, ScreenHeight - 100, 90, 90);
        }

        void DrawCursor() {
            Point mPoint = Mouse.GetState().Position;
            DrawPrimitives.DrawRect(new Rectangle(mPoint.X, mPoint.Y, 10, 10), spriteBatch, 1, Color.White, Color.Red);
            //spriteBatch.Draw(WinAdapter.GetCursor(), new Rectangle(mPoint.X, mPoint.Y, 5, 5), Color.Black);
        }
        void DrawDebugInfo() {
            foreach(Ship ship in MainCore.Instance.Ships) {
                Bounds shipBounds = ship.GetScreenBounds();
                DrawPrimitives.DrawLine(
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
            if(TraectoryPath != null)
                for(int i = 0; i < TraectoryPath.Count - 1; i++) {
                     DrawPrimitives.DrawPixel(WinAdapter.CoordPoint2Vector(Viewport.World2ScreenPoint(TraectoryPath[i])), spriteBatch, Color.Black);
                    //DrawPrimitives.DrawLine(WinAdapter.CoordPoint2Vector(Viewport.World2ScreenPoint(TraectoryPath[i])), WinAdapter.CoordPoint2Vector(Viewport.World2ScreenPoint(TraectoryPath[i + 1])), spriteBatch, 1, Color.Violet);

                }
        }
        void DrawMiniMap() {
            DrawPrimitives.DrawRect(miniMapBorder, spriteBatch, 3, Color.GhostWhite, Color.Green);
            foreach(RenderObject renderObject in renderObjects)
                if(renderObject.MiniMapLocation != null) {
                    Vector2 objLocation = miniMapBorder.Center.ToVector2() + renderObject.MiniMapLocation;
                    DrawPrimitives.DrawCircle(objLocation, 3, spriteBatch, Color.Black, miniMapBorder);
                }
        }
        void DrawObjects(GameTime gameTime) {
            WinAdapter.UpdateRenderObjects(ref renderObjects);
            foreach(RenderObject renderObject in renderObjects) {
                renderObject.Draw(spriteBatch, gameTime);
                //if(renderObject.GameObject is Body)
                //    DrawPrimitives.DrawCircle(WinAdapter.CoordPoint2Vector(renderObject.GameObject.GetScreenBounds().Center), renderObject.GameObject.GetScreenBounds().Width / 2, spriteBatch, Color.Blue);
            }
        }

        void WriteDebugInfo() {
            spriteBatch.DrawString(Font, Viewport.Scale.ToString(), new Vector2(0, 0), Color.Black);
        }

        public void DrawInterface(IEnumerable<Control> controls, GameTime time) {
            foreach(Control c in controls)
                c.Draw(spriteBatch, time);

        }
        public void Render(GameTime gameTime) {
            graphicsDevice.Clear(Color.White);
            spriteBatch.Begin();


            if(GameState == GameState.Space) {

                DrawObjects(gameTime);
                DrawMiniMap();
                DrawDebugInfo();
            }
            DrawInterface(Controller.GetActualInterface().Cast<Control>(), gameTime);
            DrawCursor();
            WriteDebugInfo();

            spriteBatch.End();
        }
    }
}
