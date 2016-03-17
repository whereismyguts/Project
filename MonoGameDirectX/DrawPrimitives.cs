using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDirectX {
    public class DrawPrimitives : IDisposable {
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
        public void DrawLine(Vector2 start, Vector2 end, SpriteBatch spBatch, Color color) {
            var edge = end - start;
            // calculate angle to rotate line
            // calculate angle to rotate line
            var angle =
            (float)Math.Atan2(edge.Y, edge.X);
            spBatch.Draw(t,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                color,
                angle, //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }
        public void DrawRect(Rectangle rect, SpriteBatch spBatch, Color color) {
            var lt = new Vector2(rect.Left, rect.Top);
            var lb = new Vector2(rect.Left, rect.Bottom);
            var rt = new Vector2(rect.Right, rect.Top);
            var rb = new Vector2(rect.Right, rect.Bottom);
            DrawLine(lt, rt, spBatch, color);
            DrawLine(rt, rb, spBatch, color);
            DrawLine(rb, lb, spBatch, color);
            DrawLine(lb, lt, spBatch, color);
        }

        public void DrawPixel(Vector2 point, SpriteBatch spBatch, Color color) {
            DrawPixel(point.X, point.Y, spBatch, color);
        }

        public void DrawCircle(Vector2 center, float radius, SpriteBatch spBatch, Color color) {
            double theta = -Math.PI;  // angle that will be increased each loop
            double step = .01;  // amount to add to theta each time (degrees)

            

            while( theta < Math.PI)
            {
                
                double x = center.X + radius * Math.Cos(theta);
                double y = center.Y + radius * Math.Sin(theta);
                DrawPixel(x,y, spBatch, color);
                theta += step;
                
            }
        }

        void DrawPixel(double x, double y, SpriteBatch spBatch,Color color) {
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
