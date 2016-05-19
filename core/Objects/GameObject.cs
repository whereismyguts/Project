using System;

namespace GameCore {
    public abstract class GameObject {
        public StarSystem CurrentSystem { get; }
        protected string Image { get; set; }
        protected internal bool IsVisible {
            get {
                return true;
            }
        }
        protected Viewport Viewport { get { return MainCore.Viewport; } }
        internal CoordPoint Location { get; set; }
        protected internal abstract Bounds Bounds { get; }
        protected internal abstract string ContentString { get; }
        protected internal float Mass { get; set; }
        public virtual string Name { get { return ""; } }

        public GameObject(StarSystem system) {
            CurrentSystem = system;
        }

        protected internal abstract float GetRotation();
        protected internal abstract void Step();

        public Bounds GetScreenBounds() {
            return Viewport.World2ScreenBounds(Bounds);
        }
    }
}
