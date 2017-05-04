using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDirectX {
    internal class ContentLoader {
        ContentManager content;
        Texture2D dummyTexture;
        Dictionary<string, Color> masks;
        Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public ContentLoader(ContentManager content, GraphicsDevice gd) {
            this.content = content;
            dummyTexture = new Texture2D(gd, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
        }

        internal Color GetColorMask(string key) {
            if(masks.ContainsKey(key))
                return masks[key];
            return Color.White;
            throw new Exception("color unasigned!");
        }

        internal Texture2D GetTexture(string key) {
            if(string.IsNullOrEmpty(key))
                return dummyTexture;
            if(textures.ContainsKey(key))
                return textures[key];
            if(SetTexture(key))
                return textures[key];
            return dummyTexture;
            throw new Exception("null texture!");
        }
        internal bool SetTexture(string key) {
            return SetTexture(key, Color.White);
        }
        internal bool SetTexture(string key, Color mask) {
            try {
                textures[key] = content.Load<Texture2D>(key);
            }
            catch(Exception e) {
                textures[key] = dummyTexture;
                return false;
            }

            if(masks == null)
                masks = new Dictionary<string, Color>();
            masks[key] = mask;

            return true;
        }

        internal void Unload() {
            dummyTexture.Dispose();
            dummyTexture = null;
            content.Unload();
        }
    }
}
