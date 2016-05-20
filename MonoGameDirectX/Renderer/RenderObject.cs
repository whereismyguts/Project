using System;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGameDirectX {
    internal class RenderObject {
        public Bounds MiniMapBounds { get; internal set; }
        public Bounds Bounds { get; internal set; }
        public string Name { get; internal set; }
        List<Sprite> sprites = new List<Sprite>();
        VisualElement element;

        public RenderObject(VisualElement element) {
            Bounds = element.ScreenBounds;
            MiniMapBounds = element.MiniMapBounds;
            Name = element.Name;
            this.element = element;
        }
        public void AddSprite(Texture2D texture, Rectangle textureRect, Vector2 origin, Color color, int frameCount) {
            if(frameCount == 1)
                sprites.Add(new Sprite(texture, textureRect, origin, color));
            else
                sprites.Add(new SpriteAnimation(texture, textureRect, origin, color, frameCount));
        }
        public override string ToString() {
            return element.ToString();
        }
        internal void Draw(SpriteBatch spriteBatch, GameTime time) {
            foreach(var sp in sprites)
                sp.Draw(spriteBatch, time);
        }
    }
    class Sprite {
        private Rectangle textureRect;
        private Color color;

        public Sprite(Texture2D texture, Rectangle textureRect, Vector2 origin, Color color) {
            Texture = texture;
            this.textureRect = textureRect;
            Origin = origin;
            this.color = color;
        }

        protected Texture2D Texture { get; set; }
        public Rectangle Rect { get; set; }
        public Color ColorMask { get; set; }
        public Vector2 Origin { get; set; }
        public float Rotation { get; set; }


        protected internal virtual void Draw(SpriteBatch batch, GameTime gameTime) {
            batch.Draw(Texture, Rect, null, ColorMask, Rotation, Origin, SpriteEffects.None, 0);
        }

    }
    class SpriteAnimation: Sprite {

        Rectangle source;

        #region animation
        float time;
        // duration of time to show each frame
        float frameTime = 0.1f;
        // an index of the current frame being shown
        int frameIndex;
        // total number of frames in our spritesheet
        int totalFrames = 10;
        // define the size of our animation frame
        int frameHeight = 64;
        int frameWidth = 64;
        #endregion

        public SpriteAnimation(Texture2D texture, Rectangle rect, Vector2 origin, Color color, int frames) : base(texture, rect, origin, color) {
            totalFrames = frames;
            frameHeight = texture.Height;
            frameWidth = texture.Width / frames;
        }
        protected internal override void Draw(SpriteBatch batch, GameTime gameTime) {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(time > frameTime) {
                // Play the next frame in the SpriteSheet
                frameIndex++;

                // reset elapsed time
                time = 0f;
            }
            if(frameIndex > totalFrames) frameIndex = 1;

            // Calculate the source rectangle of the current frame.
            Rectangle sourceRect = new Rectangle(frameIndex * frameWidth, 0, frameWidth, frameHeight);

            // Calculate position and origin to draw in the center of the screen
            //Vector2 position = new Vector2(100, 100);
            //Vector2 origin = new Vector2(frameWidth / 2.0f, frameHeight);
            batch.Draw(Texture, Rect, sourceRect, ColorMask, Rotation, Origin, SpriteEffects.None, 0f);
            batch.Draw(Texture, Rect.Location.ToVector2(), sourceRect, ColorMask, 0.0f, Origin, 1.0f, SpriteEffects.None, Rotation);
        }

    }

}