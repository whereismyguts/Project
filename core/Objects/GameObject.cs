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


        protected internal abstract IEnumerable<SpriteInfo> GetSpriteInfos();    //TODO foreach in all iternal items (weapons, effects, clouds, engines) 
        
        

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
