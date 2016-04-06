using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDirectX {
    public class DrawPrimitives: IDisposable {
        GraphicsDevice graphDevice;
        Texture2D t;

        public DrawPrimitives(GraphicsDevice gd) {
            graphDevice = gd;
            t = new Texture2D(gd, 1, 1);
            t.SetData<Color>(new Color[] { Color.White });
        }

        public void Dispose() {
            t = null;
            graphDevice = null;
            Dispose();
        }
        public void DrawLine(Vector2 start, Vector2 end, SpriteBatch spBatch, int width, Color color) {
            var edge = end - start;
            // calculate angle to rotate line
            // calculate angle to rotate line
            var angle = (float)Math.Atan2(edge.Y, edge.X);
            spBatch.Draw(t,
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
        public void DrawRect(Rectangle rect, SpriteBatch spBatch, int width, Color color) {
            var lt = new Vector2(rect.Left, rect.Top);
            var lb = new Vector2(rect.Left, rect.Bottom);
            var rt = new Vector2(rect.Right, rect.Top);
            var rb = new Vector2(rect.Right, rect.Bottom);
            DrawLine(lt, rt, spBatch, width, color);
            DrawLine(rt, rb, spBatch, width, color);
            DrawLine(rb, lb, spBatch, width, color);
            DrawLine(lb, lt, spBatch, width, color);
        }

        public void DrawPixel(Vector2 point, SpriteBatch spBatch, Color color) {
            DrawPixel(point, spBatch, color, Rectangle.Empty);
        }
        public void DrawPixel(Vector2 point, SpriteBatch spBatch, Color color, Rectangle alowedBorder) {
            DrawPixel(point, spBatch, color, alowedBorder);
        }

        public void DrawCircle(Vector2 center, float radius, SpriteBatch spBatch, Color color, Rectangle alowwedBorder) {
            double theta = -Math.PI;  // angle that will be increased each loop
            double step = .01;  // amount to add to theta each time (degrees)



            while(theta < Math.PI) {

                double x = center.X + radius * Math.Cos(theta);
                double y = center.Y + radius * Math.Sin(theta);
                DrawPixel(x, y, spBatch, color, alowwedBorder);
                theta += step;

            }
        }
        public void DrawCircle(Vector2 center, float radius, SpriteBatch spBatch, Color color) {
            DrawCircle(center, radius, spBatch, color, Rectangle.Empty);
        }

        void DrawPixel(double x, double y, SpriteBatch spBatch, Color color, Rectangle allowedBorder) {

            if(!allowedBorder.IsEmpty)
                if(!allowedBorder.Contains(new Point((int)x, (int)y)))
                    return;


            spBatch.Draw(t,
                new Rectangle((int)x, (int)y, 1, 1),
               null,
                color, //colour of line
                0, //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }
    }
}
