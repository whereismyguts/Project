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

                if(ship.Reactive.Length > 0)
                    primitiveDrawer.DrawLine(
                    WinAdapter.CoordPoint2Vector(shipBounds.Center),
                    (WinAdapter.CoordPoint2Vector((shipBounds + ship.Reactive).Center)),
                    spriteBatch, 1, Color.Yellow);
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
            foreach(RenderObject renderObject in renderObjects) {
                renderObject.Draw(spriteBatch, gameTime);
                primitiveDrawer.DrawRectDotted(WinAdapter.Bounds2Rectangle(renderObject.GameObject.GetScreenBounds()), spriteBatch, 2, Color.Yellow); // TODO remove
                // if(!string.IsNullOrEmpty(renderObject.Name))
                //     spriteBatch.DrawString(Font, renderObject.Name, WinAdapter.CoordPoint2Vector(renderObject.Bounds.LeftTop), Color.Red);
            }
        }
        void WriteDebugInfo() {
            spriteBatch.DrawString(Font, MainCore.Instance.Viewport.Scale.ToString(), new Vector2(0, 0), Color.White);
        }

        public void DrawInterface(IEnumerable<Control> controls) {
            foreach(Control c in controls) {
                primitiveDrawer.DrawRect(c.Rectangle, spriteBatch, 1, c.ActualBorderColor, c.ActualFillColor);
                Label l = c as Label;
                if(l != null) {
                    Vector2 textSize = Font.MeasureString(l.Text);
                    Vector2 panSize = l.Rectangle.Size.ToVector2();
                    Vector2 textLocation = l.Rectangle.Location.ToVector2() + (panSize - textSize) / 2;
                    spriteBatch.DrawString(Font, l.Text, textLocation, l.TextColor);
                }
            }
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
