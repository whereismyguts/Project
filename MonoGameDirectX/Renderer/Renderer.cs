using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDirectX {
    public static class Renderer {
        //static Renderer instance;

        //public static Renderer Instance {
        //    get { return instance; }
        //    set { instance = value; }
        //}

        static Rectangle miniMapBorder;
        // DrawPrimitives primitiveDrawer;
        static List<RenderObject> renderObjects;

        static UIState GameState { get { return MainCore.Instance.CurrentState; } }
        static GameCore.Viewport Viewport { get { return MainCore.Instance.Viewport; } }

        public static GraphicsDevice GraphicsDevice { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static SpriteFont Font { get; set; }

        public static int ScreenHeight { get { return GraphicsDevice.Viewport.Height; } }
        public static int ScreenWidth { get { return GraphicsDevice.Viewport.Width; } }

        public static bool DebugMode { get { return debugMode; } }

        //public List<CoordPoint> TraectoryPath { get; internal set; }

        static void DrawCursor() {
            Point mPoint = Mouse.GetState().Position;
            DrawPrimitives.DrawRect(new Rectangle(mPoint.X, mPoint.Y, 10, 10), SpriteBatch, 1, Color.White, Color.Red);
            //spriteBatch.Draw(WinAdapter.GetCursor(), new Rectangle(mPoint.X, mPoint.Y, 5, 5), Color.Black);
        }
        static void DrawDebugInfo() {
            foreach(Ship ship in MainCore.Instance.Ships) {
                Bounds shipBounds = ship.GetScreenBounds();
                DrawPrimitives.DrawLine(
                    WinAdapter.CoordPoint2Vector(shipBounds.Center),
                    (WinAdapter.CoordPoint2Vector((shipBounds + ship.Direction * 10).Center)),
                    SpriteBatch, 3, new Color(ship.Color.r, ship.Color.g, ship.Color.b));
                //if(ship.Velosity.Length > 0)
                //    DrawPrimitives.DrawLine(
                //    WinAdapter.CoordPoint2Vector(shipBounds.Center),
                //    (WinAdapter.CoordPoint2Vector((shipBounds + ship.Velosity).Center)),
                //    spriteBatch, 1, Color.Yellow);


                //if(ship.Calculator != null) {
                //    for(int i = 0; i < ship.Calculator.Path.Count - 1; i++)
                //        DrawPrimitives.DrawPixel(WinAdapter.CoordPoint2Vector(Viewport.World2ScreenPoint(ship.Calculator.Path[i])), spriteBatch, Color.Black);
                //}


                var rect = ship.GetScreenBounds();
                DrawPrimitives.DrawCircle(WinAdapter.CoordPoint2Vector(rect.Center), rect.Width / 2, SpriteBatch, ship.Fraction > 1 ? Color.Red : Color.Blue);
            }
            //foreach(var c in AIShipsController.Controllers) {
            //    DefaultAutoControl ac = c as DefaultAutoControl;
            //    if(ac != null && ac.TargetLocation != null)
            //        DrawPrimitives.DrawLine(
            //            WinAdapter.CoordPoint2Vector(ac.Owner.GetScreenBounds().Center),
            //            WinAdapter.CoordPoint2Vector(Viewport.World2ScreenPoint(ac.TargetLocation)),
            //            SpriteBatch, 1, new Color(ac.Owner.Color.r, ac.Owner.Color.g, ac.Owner.Color.b));
            //}
            //if(TraectoryPath != null)
            //    for(int i = 0; i < TraectoryPath.Count - 1; i++) {
            //         DrawPrimitives.DrawPixel(WinAdapter.CoordPoint2Vector(Viewport.World2ScreenPoint(TraectoryPath[i])), spriteBatch, Color.Black);
            //        //DrawPrimitives.DrawLine(WinAdapter.CoordPoint2Vector(Viewport.World2ScreenPoint(TraectoryPath[i])), WinAdapter.CoordPoint2Vector(Viewport.World2ScreenPoint(TraectoryPath[i + 1])), spriteBatch, 1, Color.Violet);

            //    }
            if(MainCore.Cursor != null) {
                DrawPrimitives.DrawCircle(WinAdapter.CoordPoint2Vector(Viewport.World2ScreenPoint(MainCore.Cursor)), 5, SpriteBatch, Color.Red);
            }
            //var rect = Viewport.World2ScreenBounds(new Bounds(-25000, -25000, 50000, 50000));

            //foreach(var c in ShipController.Controllers) 
            //    if(c != null && c is AutoControl) { 
            //        DrawPrimitives.DrawLine(
            //           WinAdapter.CoordPoint2Vector(Viewport.World2ScreenPoint((c.Owner.Position))),
            //           WinAdapter.CoordPoint2Vector(Viewport.World2ScreenPoint(((c as AutoControl).TargetLocation))),
            //           spriteBatch, 1, Color.Blue);
            //    }

            //DrawPrimitives.DrawCircle(WinAdapter.CoordPoint2Vector(rect.Center), rect.Width / 2, SpriteBatch, Color.Brown);
        }

        static bool debugMode = true;
        internal static void SwitchDebugMode() {
            debugMode = !debugMode;
        }

        internal static void Set(GraphicsDevice graphicsDevice, SpriteFont spriteFont) {
            GraphicsDevice = graphicsDevice;
            Font = spriteFont;
            SpriteBatch = new SpriteBatch(graphicsDevice);

            miniMapBorder = new Rectangle(ScreenWidth - 100, ScreenHeight - 100, 90, 90);
        }

        static void DrawMiniMap() {
            //DrawPrimitives.DrawRect(miniMapBorder, SpriteBatch, 3, Color.GhostWhite, Color.Green);
            //foreach(RenderObject renderObject in renderObjects)
            //    if(renderObject.MiniMapLocation != null) {
            //        Vector2 objLocation = miniMapBorder.Center.ToVector2() + renderObject.MiniMapLocation;
            //        DrawPrimitives.DrawCircle(objLocation, 3, SpriteBatch, Color.Black, miniMapBorder);
            //    }
        }
        static void DrawObjects(GameTime gameTime) {
            WinAdapter.UpdateRenderObjects(ref renderObjects);
            foreach(RenderObject renderObject in renderObjects) {
                //DONT remove!!
                renderObject.Draw(SpriteBatch, gameTime);
                //if(renderObject.GameObject is Body)
                //    renderObject.Draw(SpriteBatch, gameTime);

                //DrawPrimitives.DrawCircle(WinAdapter.CoordPoint2Vector(renderObject.GameObject.GetScreenBounds().Center), renderObject.GameObject.GetScreenBounds().Width / 2, SpriteBatch, Color.Blue);
            }
        }

        static void WriteDebugInfo() {
            int lines = ScreenHeight / 30;
            int line = Debugger.Lines.Count - 1;
            for(int i = ScreenHeight - 30; i > 10 && line >= 0; i -= 30) {
                SpriteBatch.DrawString(Font, Debugger.Lines[line], new Vector2(0, i), Color.Black);
                line--;
            }
        }

        static void DrawInterface(GameTime time) {
            InterfaceController.GetActualControls().ToList().ForEach(con => con.Draw(SpriteBatch, time));
            //foreach(Control c in controls)
            //c.Draw(SpriteBatch, time);
        }


        public static void Render(GameTime gameTime) {

            GraphicsDevice.Clear(Color.White);
            SpriteBatch.Begin();

            //cells = new StationGenerator().Generate();
            //if(GameState == GameState.Station) {
            //    for(int i = 0; i < 400; i++)
            //        for(int j = 0; j < 400; j++) {
            //            Color c = Color.LightGray;
            //            switch(cells[i, j]) {
            //                case 1:
            //                    c = Color.Red;
            //                    break;
            //                case 2:
            //                    c = Color.Green;
            //                    break;
            //            }
            //            DrawPrimitives.DrawRect(new Rectangle(i * 2, j * 2, 2, 2), spriteBatch, 1, c, c);
            //            //DrawPrimitives.DrawPixel(new Vector2(i*2, j*2), spriteBatch, c);
            //        }
            //    spriteBatch.End();
            //    return;
            //}

            if(InterfaceController.CurrentState.InGame) {
                DrawMiniMap();
                if(debugMode)
                    DrawDebugInfo();
                DrawObjects(gameTime);
            }

            DrawInterface(gameTime);
            DrawCursor();
            WriteDebugInfo();

            SpriteBatch.End();
        }
    }
}
