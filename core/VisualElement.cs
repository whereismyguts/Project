using System;
using System.Collections.Generic;

namespace GameCore {
    public class VisualElement {
        public string ContentString { get; set; }
        public Bounds MiniMapBounds { get; set; }
        public string Name { get; set; }
        public float Rotation { get; set; }
        public Bounds ScreenBounds { get; set; }
        List<SpriteInfo> sprites = new List<SpriteInfo>();
        public List<SpriteInfo> SpriteInfoList {
            get {
                return sprites;
            }
        }

        internal VisualElement(GameObject obj) {
            foreach(var item in obj.GetSpriteInfos())
                sprites.Add(item);
            
            Name = obj.Name;
        }

        public override string ToString() {
            return ContentString + " " + Name;
        }
    }
    public class SpriteInfo {
        public string ContentString { get; set; }
        public Bounds MiniMapBounds { get; internal set; }
        public float Rotation { get; internal set; }
        public Bounds ScreenBounds { get; set; }
    }
}
