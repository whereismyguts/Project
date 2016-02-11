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
        public void DrawLine(Vector2 start, Vector2 end, SpriteBatch spBatch) {
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
                Color.Red, //colour of line
                angle, //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }
        public void DrawRect(Rectangle rect, SpriteBatch spBatch) {
            var lt = new Vector2(rect.Left, rect.Top);
            var lb = new Vector2(rect.Left, rect.Bottom);
            var rt = new Vector2(rect.Right, rect.Top);
            var rb = new Vector2(rect.Right, rect.Bottom);
            DrawLine(lt, rt, spBatch);
            DrawLine(rt, rb, spBatch);
            DrawLine(rb, lb, spBatch);
            DrawLine(lb, lt, spBatch);
        }
    }
}
