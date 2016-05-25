using System;
using System.Collections.Generic;

namespace GameCore {
    public class VisualElement {
        //public string ContentString { get; set; }
        public Bounds MiniMapBounds { get; set; }
        public string Name { get; set; }
        //public float Rotation { get; set; }
       // public Bounds ScreenBounds { get; set; }
        List<SpriteInfo> sprites = new List<SpriteInfo>();
        public List<SpriteInfo> SpriteInfoList {
            get {
                return sprites;
            }
        }

        internal VisualElement(GameObject obj) {
            foreach(var info in obj.GetSpriteInfos())
                sprites.Add(info);
            //ScreenBounds = sprites[0].ScreenBounds;
            MiniMapBounds = obj.GetScreenBounds() / 10f;
            //Rotation = sprites[0].Rotation;
            //ContentString = sprites[0].ContentString;
            Name = obj.Name;
        }
    }
    public class SpriteInfo {


        public SpriteInfo(Bounds screenBounds, string content, float rotation, CoordPoint origin) {
            ScreenBounds = screenBounds;
            ContentString = content;
            Rotation = rotation;
            Origin = origin;
        }

        public string ContentString { get; set; }
        public CoordPoint Origin { get; set; }

        //public Bounds MiniMapBounds { get; internal set; }
        public float Rotation { get; internal set; }
        public Bounds ScreenBounds { get; set; }
    }
}
