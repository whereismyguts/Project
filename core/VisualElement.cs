using System;

namespace GameCore {
    public class VisualElement {
        public string ContentString { get; set; }
        public float Rotation { get; set; }
        public Bounds ScreenBounds { get; set; }
        public Bounds MiniMapBounds { get; set; }
        public string Name { get; set; }

        internal VisualElement(GameObject obj) {
            //obj.GetScreenBounds(), obj.ContentString, obj.GetRotation(), obj.Name
            ScreenBounds = obj.GetScreenBounds();
            MiniMapBounds = ScreenBounds / 10f;
            ContentString = obj.ContentString;
            Rotation = obj.Rotation;
            Name = obj.Name;
        }
        public override string ToString() {
            return ContentString+" "+Name;
        }
    }
}
