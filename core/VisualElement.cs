using System;

namespace GameCore {
    public class VisualElement {
        public string ContentString { get; set; }
        public float Rotation { get; set; }
        public Bounds ScreenBounds { get; set; }
        public Bounds MiniMapBounds { get; set; }
        public string Name { get; set; }

        internal VisualElement(GameObject obj) {
            ScreenBounds = obj.GetScreenBounds();
            MiniMapBounds = obj.IsMinimapVisible ? ScreenBounds / 10f : null;
            ContentString = obj.ContentString;
            Rotation = obj.Rotation;
            Name = obj.Name;
        }

        public override string ToString() {
            return ContentString+" "+Name;
        }
    }
}
