using System;

namespace Core {
    public abstract class GameObject {
        protected internal float Mass { get; set; }
        internal CoordPoint Location { get; set; }
        protected string Image { get; set; }
        protected internal abstract Bounds Bounds { get; }
        protected internal abstract string ContentString { get; }
        protected Viewport Viewport { get; }
        protected bool IsVisible { get { return Viewport.Bounds.isIntersect(Bounds); } }
        protected GameObject(Viewport viewport) {
            Viewport = viewport;
        }
        protected internal Bounds GetScreenBounds() {
            //return Bounds;
            CoordPoint lt = new CoordPoint(Bounds.LeftTop.X - Viewport.Bounds.X, Bounds.Y - Viewport.Bounds.Y) * Viewport.Scale;
            CoordPoint delta = new CoordPoint(Bounds.Width, Bounds.Height) * (Viewport.Scale / 2);
            return new Bounds(lt - delta, lt + delta);
        }
        protected internal abstract void Move();
        protected internal abstract float GetRotation();
    }
}