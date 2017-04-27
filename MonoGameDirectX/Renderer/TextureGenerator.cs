using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Collections.Generic;

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
            
            Color[] data = new Color[diam*diam];
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
            Vector2 source = new Vector2(radius, -radius/2);

            for(int pixel = 0; pixel < data.Count(); pixel++) {
                var dist = Vector2.Distance(center, new Vector2(x, y));
                if(dist <= radius) {


                    var lightDist = Vector2.Distance(source, new Vector2(x, y));
                    if(lightDist>radius*2)
                        data[pixel] = GameCore.Rnd.Get(0,lightDist)> lightDist /3.5? color: Color.Transparent;

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