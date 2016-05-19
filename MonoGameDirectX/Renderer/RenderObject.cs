using System;
using GameCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDirectX {
    internal class RenderObject {
        
        public Color ColorMask { get; internal set; }
        public Vector2 Origin { get; internal set; }
        public float Rotation { get; internal set; }
        public Texture2D Texture { get; internal set; }
        public Rectangle TextureRect { get; internal set; }
        public Bounds MiniMapBounds { get; internal set; }
        public string Name { get; internal set; }
        VisualElement element;

        public RenderObject(Texture2D texture, Rectangle textureRect, Vector2 origin, Color color, VisualElement element) {
            Texture = texture;
            TextureRect = textureRect;
            Origin = origin;
            Rotation = element.Rotation;
            ColorMask = color;
            MiniMapBounds = element.MiniMapBounds;
            Name = element.Name;
            this.element = element;
        }
        public override string ToString() {
            return element.ToString();
        }
    }
}