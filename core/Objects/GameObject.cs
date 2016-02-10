using System;

namespace Core {
    public abstract class GameObjectBase {
        public float Mass { get; set; }
        internal CoordPoint Location { get; set; }
        public string Image { get; set; }
        public abstract Bounds Bounds { get; }
        public abstract string ContentString { get; }
        protected Viewport Viewport { get; }
        public bool IsVisible { get { return Viewport.Bounds.isIntersect(Bounds); } }

        public GameObjectBase(Viewport viewport) {
            Viewport = viewport;
        }
        public Bounds GetScreenBounds() {
            //return Bounds;
            CoordPoint lt = new CoordPoint(Bounds.LeftTop.X - Viewport.Bounds.X, Bounds.Y - Viewport.Bounds.Y)*Viewport.Scale;
            CoordPoint delta = new CoordPoint(Bounds.Width, Bounds.Height) * (Viewport.Scale / 2);
            return new Bounds(lt-delta, lt +delta);
        }
        public abstract void Move();
        public abstract float GetRotation();
    }
}