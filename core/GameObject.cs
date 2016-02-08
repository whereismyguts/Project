using System;

namespace Core {
    public abstract class GameObjectBase {
        public CoordPoint Location { get; set; }
        public string Image { get; set; }
        public abstract Bounds Bounds { get; }

        protected Viewport Viewport { get; }

        public GameObjectBase(Viewport viewport) {
            Viewport = viewport;
        }

        public CoordPoint GetLocation() {
            return new CoordPoint(Location.X - Viewport.LeftTop.X, Location.Y - Viewport.LeftTop.Y);
        }
    }

    public class Planet : GameObjectBase {
        float radius;
        float diameter;

        public override Bounds Bounds {
            get {
                return new Bounds(Location - new CoordPoint(radius,radius), Location + new CoordPoint(radius, radius));
            }
        }

        public Planet(CoordPoint location, float diameter, Viewport viewport)
            : base(viewport) {
            this.Location = location;
            this.diameter = diameter;
            this.radius = diameter / 2.0f;
        }

    }
}