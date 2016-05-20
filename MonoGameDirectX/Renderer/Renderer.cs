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
        public SpriteFont Font { get; set; }

        public Renderer(GraphicsDevice gd) {
            graphicsDevice = gd;
            spriteBatch = new SpriteBatch(graphicsDevice);
            primitiveDrawer = new DrawPrimitives(graphicsDevice);
            miniMapBorder = new Rectangle(graphicsDevice.Viewport.Width - 100, graphicsDevice.Viewport.Height - 100, 90, 90);
        }

        public void Render() {
            graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            WinAdapter.UpdateRenderObjects(ref renderObjects);

            DrawObjects();
            DrawMiniMap();
            DrawVisualInfo();
            DrawCursor();
            WriteDebugInformation();
            spriteBatch.End();
        }
        void DrawCursor() {
            Point mPoint = Mouse.GetState().Position;
            spriteBatch.Draw(WinAdapter.GetCursor(), new Rectangle(mPoint.X, mPoint.Y, 5, 5), Color.White);
        }
        void DrawObjects() {
            foreach(RenderObject renderObject in renderObjects) {
                spriteBatch.Draw(renderObject.Texture, renderObject.TextureRect, null, renderObject.ColorMask, renderObject.Rotation, renderObject.Origin, SpriteEffects.None, 0);
                primitiveDrawer.DrawCircle(renderObject.TextureRect.Location.ToVector2(), renderObject.TextureRect.Width / 2.0f, spriteBatch, Color.Red);

                if(!string.IsNullOrEmpty(renderObject.Name))
                    spriteBatch.DrawString(Font, renderObject.Name, renderObject.TextureRect.Location.ToVector2(), Color.Red);
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
