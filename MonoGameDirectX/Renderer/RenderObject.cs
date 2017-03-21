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

            primitives = obj.GetPrimitives();
        }

        internal void Draw(SpriteBatch spriteBatch, GameTime time) {
            
            foreach(var sprite in sprites)
                if(sprite.DestRect.Size != new Point() && sprite.DestRect.Intersects(spriteBatch.GraphicsDevice.Viewport.Bounds))
                    sprite.Draw(spriteBatch, time,false);
            foreach(Geometry geom in primitives)
                DrawPrimitives.DrawGeometry(geom, spriteBatch);
        }

        internal void Update() {
            foreach(Sprite sprite in sprites)
                sprite.Update();
            primitives = GameObject.GetPrimitives();
           // var items = GameObject.GetItems();
         //   MiniMapLocation = WinAdapter.CoordPoint2Vector(GameObject.Position / 10000f);
        }
        List<Sprite> sprites = new List<Sprite>();
        IEnumerable<Geometry> primitives = new List<Geometry>();
        
    }
    class Sprite {
        const float frameTime = 0.03f;

        internal Rectangle DestRect { get; private set; }
        int frameHeight;
        int frameIndexX;
        int frameIndexY;
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
            framesX = info.FramesX;
            framesY = info.FramesY;
            DestRect = rectangle;
            origin = new Vector2();
            if(texture == null)
                return;
            frameHeight = texture.Height / framesY;
            frameWidth = texture.Width / framesX;
        }

        void DrawAnimation(SpriteBatch spriteBatch, GameTime gameTime) {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while(time > frameTime) {
                if(frameIndexX < framesX)
                    frameIndexX++;
                else {
                    frameIndexX = 0;
                    frameIndexY++;
                }
                time = 0f;
            }
            if(frameIndexY >= framesY) {
                frameIndexY = 0;
                frameIndexX = 0;
            }
            Rectangle source = new Rectangle(frameIndexX * frameWidth, frameIndexY * frameHeight, frameWidth, frameHeight);
            spriteBatch.Draw(texture, DestRect, source, Color.White, rotation, origin, SpriteEffects.None, 0f);
        }
        void DrawImage(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, DestRect, null, Color.White, rotation, origin, SpriteEffects.None, 0f);
        }

        internal void Draw(SpriteBatch spBatch, GameTime t, bool fit) {
            if(texture == null)
                return;
            Rectangle temp = DestRect;
            if(fit)
                ResizeToFit();
            if(framesX > 1 || framesY > 1)
                DrawAnimation(spBatch, t);
            else DrawImage(spBatch);
            DestRect = temp;
        }

        void ResizeToFit() {
                if(frameWidth > texture.Height || frameHeight > texture.Width) {
                    float r = DestRect.X / (float)texture.Width;
                    DestRect = new Rectangle(DestRect.Location, new Point(DestRect.Width, (int)(DestRect.Height * r)));
                }
                if(texture.Height > frameWidth || texture.Width>frameHeight) {
                    float r = (float)texture.Height / DestRect.Y;
                    DestRect = new Rectangle(DestRect.Location, new Point((int)(DestRect.Width * r), DestRect.Height));
                }
        }

        internal void Update() {
            Vector2 location = WinAdapter.CoordPoint2Vector(item.PositionScreen);
            rotation = item.Rotation;
            texture = WinAdapter.GetTexture(item.SpriteInfo.Content);
            framesX = item.SpriteInfo.FramesX;
            framesY = item.SpriteInfo.FramesY;
            DestRect = new Rectangle(location.ToPoint(), WinAdapter.CoordPoint2Vector(item.ScreenSize).ToPoint());

            float xFactor = item.ScreenOrigin.X / DestRect.Size.X;
            float yFactor = item.ScreenOrigin.Y / DestRect.Size.Y;

            origin = new Vector2(texture.Width / framesX * xFactor, texture.Height / framesY * yFactor);

            frameHeight = texture.Height / framesY;
            frameWidth = texture.Width / framesX;
        }
        int framesX = 1;
        int framesY = 1;
    }
}