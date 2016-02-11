using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDirectX {
    internal class ContentLoader {
        ContentManager content;
        Dictionary<string, Texture2D> textures;

        public ContentLoader(ContentManager content) {
            this.content = content;
        }

        internal Texture2D GetTexture(string key) {
            if (textures.ContainsKey(key))
                return textures[key];
            throw new Exception("null texture!");
        }
        internal void SetTexture(string key) {
            var texture = content.Load<Texture2D>(key);
            if (textures == null)
                textures = new Dictionary<string, Texture2D>();
            textures.Add(key, texture);
        }
    }
}
