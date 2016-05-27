using System;
using System.Collections.Generic;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDirectX {
    internal class RenderObject {

        protected float Scale { get { return MainCore.Instance.Viewport.Scale; } }

        public GameObject GameObject { get; internal set; }
        public Vector2 MiniMapLocation { get; internal set; }

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

        internal void Update() {
            foreach(Sprite sprite in sprites)
                sprite.Update();
            MiniMapLocation = WinAdapter.CoordPoint2Vector(GameObject.Position / 10000f);
        }
        List<Sprite> sprites = new List<Sprite>();
    }
    class Sprite {
        const float frameTime = 0.05f;

        Rectangle destRect;
        int frameHeight;
        int frameIndex;
        int frameWidth;
        Item item;
        Vector2 location;
        Vector2 origin;
        float rotation;
        Texture2D texture;

        float time;

        public Sprite(Item item) {
            this.item = item;
            Update();
        }

        void DrawAnimation(SpriteBatch spriteBatch, GameTime gameTime) {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while(time > frameTime) {
                frameIndex++;
                time = 0f;
            }
            if(frameIndex >= frames)
                frameIndex = 0;
            Rectangle source = new Rectangle(frameIndex * frameWidth, 0, frameWidth, frameHeight);
            spriteBatch.Draw(texture, destRect, source, new Color(100, 100, 150, 100), rotation, origin, SpriteEffects.None, 0f);
        }
        void DrawImage(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, destRect, null, new Color(100, 100, 150, 100), rotation, origin, SpriteEffects.None, 0f);
        }

        internal void Draw(SpriteBatch spBatch, GameTime t) {
            if(frames > 1)
                DrawAnimation(spBatch, t);
            else DrawImage(spBatch);
        }
        internal void Update() {
            location = WinAdapter.CoordPoint2Vector(item.PositionScreen);
            rotation = item.Rotation;
            texture = WinAdapter.GetTexture(item.SpriteInfo.Content);
            frames = item.SpriteInfo.Framecount;
            destRect = new Rectangle(location.ToPoint(), WinAdapter.CoordPoint2Vector(item.ScreenSize).ToPoint());

            float xFactor = item.ScreenOrigin.X / destRect.Size.X;
            float yFactor = item.ScreenOrigin.Y / destRect.Size.Y;

            origin = new Vector2(texture.Width / frames * xFactor, texture.Height * yFactor);

            frameHeight = texture.Height;
            frameWidth = texture.Width / frames;
        }
        int frames = 1;
    }
}