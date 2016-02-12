using System;

namespace Core {
    public abstract class GameObject {
        protected string Image { get; set; }
        protected bool IsVisible {
            get {
                return Viewport.Bounds.isIntersect(Bounds);
            }
        }
        protected Viewport Viewport { get; set; }

        internal CoordPoint Location { get; set; }

        protected internal abstract Bounds Bounds { get; }
        protected internal abstract string ContentString { get; }
        protected internal float Mass { get; set; }

        protected GameObject(Viewport viewport) {
            Viewport = viewport;
        }

        protected internal abstract float GetRotation();
        protected internal Bounds GetScreenBounds() {
            var centerPoint = (Bounds.CenterPoint - Viewport.Bounds.CenterPoint) * Viewport.Scale;
            var scaleVector = new CoordPoint(Bounds.Width, Bounds.Height) * Viewport.Scale;
            return new Bounds(centerPoint - scaleVector, centerPoint + scaleVector) + new CoordPoint(Viewport.Width, Viewport.Height) / 2 ;
        }
        protected internal abstract void Move();
    }
}
