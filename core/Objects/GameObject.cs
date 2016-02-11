using System;

namespace Core {
    public abstract class GameObject {
        protected GameObject(Viewport viewport) {
            Viewport = viewport;
        }

        protected string Image { get; set; }
        protected bool IsVisible {
            get {
                return Viewport.Bounds.isIntersect(Bounds);
            }
        }
        protected Viewport Viewport { get; set;}

        internal CoordPoint Location { get; set; }

        protected internal abstract Bounds Bounds { get; }
        protected internal abstract string ContentString { get; }
        protected internal float Mass { get; set; }

        protected internal abstract float GetRotation();
        protected internal Bounds GetScreenBounds() {

            
            //return Bounds;
            var lt = new CoordPoint(Bounds.LeftTop.X - Viewport.Bounds.X, Bounds.Y - Viewport.Bounds.Y) * Viewport.Scale;
            var delta = new CoordPoint(Bounds.Width, Bounds.Height) * (Viewport.Scale );
            return new Bounds(lt - delta, lt + delta);// + new CoordPoint(Viewport.Width, Viewport.Height) ;
        }
        protected internal abstract void Move();
    }
}
