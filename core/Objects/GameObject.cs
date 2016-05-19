using System;

namespace GameCore {
    public abstract class GameObject {
        public StarSystem CurrentSystem { get; }
        protected string Image { get; set; }
        protected Viewport Viewport { get { return MainCore.Instance.Viewport; } }
        protected internal virtual CoordPoint Location { get; set; }
        protected internal abstract Bounds Bounds { get; }
        protected internal abstract float Rotation { get; }
        protected internal abstract string ContentString { get; }
        protected internal float Mass { get; set; }
        public virtual string Name { get { return ""; } }
        internal abstract bool IsMinimapVisible { get; }

        public GameObject(StarSystem system) {
            CurrentSystem = system;
        }

        protected internal abstract void Step();
        public Bounds GetScreenBounds() {
            return Viewport.World2ScreenBounds(Bounds);
        }
    }
}
