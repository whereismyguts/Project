using System;

namespace Core {
    public abstract class GameObjectBase {
        public CoordPoint Location { get; set; }
        public string Image { get; set; }
        public abstract Bounds Bounds { get; }
    }

    public class Planet : GameObjectBase {
        float radius;
        float diameter;

        public override Bounds Bounds {
            get {
                return new Bounds(Location - new CoordPoint(radius,radius), Location + new CoordPoint(radius, radius));
            }
        }

        public Planet(CoordPoint location, float diameter) {
            this.Location = location;
            this.diameter = diameter;
            this.radius = diameter / 2.0f;
        }
    }
}