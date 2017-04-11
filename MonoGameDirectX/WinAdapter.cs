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

        static RenderObject CreateRenderObject(IRenderableObject obj) {
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
            return contentLoader != null ? contentLoader.GetTexture(key) : null;
        }
        internal static void LoadContent(ContentManager content, GraphicsDevice gd) {
            contentLoader = new ContentLoader(content, gd);
            contentLoader.SetTexture("256tile.png");
            contentLoader.SetTexture("flame_sprite.png");
            contentLoader.SetTexture("player_1_straight_idle.gif");
            contentLoader.SetTexture("planet.png");
            contentLoader.SetTexture("emptyslot.png");
            contentLoader.SetTexture("engine.png");
            contentLoader.SetTexture("exp.png");
            contentLoader.SetTexture("exp2.png");
            contentLoader.SetTexture("explosion-sprite.png");
            contentLoader.SetTexture("spaceship.png");
            contentLoader.SetTexture("bullet.png");
            contentLoader.SetTexture("slime.png");
            contentLoader.SetTexture("gun.png");
            contentLoader.SetTexture("eng_active.png");
            contentLoader.SetTexture("eng.png");
            contentLoader.SetTexture("hull.png");
            contentLoader.SetTexture("retrogun.png");
            contentLoader.SetTexture("retrogunfire.png");
            contentLoader.SetTexture("circle.png");
        }

        internal static Color Color(InternalColor color) {
            return new Color(color.r, color.g, color.b);
        }

        internal static void Unload() {
            contentLoader.Unload();
        }

        internal static void UpdateRenderObjects(ref List<RenderObject> renderObjects) {
            if(renderObjects == null)
                renderObjects = CreateRenderObjects();
            else {
                var newrenderObjects = CreateRenderObjects();
                foreach(var newObj in newrenderObjects) {

                    var oldObj = renderObjects.FirstOrDefault(o => o.GameObject == newObj.GameObject);
                    if(oldObj == null)
                        renderObjects.Add(newObj);
                }
                renderObjects.RemoveAll(o => newrenderObjects.FirstOrDefault(n => n.GameObject == o.GameObject) == null);
            }
            foreach(RenderObject obj in renderObjects)
                obj.Step();
        }

        private static List<RenderObject> CreateRenderObjects() {
            var list = new List<RenderObject>();

            foreach(GameObject obj in MainCore.Instance.Objects)
                list.Add(CreateRenderObject(obj));

            var playerInterface = PlayerController.GetInterfaceElements();

            foreach(IRenderableObject obj in playerInterface)
                list.Add(CreateRenderObject(obj));

            return list;
        }
    }
}
