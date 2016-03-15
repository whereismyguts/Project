using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDirectX {
    internal class ContentLoader {
        ContentManager content;
        Dictionary<string, Color> masks;
        Dictionary<string, Texture2D> textures;

        public ContentLoader(ContentManager content) {
            this.content = content;
        }

        internal Texture2D GetTexture(string key) {
            if(textures.ContainsKey(key))
                return textures[key];
            throw new Exception("null texture!");
        }
        internal void SetTexture(string key) {
            SetTexture(key, Color.White);
        }
        internal void SetTexture(string key, Color mask) {
            var texture = content.Load<Texture2D>(key);
            if(textures == null)
                textures = new Dictionary<string, Texture2D>();
            textures[key] = texture;

            if(masks == null)
                masks = new Dictionary<string, Color>();
            masks[key] = mask;
        }

        internal Color GetColorMask(string key) {
            if(masks.ContainsKey(key))
                return masks[key];
            throw new Exception("color unasigned!");
        }
    }
}
