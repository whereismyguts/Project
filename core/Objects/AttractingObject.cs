using System;

namespace Core {
    public class AttractingObject : GameObject {
        float radius;
        protected float SelfRotation { get; set; }
        protected internal string ImageName { get; protected set; }
        protected internal override Bounds Bounds {
            get {
                return new Bounds(Location - new CoordPoint(radius, radius), Location + new CoordPoint(radius, radius));
            }
        }
        protected internal override string ContentString { get { return ImageName; } }
        public AttractingObject(CoordPoint location, float diameter, Viewport viewport, string imageName)
            : base(viewport) {

            this.radius = diameter / 2.0f;
            this.Location = location;
            this.Mass = diameter * 10;
            ImageName = imageName;
        }
        protected internal override void Move() {
            SelfRotation += .001f;
        }
        protected internal override float GetRotation() {
            return SelfRotation;
        }
    }
    public class Planet : AttractingObject {
        float t = 0;
        AttractingObject rotateCenter;
        static Random rnd = new Random();
        float DistanceToSun {
            get { return CoordPoint.Distance(rotateCenter.Location, Location); }
        }
        bool clockwise;
        public Planet(CoordPoint location, float diameter, Viewport viewport, float T, AttractingObject rotateCenter, string imageName, bool clockwise)
            : base(location, diameter, viewport, imageName) {
            t = T;
            this.rotateCenter = rotateCenter;
            this.clockwise = clockwise;
        }
        protected internal override void Move() {
            if(t >= 2 * Math.PI)
                t = 0;
            Location = new CoordPoint((float)(DistanceToSun * Math.Cos(t) + rotateCenter.Location.X), (float)(DistanceToSun * Math.Sin(t) + rotateCenter.Location.Y));

            t += clockwise ? .01f : -.01f;
            SelfRotation += .05f;
        }
    }
}
