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
                sprite.Draw(spriteBatch, time,false);
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

        Vector2 origin;
        float rotation;
        Texture2D texture;

        float time;

        public Sprite(Item item) {
            this.item = item;
            Update();
        }

        public Sprite(SpriteInfo info, Rectangle rectangle) {
            rotation = 0;
            texture = WinAdapter.GetTexture(info.Content);
            frames = info.Framecount;
            destRect = rectangle;
            origin = new Vector2();
            if(texture == null)
                return;
            frameHeight = texture.Height;
            frameWidth = texture.Width / frames;
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

        internal void Draw(SpriteBatch spBatch, GameTime t, bool fit) {
            if(texture == null)
                return;
            Rectangle temp = destRect;
            if(fit) {
                if(texture.Width/frames > texture.Height) {
                    float r = destRect.X / (float)texture.Width;
                    destRect = new Rectangle(destRect.Location, new Point(destRect.Width, (int)(destRect.Height * r)));
                }
                if(texture.Height > texture.Width/frames) {
                    float r =  (float)texture.Height/ destRect.Y;
                    destRect = new Rectangle(destRect.Location, new Point((int)(destRect.Width*r), destRect.Height));
                }
            }
            if(frames > 1)
                DrawAnimation(spBatch, t);
            else DrawImage(spBatch);
            destRect = temp;
        }
        internal void Update() {
            Vector2 location = WinAdapter.CoordPoint2Vector(item.PositionScreen);
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