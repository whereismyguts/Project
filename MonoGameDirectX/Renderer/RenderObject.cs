using System;
using System.Collections.Generic;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace MonoGameDirectX {
    internal class RenderObject {

        public IRenderableObject GameObject { get; internal set; }
        //public Vector2 MiniMapLocation { get; internal set; }

        public RenderObject(IRenderableObject obj) {
            GameObject = obj;
            Update();
            GameObject.Changed += GameObject_Changed;
        }

        void GameObject_Changed(RenderObjectChangedEventArgs args) {
            Update();
        }

        void Update() {
            List<Item> items = GameObject.GetItems().ToList();
            if(sprites.Count != items.Count) {
                sprites.Clear();
                foreach(Item item in items) {
                    sprites.Add(new Sprite(item));
                }
            }
            else {
                for(int i=0; i<  items.Count;i++) {
                    sprites[i].Update(items[i]);
                }
            }
        }

        internal void Draw(SpriteBatch spriteBatch, GameTime time) {
            Step();
            drawQueue = drawQueue.OrderBy(q => q.ZIndex).ToList();

            foreach(IDrawMyself selfDrawn in drawQueue)
                selfDrawn.Draw(spriteBatch, time, false);
        }

        internal void Step() {
            drawQueue.Clear();
            Update();
            if(Renderer.DebugMode > 0) {
                sprites.ForEach(s => s.Step());
                drawQueue.AddRange(sprites);
            }

            if(Renderer.DebugMode < 2) {
                var geometries = GameObject.GetPrimitives();
                foreach(Geometry g in geometries)
                    drawQueue.Add(new DrawableGeometry(g));
            }
        }
        List<IDrawMyself> sprites = new List<IDrawMyself>();
        List<IDrawMyself> drawQueue = new List<IDrawMyself>();
    }


    class DrawableGeometry: IDrawMyself {
        Geometry geometry;

        public int ZIndex {
            get {
                return geometry.ZIndex;
            }
        }

        public DrawableGeometry(Geometry geometry) {
            this.geometry = geometry;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime time, bool fit) {
            DrawPrimitives.DrawGeometry(geometry, spriteBatch);
        }

        public void Step() {

        }

        public void Update(Item item) {
         //   throw new NotImplementedException();
        }
    }

    interface IDrawMyself {
        int ZIndex { get; }
        void Draw(SpriteBatch spriteBatch, GameTime time, bool fit);
        void Step();
        void Update(Item item);
    }

    class Sprite: IDrawMyself {
        const float frameTime = 0.04f;
        public int ZIndex { get; set; } = 0;
        internal Rectangle DestRect { get; private set; }
        public Item Item { get { return item; } }

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
            Step();
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

            Rectangle source = new Rectangle(frameIndexX * frameWidth, frameIndexY * frameHeight, frameWidth, frameHeight);
            spriteBatch.Draw(texture, DestRect, source, Color.White, rotation, origin, SpriteEffects.None, 0f);
               spriteBatch.DrawString(Renderer.Font, frameIndexX + " : " + frameIndexY, (DestRect.Center).ToVector2(), Color.Red);

            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(time > frameTime) {
                if(frameIndexX < framesX - 1) {
                    frameIndexX++;
                }
                else {

                    frameIndexX = 0;

                    if(frameIndexY < framesY - 1) {
                        frameIndexY++;
                    }
                    else
                        frameIndexY = 0;
                }

                time = 0f;
            }
        }
        void DrawImage(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, DestRect, null, Color.White, rotation, origin, SpriteEffects.None, 0f);
            //spriteBatch.DrawString(Renderer.Font, texture.Name, (DestRect.Center).ToVector2(), Color.Red);
        }

        public void Draw(SpriteBatch spBatch, GameTime t, bool fit) {

            if(DestRect.Size != new Point() /*&& DestRect.Intersects(spBatch.GraphicsDevice.Viewport.Bounds)*/) {

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
        }

        void ResizeToFit() {
            if(frameWidth > texture.Height || frameHeight > texture.Width) {
                float r = DestRect.X / (float)texture.Width;
                DestRect = new Rectangle(DestRect.Location, new Point(DestRect.Width, (int)(DestRect.Height * r)));
            }
            if(texture.Height > frameWidth || texture.Width > frameHeight) {
                float r = (float)texture.Height / DestRect.Y;
                DestRect = new Rectangle(DestRect.Location, new Point((int)(DestRect.Width * r), DestRect.Height));
            }
        }

        public void Step() {
            Vector2 location = item.ScreenLocation;
            rotation = item.Rotation;
            ZIndex = item.SpriteInfo.ZIndex;
            texture = WinAdapter.GetTexture(item.SpriteInfo.Content);
            framesX = item.SpriteInfo.FramesX;
            framesY = item.SpriteInfo.FramesY;
            DestRect = new Rectangle(location.ToPoint(), item.ScreenSize.ToPoint());

            float xFactor = item.ScreenOrigin.X / DestRect.Size.X;
            float yFactor = item.ScreenOrigin.Y / DestRect.Size.Y;

            origin = new Vector2(texture.Width / framesX * xFactor, texture.Height / framesY * yFactor);

            frameHeight = texture.Height / framesY;
            frameWidth = texture.Width / framesX;
        }

        public void Update(Item item) {
            this.item = item;
        }

        int framesX = 1;
        int framesY = 1;
    }
}