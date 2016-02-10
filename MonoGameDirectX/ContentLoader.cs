using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MonoGameDirectX {
    class ContentLoader {
        Dictionary<string, Texture2D> textures;
        ContentManager content;
        internal Texture2D GetTexture(string key) {
            if(textures.ContainsKey(key))
                return textures[key];
            throw new Exception("null texture!");
        }
        public ContentLoader(ContentManager content) {
            this.content = content;
        }
        internal void SetTexture(string key) {
            var texture = content.Load<Texture2D>(key);
            if(textures == null) textures = new Dictionary<string, Texture2D>();
            textures.Add(key, texture);
        }
    }
}
