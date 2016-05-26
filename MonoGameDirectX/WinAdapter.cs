using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDirectX {
    public static class WinAdapter {
        static ContentLoader contentLoader;

        static RenderObject CreateRenderObject(VisualElement element) {
            var renderObject = new RenderObject( element);
            foreach(SpriteInfo spriteInfo in element.SpriteInfoList) {
                Texture2D texture = contentLoader.GetTexture(spriteInfo.ContentString);
                Rectangle boundsRect = Bounds2Rectangle(spriteInfo.ScreenBounds); // it might be a mono bug - location used like a center by mono draw() , so I do it:
                boundsRect.Location = boundsRect.Center;
                //--
                float xFactor =   spriteInfo.Origin.X/ boundsRect.Width;
                float yFactor =    spriteInfo.Origin.Y/ boundsRect.Height;
                Vector2 textureOrigin = new Vector2(texture.Width *xFactor, texture.Height*yFactor);
              //  Vector2 origin =  textureOrigin;//  new Vector2(texture.Width / 2f, texture.Height / 2f);// TODO actual sprite origin from Core
                renderObject.AddSprite(texture, boundsRect, textureOrigin, contentLoader.GetColorMask(spriteInfo.ContentString), 1, spriteInfo.Rotation);
            }
            return renderObject;
        }

        internal static Rectangle Bounds2Rectangle(Bounds bounds) {
            return new Rectangle((int)bounds.LeftTop.X, (int)bounds.LeftTop.Y, (int)bounds.Width, (int)bounds.Height);
        }
        internal static Vector2 CoordPoint2Vector(CoordPoint point) {
            return new Vector2(point.X, point.Y);
        }
        internal static Texture2D GetCursor() {
            return contentLoader.GetTexture(string.Empty); // TODO get "cursor"
        }
        internal static void LoadContent(ContentManager content, GraphicsDevice gd) {
            contentLoader = new ContentLoader(content, gd);
            contentLoader.SetTexture("256tile.png");
            contentLoader.SetTexture("world_png256.png");
            contentLoader.SetTexture("ship1");
            contentLoader.SetTexture("planet1");
            contentLoader.SetTexture("planet2");
            contentLoader.SetTexture("planet3");
            contentLoader.SetTexture("planet4");
            contentLoader.SetTexture("planet5");
            contentLoader.SetTexture("star", new Color(100, 100, 100, 100));
        }
        internal static void Unload() {

            contentLoader.Unload();
        }
        internal static Texture2D GetTexture(string key) {
            return contentLoader.GetTexture(key);
        }

        internal static void UpdateRenderObjects(ref List<RenderObject> renderObjects) {
            if(renderObjects == null)
                renderObjects = new List<RenderObject>();
            else renderObjects.Clear();

            foreach(VisualElement ve in MainCore.Instance.VisualElements)
                renderObjects.Add(CreateRenderObject(ve));
        }
    }
}
