using System;

namespace Core {
    public class AttractingObject : GameObjectBase {
        float radius;
        float selfRotation;

        public override Bounds Bounds
        {
            get
            {
                return new Bounds(Location - new CoordPoint(radius, radius), Location + new CoordPoint(radius, radius));
            }
        }
        public override string ContentString
        {
            get
            {
                return "planet1";
            }
        }
        public AttractingObject(CoordPoint location, float diameter, Viewport viewport)
            : base(viewport) {

            this.radius = diameter / 2.0f;
            this.Location = location;
            this.Mass = diameter*10;

        }
        public override void Move() {
            // do nothing yet
            selfRotation += 0.01f;
            
            return;
        }

        public override float GetRotation() {
            return selfRotation;
        }
    }
    public class Planet : AttractingObject {
        float distanceToSun;
        float t = 0;
        CoordPoint rotateCenter;
        public bool Clockwise
        {
            get { return true; }
        }

        public Planet(CoordPoint location, float diameter, Viewport viewport, float T, AttractingObject sun)
            : base(location, diameter, viewport) {
            t = T;
            rotateCenter = sun.Location;
            distanceToSun = CoordPoint.Distance(rotateCenter, Location);
        }
        public override void Move() {
            if(t >= 2 * Math.PI)
                t = 0;
            Location = new CoordPoint((float)(distanceToSun * Math.Cos(t) + rotateCenter.X), (float)(distanceToSun * Math.Sin(t) + rotateCenter.Y));

            t += 0.01f;
            base.Move();
        }
    }
}
