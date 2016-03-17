using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Content;

namespace MonoGameDirectX {
    public static class WinAdapter {
        static ContentLoader contentLoader;

        internal static Rectangle ToRectangle(Bounds bounds) {
            return new Rectangle((int)bounds.LeftTop.X, (int)bounds.LeftTop.Y, (int)bounds.Width, (int)bounds.Height);
        }
        internal static Vector2 ToVector2(Bounds bounds) {
            return new Vector2(bounds.LeftTop.X, bounds.LeftTop.Y);
        }
        internal static Vector2 ToVector2(CoordPoint point) {
            return new Vector2(point.X, point.Y);
        }
        internal static RenderObject CreateRenderObject(RenderObjectCore obj) {
            Texture2D texture = contentLoader.GetTexture(obj.ContentString);
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Rectangle boundsRect = WinAdapter.ToRectangle(obj.ScreenBounds);
            Rectangle textureRect = new Rectangle(boundsRect.Location + new Point(boundsRect.Width / 2, boundsRect.Height / 2), boundsRect.Size);

            RenderObject rObj = new RenderObject(texture, textureRect, origin, obj.Rotation, contentLoader.GetColorMask(obj.ContentString), obj.MiniMapBounds);
            return rObj;
        }
        internal static void LoadContent(ContentManager content) {
            contentLoader = new ContentLoader(content);

            contentLoader.SetTexture("ship1");
            contentLoader.SetTexture("planet1");
            contentLoader.SetTexture("planet2");
            contentLoader.SetTexture("planet3");
            contentLoader.SetTexture("planet4");
            contentLoader.SetTexture("planet5");
            contentLoader.SetTexture("star", new Color(100, 100, 100, 100));

            
        }
    }
}
