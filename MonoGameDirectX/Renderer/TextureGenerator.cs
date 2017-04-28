using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Collections.Generic;
using GameCore;

namespace MonoGameDirectX {
    public static class TextureGenerator {

        static Dictionary<string, Texture2D> circles = new Dictionary<string, Texture2D>();

        internal static Texture2D Circle(GraphicsDevice device, int radius, Color color) {

            if(radius < 1)
                radius = 1;

            string key = "" + radius + color.PackedValue;

            if(circles.ContainsKey(key))
                return circles[key];

            int diam = radius * 2;

            Texture2D texture = new Texture2D(device, diam, diam);

            Color[] data = new Color[diam * diam];
            int x = 0, y = 0;

            Vector2 center = new Vector2(radius, radius);

            for(int pixel = 0; pixel < data.Count(); pixel++) {
                var dist = Vector2.Distance(center, new Vector2(x, y));
                if(dist <= radius)
                    data[pixel] = color;
                else
                    data[pixel] = Color.Transparent;


                x++;
                if(x == diam) {
                    x = 0; y++;
                }
            }

            texture.SetData(data);

            circles.Add(key, texture);



            return texture;
        }

        class Particle {

            public Vector2 location; // relative
            public float size;
            public Vector2 dir;
            public Color color;
            public float speed;

            public int delay;

            public Particle(Vector2 location, Color color, int size) {
                this.location = location;
                //  this.radius = radius;
                this.color = color;
                this.dir = Vector2.One.GetRotated(Rnd.GetPeriod());
                speed = Rnd.Get(10, 25);
                this.size = Rnd.Get(size - 5, size + 5);
            }
            internal Particle Clone() {
                return new Particle(location, color, 0) { size = this.size, dir = this.dir, speed = this.speed };
            }
            internal void Step(Vector2 d) {
                if(delay > 0) {
                    delay--;
                    return;
                }
                size *= 0.5f;
                speed *= 0.6f;
                location += dir * speed + d;
            }
        }

        public static string time = "";

        internal static Texture2D Explosion(GraphicsDevice device, int size, Vector2 direction) {
            long start = DateTime.Now.Ticks;
            int frames = 15;
            Texture2D texture = new Texture2D(device, frames * size, size);
            int component = 255 / 4;
            List<Particle> particles = new List<Particle>();
            for(int i = 0; i < 100; i++) {

                for(int c = 0; c < 4; c++) {
                    int col = (c + 1) * component;
                    particles.Add(new Particle(Vector2.Zero, new Color(col - 10, col + 2, col - 10), (4 - c) * 5) { delay = c });
                }
                //particles.Add(new Particle(Vector2.Zero, Color.Gray) { delay = 2 });
                //particles.Add(new Particle(Vector2.Zero, Color.DarkSlateGray) { delay = 3 });
                ////particles.Add(new Particle(Vector2.Zero, 1, Color.Goldenrod) { delay = 3 });
                //particles.Add(new Particle(Vector2.Zero, Color.Yellow) { delay = 4 });
            }

            List<List<Particle>> total = new List<List<Particle>>();

            for(int i = 0; i < frames; i++) {
                Vector2 center = new Vector2(size / 2 + i * size, size / 2);

                total.Add(new List<Particle>());

                foreach(var p in particles) {
                    var t = p.Clone();
                    t.location += center;
                    total[i].Add(t);
                    p.Step(direction);
                }
            }

            int width = size * frames;

            Color[] data = new Color[size * width];
            //int x = 0, y = 0;

            for(int pixel = 0; pixel < data.Count(); pixel++)
                data[pixel] = Color.Transparent;

            foreach(var list in total) {
                list.RemoveAll(p => p.size == 0 || p.speed < 0.5);
                foreach(var p in list) {

                    if(p.size == 0 || p.speed < 0.1)
                        continue;

                    double c = 2 * Math.PI * p.size / 2;
                    double step = Math.PI / c;

                    for(int r = 0; r <= p.size / 2; r++)
                        for(double th = -Math.PI; th < Math.PI; th += step) {
                            float rf = r + Rnd.Get(-1f, 1f);
                            float x = (float)(p.location.X + rf * Math.Cos(th));
                            float y = (float)(p.location.Y + rf * Math.Sin(th));

                            int pixel = (width * (int)y + (int)x);
                            //  int pixelr = CountPixel((int)x, (int)y, data.Count(), width);
                            if(pixel > 0 && pixel < data.Count()) {
                                if(data[pixel] != Color.Transparent && data[pixel] != p.color)
                                    data[pixel] = new Color((p.color.ToVector3() + data[pixel].ToVector3()) / 2);
                                else
                                    data[pixel] = p.color;
                            }
                        }
                }
            }
            
            texture.SetData(data);
            time = TimeSpan.FromTicks(DateTime.Now.Ticks - start).TotalMilliseconds.ToString("f3");
            return texture;
        }

        //TODO Remove
        private static int CountPixel(int ax, int ay, int count, int width) {
            int x = 0, y = 0;
            for(int pixel = 0; pixel < count; pixel++) {

                if(ax == x && ay == y)
                    return pixel;
                x++;
                if(x == width) {
                    x = 0; y++;
                }
            }
            return 0;
        }

        internal static Texture2D CircleShadow(GraphicsDevice device, int radius, Color color) {
            if(radius < 1)
                radius = 1;

            string key = "s" + radius + color.PackedValue;

            if(circles.ContainsKey(key))
                return circles[key];

            int diam = radius * 2;




            Texture2D texture = new Texture2D(device, diam, diam);

            Color[] data = new Color[diam * diam];
            int x = 0, y = 0;

            Vector2 center = new Vector2(radius, radius);
            Vector2 source = new Vector2(radius, -radius / 2);

            for(int pixel = 0; pixel < data.Count(); pixel++) {
                var dist = Vector2.Distance(center, new Vector2(x, y));
                if(dist <= radius) {


                    var lightDist = Vector2.Distance(source, new Vector2(x, y));
                    if(lightDist > radius * 2)
                        data[pixel] = GameCore.Rnd.Get(0, lightDist) > lightDist / 3.5 ? color : Color.Transparent;

                }


                else
                    data[pixel] = Color.Transparent;

                x++;
                if(x == diam) {
                    x = 0; y++;
                }
            }


            texture.SetData(data);

            circles.Add(key, texture);
            return texture;
        }
        /*
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
*/
    }
}