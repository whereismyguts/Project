using System;

namespace Core {
    public class AttractingObject : GameObject {
        float radius;
        protected float SelfRotation { get; set; }
        protected internal string ImageName { get; protected set; }

        protected internal override Bounds Bounds
        {
            get
            {
                return new Bounds(Location - new CoordPoint(radius, radius), Location + new CoordPoint(radius, radius));
            }
        }
        protected internal override string ContentString
        {
            get
            {
                return ImageName;
            }
        }
        public AttractingObject(CoordPoint location, float diameter, Viewport viewport, string imageName)
            : base(viewport) {

            this.radius = diameter / 2.0f;
            this.Location = location;
            this.Mass = diameter * 10;
            ImageName = imageName;
        }
        protected internal override void Move() {
            SelfRotation += 0.001f;
            return;
        }
        protected internal override float GetRotation() {
            return SelfRotation;
        }
    }
    public class Planet : AttractingObject {
        float distanceToSun;
        float t = 0;
        CoordPoint rotateCenter;
        public bool Clockwise { get { return true; } }

        public Planet(CoordPoint location, float diameter, Viewport viewport, float T, AttractingObject sun, string imageName)
            : base(location, diameter, viewport, imageName) {
            t = T;
            rotateCenter = sun.Location;
            distanceToSun = CoordPoint.Distance(rotateCenter, Location);
        }
        protected internal override void Move() {
            if(t >= 2 * Math.PI)
                t = 0;
            Location = new CoordPoint((float)(distanceToSun * Math.Cos(t) + rotateCenter.X), (float)(distanceToSun * Math.Sin(t) + rotateCenter.Y));

            t += 0.01f;
            SelfRotation += 0.05f;
        }
    }
}
