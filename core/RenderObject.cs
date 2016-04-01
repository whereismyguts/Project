using System;

namespace GameCore {
    public class RenderObjectCore {
        public string ContentString { get; set; }
        public float Rotation { get; set; }
        public Bounds ScreenBounds { get; set; }
        public Bounds MiniMapBounds { get; set; }
        public string Name { get; set; }

        internal RenderObjectCore(Bounds bounds, Bounds miniMapBounds, string content, float rotation, string name) {
            ScreenBounds = bounds;
            MiniMapBounds = miniMapBounds;
            ContentString = content;
            Rotation = rotation;
            Name = name;
        }
        public override string ToString() {
            return ContentString+" "+Name;
        }
    }
}
