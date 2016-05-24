using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDirectX {
    public class DrawPrimitives: IDisposable {
        Color defaultFillColor = Color.Transparent;
        GraphicsDevice graphDevice;
        Texture2D t;

        public DrawPrimitives(GraphicsDevice gd) {
            graphDevice = gd;
            t = new Texture2D(gd, 1, 1);
            t.SetData<Color>(new Color[] { Color.White });
        }

        void DrawPixel(double x, double y, SpriteBatch spBatch, Color color, Rectangle border) {

            if(!border.IsEmpty)
                if(!border.Contains(new Point((int)x, (int)y)))
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

        public void Dispose() {
            t = null;
            graphDevice = null;
            Dispose();
        }
        public void DrawCircle(Vector2 center, float radius, SpriteBatch spBatch, Color color) {
            DrawCircle(center, radius, spBatch, color, Rectangle.Empty);
        }

        public void DrawCircle(Vector2 center, float radius, SpriteBatch spBatch, Color color, Rectangle border) {
            double theta = -Math.PI;  // angle that will be increased each loop
            double step = .01;  // amount to add to theta each time (degrees)

            while(theta < Math.PI) {
                double x = center.X + radius * Math.Cos(theta);
                double y = center.Y + radius * Math.Sin(theta);
                DrawPixel(x, y, spBatch, color, border);
                theta += step;
            }
        }
        public void DrawDottedLine(Vector2 start, Vector2 end, SpriteBatch spBatch, int width, Color color) {
            Vector2 v = end - start;
            int count = (int)(v.Length());
            v.Normalize();
            for(int i = 0; i < count; i += 10)
                DrawLine(start + v * i, start + v * (i + 5), spBatch, width, color);
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

        public void DrawPixel(Vector2 point, SpriteBatch spBatch, Color color) {
            DrawPixel(point, spBatch, color, Rectangle.Empty);
        }
        public void DrawPixel(Vector2 point, SpriteBatch spBatch, Color color, Rectangle border) {
            DrawPixel(point, spBatch, color, border);
        }
        public void DrawRect(Rectangle rect, SpriteBatch spBatch, int strokeWidth, Color borderColor) {
            DrawRect(rect, spBatch, strokeWidth, borderColor, defaultFillColor);
        }
        public void DrawRect(Rectangle rect, SpriteBatch spBatch, int width, Color borderColor, Color fillColor) {
            spBatch.Draw(t, rect, null, fillColor, 0f, new Vector2(), SpriteEffects.None, 0f);


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
