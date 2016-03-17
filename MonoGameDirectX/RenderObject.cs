using Core;
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

        public RenderObject(Texture2D texture, Rectangle textureRect, Vector2 origin, float rotation, Color color, Bounds miniMapBounds) {
            Texture = texture;
            TextureRect = textureRect;
            Origin = origin;
            Rotation = rotation;
            ColorMask = color;
            MiniMapBounds = miniMapBounds;
        }
    }
}