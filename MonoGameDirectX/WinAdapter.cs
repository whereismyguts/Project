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

        static RenderObject CreateRenderObject(GameObject obj) {
            RenderObject renderObject = new RenderObject(obj);
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
        internal static Texture2D GetTexture(string key) {
            return contentLoader!=null? contentLoader.GetTexture(key):null;
        }
        internal static void LoadContent(ContentManager content, GraphicsDevice gd) {
            contentLoader = new ContentLoader(content, gd);
            contentLoader.SetTexture("256tile.png");
            contentLoader.SetTexture("flame_sprite.png");
            contentLoader.SetTexture("player_1_straight_idle.gif");
            contentLoader.SetTexture("planet.png");
            contentLoader.SetTexture("emptyslot.png");
            contentLoader.SetTexture("engine.png");
        }
        internal static void Unload() {

            contentLoader.Unload();
        }

        internal static void UpdateRenderObjects(ref List<RenderObject> renderObjects) {
            if(renderObjects == null)
                renderObjects = new List<RenderObject>();

            if(renderObjects.Count == 0)
                foreach(GameObject obj in MainCore.Instance.Objects) 
                    renderObjects.Add(CreateRenderObject(obj));

            else foreach(RenderObject obj in renderObjects)

                    obj.Update();
        }
        
    }
}
