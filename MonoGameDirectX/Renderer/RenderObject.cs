using System;
using System.Collections.Generic;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDirectX {
    internal class RenderObject {
        List<Sprite> sprites = new List<Sprite>();

        protected float Scale { get { return MainCore.Instance.Viewport.Scale; } }

        public GameObject GameObject { get; internal set; }
        public Vector2 MiniMapLocation { get; }

        public RenderObject(GameObject obj) {
            GameObject = obj;
            IEnumerable<Item> items = obj.GetItems();
            foreach(Item item in items)
                sprites.Add(new Sprite(item));
            MiniMapLocation = WinAdapter.CoordPoint2Vector(obj.Position / 10000f);
        }

        internal void Draw(SpriteBatch spriteBatch, GameTime time) {
            foreach(var sprite in sprites)
                sprite.Draw(spriteBatch, time);
        }
    }
    class Sprite {
        Rectangle destRect;
        Item item;
        Vector2 origin;
        float rotation;
        Texture2D texture;

        Vector2 Location { get; }
        Vector2 Size { get; }

        public Sprite(Item item) {
            this.item = item;
            Location = WinAdapter.CoordPoint2Vector(item.PositionScreen);

            rotation = item.Rotation;
            texture = WinAdapter.GetTexture(item.ContentString);
            destRect = new Rectangle(Location.ToPoint(), WinAdapter.CoordPoint2Vector(item.ScreenSize).ToPoint());

            float xFactor = item.ScreenOrigin.X / destRect.Size.X;
            float yFactor = item.ScreenOrigin.Y / destRect.Size.Y;

            origin = new Vector2(texture.Width * xFactor, texture.Height * yFactor);
        }

        internal void Draw(SpriteBatch spriteBatch, GameTime time) {
            spriteBatch.Draw(texture, destRect, null, new Color(100, 100, 150, 100), rotation, origin, SpriteEffects.None, 0f);
        }
    }
}