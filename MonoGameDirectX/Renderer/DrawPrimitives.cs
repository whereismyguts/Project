using System;
using System.Linq;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDirectX {
    public static class DrawPrimitives {
        static Color defaultFillColor = Color.Transparent;
        //  static GraphicsDevice graphDevice;

        static Texture2D texture;
        static Texture2D GetTexture(SpriteBatch sb) {
            if(texture == null) {
                texture = new Texture2D(sb.GraphicsDevice, 1, 1);
                texture.SetData(new Color[] { Color.White });
            }
            return texture;
        }

        static void DrawPixel(double x, double y, SpriteBatch spBatch, Color color, Rectangle clip) {

            if(!clip.IsEmpty)
                if(!clip.Contains(new Point((int)x, (int)y)))
                    return;


            spBatch.Draw(GetTexture(spBatch),
                new Rectangle((int)x, (int)y, 1, 1),
               null,
                color, //colour of line
                0, //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }

        //public void Dispose() {
        //    t = null;
        //    graphDevice = null;
        //    Dispose();
        //}
        public static void DrawCircle(Vector2 center, float radius, SpriteBatch spBatch, Color color) {
            DrawCircle(center, radius, spBatch, color, Rectangle.Empty);
        }

        internal static void DrawGeometry(Geometry geom, SpriteBatch spriteBatch) {

            var location = geom.ScreenLocation-geom.ScreenOrigin;
            var size = geom.ScreenSize;

            var screenRect = new Rectangle((int)location.X, (int)location.Y, (int)size.X, (int)size.Y);
            DrawRect(screenRect, spriteBatch, 1, Color.Black, new Color(Color.Blue, 0.7f));
        }

        //private static void DrawCircle(Vector2 vector2, object radius, SpriteBatch spriteBatch, Color black) {
        //    throw new NotImplementedException();
        //}

        public static void DrawCircle(Vector2 center, float radius, SpriteBatch spBatch, Color color, Rectangle clip) {
            double theta = -Math.PI;  // angle that will be increased each loop
                                      //    double step = .01;  // amount to add to theta each time (degrees)

            //   if(radius < 1)
            //        radius = 1;
            double c = 2 * Math.PI * radius;
            double step = Math.PI / c;

            while(theta < Math.PI) {
                float x = (float)(center.X + radius * Math.Cos(theta));
                float y = (float)(center.Y + radius * Math.Sin(theta));
                DrawPixel(x, y, spBatch, color, clip);
                theta += step;
            }
            //  spBatch.DrawString(Renderer.Font, radius.ToString("f2"), center + new Vector2(radius / 4, radius / 4), Color.Black);
        }
        public static void DrawLineDotted(Vector2 start, Vector2 end, SpriteBatch spBatch, int width, Color color) {
            Vector2 v = end - start;
            int count = (int)(v.Length());
            v.Normalize();
            for(int i = 0; i < count; i += 10)
                DrawLine(start + v * i, start + v * (i + 5), spBatch, width, color);
        }
        public static void DrawLine(Vector2 start, Vector2 end, SpriteBatch spBatch, int width, Color color) {

            var edge = end - start;
            // calculate angle to rotate line
            // calculate angle to rotate line
            var angle = (float)Math.Atan2(edge.Y, edge.X);
            spBatch.Draw(GetTexture(spBatch),
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    width), //width of line, change this to make thicker line
                null,
                color,
                angle, //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }

        public static void DrawPixel(Vector2 point, SpriteBatch spBatch, Color color) {
            DrawPixel(point, spBatch, color, Rectangle.Empty);
        }

        internal static void DrawRectDotted(Rectangle rect, SpriteBatch spBatch, int width, Color borderColor) {
            spBatch.Draw(GetTexture(spBatch), rect, null, defaultFillColor, 0f, new Vector2(), SpriteEffects.None, 0f);


            var lt = new Vector2(rect.Left, rect.Top);
            var lb = new Vector2(rect.Left, rect.Bottom);
            var rt = new Vector2(rect.Right, rect.Top);
            var rb = new Vector2(rect.Right, rect.Bottom);
            DrawLineDotted(lt, rt, spBatch, width, borderColor);
            DrawLineDotted(rt, rb, spBatch, width, borderColor);
            DrawLineDotted(rb, lb, spBatch, width, borderColor);
            DrawLineDotted(lb, lt, spBatch, width, borderColor);
        }

        public static void DrawPixel(Vector2 point, SpriteBatch spBatch, Color color, Rectangle border) {
            DrawPixel(point.X, point.Y, spBatch, color, border);
        }
        public static void DrawRect(Rectangle rect, SpriteBatch spBatch, int strokeWidth, Color borderColor) {
            DrawRect(rect, spBatch, strokeWidth, borderColor, defaultFillColor);
        }
        public static void DrawRect(Rectangle rect, SpriteBatch spBatch, int width, Color borderColor, Color fillColor) {
            spBatch.Draw(GetTexture(spBatch), rect, null, fillColor, 0f, new Vector2(), SpriteEffects.None, 0f);


            var lt = new Vector2(rect.Left, rect.Top);
            var lb = new Vector2(rect.Left, rect.Bottom);
            var rt = new Vector2(rect.Right, rect.Top);
            var rb = new Vector2(rect.Right, rect.Bottom);
            DrawLine(lt, rt, spBatch, width, borderColor);
            DrawLine(rt, rb, spBatch, width, borderColor);
            DrawLine(rb, lb, spBatch, width, borderColor);
            DrawLine(lb, lt, spBatch, width, borderColor);
        }

    }
}
