using System;
using System.Collections.Generic;

namespace GameCore {
    public abstract class GameObject {
        protected string Image { get; set; }
        protected Viewport Viewport { get { return MainCore.Instance.Viewport; } }

        protected internal abstract Bounds Bounds { get; }
        protected internal virtual CoordPoint Location { get; set; }
        protected internal float Mass { get; set; }
        protected internal abstract float Rotation { get; }

        internal abstract bool IsMinimapVisible { get; }

        public StarSystem CurrentSystem { get; }

        protected internal IEnumerable<SpriteInfo> GetSpriteInfos() {
            //TODO foreach in all iternal items (weapons, effects, clouds, engines) :
            var screenBounds = GetScreenBounds();
            SpriteInfo info = new SpriteInfo() {
                ScreenBounds = screenBounds,
                MiniMapBounds = IsMinimapVisible ? screenBounds / 10f : null,
                ContentString = "256tile.png",
                Rotation = Rotation
            };
            return new SpriteInfo[] { info };
        }

        public virtual string Name { get { return string.Empty; } }

        public GameObject(StarSystem system) {
            CurrentSystem = system;
        }

        protected internal abstract void Step();

        public Bounds GetScreenBounds() {
            return Viewport.World2ScreenBounds(Bounds);
        }
    }
}
