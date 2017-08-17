using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace MonoGameDirectX {
    public static class Renderer {
        //static Renderer instance;

        //public static Renderer Instance {
        //    get { return instance; }
        //    set { instance = value; }
        //}


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
                    SpriteBatch, 1, ship.Color);
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
            return;
            foreach(var c in AIShipsController.Controllers)
                if(c != null && c is DefaultAutoControl) {
                    DrawPrimitives.DrawLine(
                       Viewport.World2ScreenPoint((c.Owner.Location)),
                       Viewport.World2ScreenPoint(((c as DefaultAutoControl).TargetLocation)),
                       SpriteBatch, 1, Color.Blue);
                }

            //DrawPrimitives.DrawCircle(WinAdapter.CoordPoint2Vector(rect.Center), rect.Width / 2, SpriteBatch, Color.Brown);
        }

        static int debugMode = 2;
        internal static void SwitchDebugMode() {

            if(debugMode < 2)
                debugMode++;
            else debugMode = 0;

           Debugger.AddLine("debug mode: " + debugMode);
        }

        internal static void Set(GraphicsDevice graphicsDevice, SpriteFont spriteFont) {
            GraphicsDevice = graphicsDevice;
            Font = spriteFont;
            SpriteBatch = new SpriteBatch(graphicsDevice);
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
            int lines = ScreenHeight / 20;
            int line = Debugger.LinesCount - 1;
            for(int y = ScreenHeight - 30; y > 9 && line >= 0; y -= 20) {
                SpriteBatch.DrawString(Font, Debugger.GetLine(line), new Vector2(10, y), new Color(Color.Black, 0.8f));
                line--;
            }
        }

        static void DrawInterface(GameTime time, Rectangle viewport) {
            // DrawPrimitives.DrawRect(new Rectangle(Viewport.Location.ToPoint(), Viewport.PxlSize.ToPoint()), SpriteBatch, 2, Color.Purple);

            InterfaceController.GetActualControls().ToList().ForEach(con => con.Draw(SpriteBatch, time));
            var playerInterface = PlayerController.GetInterfaceElements(viewport);
            foreach(IRenderableObject obj in playerInterface)
                WinAdapter.CreateRenderObject(obj).Draw(SpriteBatch, time);
            //foreach(Control c in controls)
            //c.Draw(SpriteBatch, time);
        }




        static int mapSize = 300;
        static Rectangle MapBorder {
            get { return new Rectangle(ScreenWidth / 2 - mapSize / 2, ScreenHeight / 2 - mapSize / 2 - 1, mapSize, mapSize); }
        }

        static void DrawMap() {
            // Thread.Sleep(50);
            //GenerateTexture();
            //DrawPrimitives.DrawCircle(MapBorder.Center.ToVector2(), mapSize/2, SpriteBatch,  Color.Black, MapBorder);
            Texture2D back = TextureGenerator.Rectangle(GraphicsDevice, mapSize, Color.LightGray, Color.DarkSlateBlue);// Circle(GraphicsDevice, mapSize / 2, Color.Gray);
            SpriteBatch.Draw(back, MapBorder.Center.ToVector2(), null, Color.White, 0f, new Vector2(mapSize / 2f, mapSize / 2f), 1, SpriteEffects.None, 0);


            Rectangle rect = new Rectangle((-MapBorder.Size.ToVector2() / 2).ToPoint(), MapBorder.Size);


            foreach(var obj in MainCore.Instance.Objects.Where(o => o is Ship || o is SpaceBody)) {
                Vector2 objLocation = MapBorder.Center.ToVector2() + obj.Location / 10f;
                if(!MapBorder.Contains(objLocation))
                    continue;

                if(obj is SpaceBody) {
                    int radius = (int)(obj.Radius / 10f);
                    //      DrawPrimitives.DrawCircle(objLocation, radius, SpriteBatch, Color.Red, MapBorder);
                    var tex = TextureGenerator.Circle(GraphicsDevice, radius, Color.DarkSlateGray);
                    SpriteBatch.Draw(tex, objLocation, null, Color.White, 0f, new Vector2(radius, radius), 1, SpriteEffects.None, 0);
               //     var shadow = TextureGenerator.CircleShadow(GraphicsDevice, radius, Color.DarkSlateGray);
                 //   SpriteBatch.Draw(shadow, objLocation, null, Color.White, 0, new Vector2(radius, radius), 1, SpriteEffects.None, 0);
                }

                else {
                    Ship ship = obj as Ship;
                    var tex = TextureGenerator.Circle(GraphicsDevice, 4, ship.Fraction == 1 ? Color.IndianRed : Color.CornflowerBlue);
                    SpriteBatch.Draw(tex, objLocation, null, Color.White, 0f, new Vector2(3, 3), 1, SpriteEffects.None, 0);

                    Player pl = PlayerController.Players.FirstOrDefault(p => p.Ship == ship);

                    if(pl != null) {
                        SpriteBatch.DrawString(Font, "p" + pl.Index, objLocation, ship.Fraction == 1 ? Color.IndianRed : Color.CornflowerBlue);
                    }

                }
            }
        }

        private static void GenerateTexture() {
            //generate
            var width = 128;
            var exp = TextureGenerator.Explosion(GraphicsDevice, width, 2 * Vector2.One.GetRotated(Rnd.GetPeriod()));

            // save
            var destrect = new Rectangle(0, 0, width, width);
            Stream stream = File.Create("explosion_generated.png");
            exp.SaveAsPng(stream, exp.Width, exp.Height);

            //draw
            //SpriteBatch.Draw(exp, destrect, new Rectangle(width * t, 0, width, width), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public static void RenderInterface(GameTime time) {
            SpriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null, null);
            DrawPrimitives.DrawRect(new Rectangle(1, 1, ScreenWidth - 2, ScreenHeight - 2), SpriteBatch, 3, Color.Black);

            DrawMap();

            SpriteBatch.End();
        }

        public static void RenderTotalOverlay(GameTime time, Rectangle viewport) {
            SpriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null, null);

            DrawInterface(time, viewport);
          //  WriteDebugInfo();
            DrawCursor();

            SpriteBatch.End();
        }

        public static void Render(GameTime gameTime) {
            SpriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null, null);

            if(InterfaceController.CurrentState.InGame) {
                DrawGrid();
                if(debugMode == 0 || debugMode == 1)
                    DrawDebugInfo();
                DrawObjects(gameTime);
            }
            DrawPrimitives.DrawRect(new Rectangle(1, 1, ScreenWidth - 2, ScreenHeight - 2), SpriteBatch, 3, Color.Black);

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
