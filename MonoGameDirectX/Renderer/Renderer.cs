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

        public static int DebugMode { get { return debugMode; } }

        public static Texture2D Cover { get; internal set; }

        //public List<CoordPoint> TraectoryPath { get; internal set; }

        static void DrawCursor() {
            Point mPoint = Mouse.GetState().Position;
            // DrawPrimitives.DrawRect(new Rectangle(mPoint.X, mPoint.Y, 10, 10), SpriteBatch, 1, Color.White, Color.Red);
            SpriteBatch.Draw(WinAdapter.GetCursor(), new Rectangle(mPoint.X, mPoint.Y, 5, 5), Color.Black);
        }
        static void DrawDebugInfo() {
            foreach(Ship ship in MainCore.Instance.Ships) {
                Bounds shipBounds = ship.ScreenBounds;
                DrawPrimitives.DrawLine(
                    shipBounds.Center,
                    (shipBounds + ship.Direction * 10).Center,
                    SpriteBatch, 1, WinAdapter.Color(ship.Color));
                //if(ship.Velosity.Length > 0)
                //    DrawPrimitives.DrawLine(
                //    WinAdapter.CoordPoint2Vector(shipBounds.Center),
                //    (WinAdapter.CoordPoint2Vector((shipBounds + ship.Velosity).Center)),
                //    spriteBatch, 1, Color.Yellow);


                //if(ship.Calculator != null) {
                //    for(int i = 0; i < ship.Calculator.Path.Count - 1; i++)
                //        DrawPrimitives.DrawPixel(WinAdapter.CoordPoint2Vector(Viewport.World2ScreenPoint(ship.Calculator.Path[i])), spriteBatch, Color.Black);
                //}

                //     SpriteBatch.DrawString(Font, ship.Name, shipBounds.Center + new Vector2(0, -20), WinAdapter.Color(ship.Color));

                //var rect = ship.ScreenBounds;
                //DrawPrimitives.DrawCircle(rect.Center, rect.Width / 2, SpriteBatch, ship.Fraction > 1 ? Color.Red : Color.Blue);
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
            if(MainCore.Instance.Cursor != null) {

                var scrPoint = Viewport.World2ScreenPoint(MainCore.Instance.Cursor);

                DrawPrimitives.DrawCircle(scrPoint, 2, SpriteBatch, Color.Red);
                SpriteBatch.DrawString(Font, MainCore.Instance.Cursor.X.ToString("f1") + ":" + MainCore.Instance.Cursor.Y.ToString("f1"), scrPoint, Color.Red);
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

        static int debugMode = 2;
        internal static void SwitchDebugMode() {

            if(debugMode < 2)
                debugMode++;
            else debugMode = 0;

            Debugger.Lines.Add("debug mode: " + debugMode);
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
                SpriteBatch.DrawString(Font, Debugger.Lines[line], new Microsoft.Xna.Framework.Vector2(0, i), new Color(Color.Black, 0.8f));
                line--;
            }
        }

        static void DrawInterface(GameTime time) {
            // DrawPrimitives.DrawRect(new Rectangle(Viewport.Location.ToPoint(), Viewport.PxlSize.ToPoint()), SpriteBatch, 2, Color.Purple);

            InterfaceController.GetActualControls().ToList().ForEach(con => con.Draw(SpriteBatch, time));
            //foreach(Control c in controls)
            //c.Draw(SpriteBatch, time);
        }


        public static Texture2D CreateTexture(GraphicsDevice device, int width, int height) {
            //initialize a texture

            var view = Viewport.Rectangle;

            Texture2D texture = new Texture2D(device, width, height);

            //the array holds the color for each pixel in the texture

            Color[] data = new Color[width * height];
            int x = 0, y = 0;

            for(int pixel = 0; pixel < data.Count(); pixel++) {

                if((x == view.Left || x == view.Right) && (y > view.Top && y < view.Bottom)
                    ||
                    (y == view.Top || y == view.Bottom) && (x > view.Left && x < view.Right))
                    data[pixel] = Color.Black;
                else
                if(view.Contains(x, y))
                    data[pixel] = Color.Transparent;
                else data[pixel] = Color.White;

                x++;
                if(x == width) {
                    x = 0; y++;
                }
            }

            texture.SetData(data);
            return texture;
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
                DrawGrid();
                if(debugMode == 0 || debugMode == 1)
                    DrawDebugInfo();

                DrawObjects(gameTime);
            }
            SpriteBatch.Draw(Cover, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
            DrawInterface(gameTime);
            WriteDebugInfo();
            DrawCursor();

            SpriteBatch.End();
        }

        static int gridStep = 500;
        static int gridSize = 3000;

        private static void DrawGrid() {
            for(int i = -gridSize; i <= gridSize; i += gridStep) {

                var x1 = i; var x2 = i; var y1 = -gridSize; var y2 = gridSize;

                var p1 = Viewport.World2ScreenPoint(x1, y1);
                var p2 = Viewport.World2ScreenPoint(x2, y2);

                DrawPrimitives.DrawLine(p1, p2, SpriteBatch, 1, Color.Gray);

                x1 = -gridSize; x2 = gridSize; y1 = i; y2 = i;

                p1 = Viewport.World2ScreenPoint(x1, y1);
                p2 = Viewport.World2ScreenPoint(x2, y2);

                DrawPrimitives.DrawLine(p1, p2, SpriteBatch, 1, Color.Gray);
            }
        }
    }
}
