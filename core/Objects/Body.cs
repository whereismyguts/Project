using System;

namespace GameCore {
    public class Body: GameObject {
        float radius;
        string ImageName;

        public float Diameter {
            get { return radius * 2; }
        }
        protected float SelfRotation { get; set; }
        protected internal override Bounds Bounds {
            get {
                return new Bounds(Location - new CoordPoint(radius, radius), Location + new CoordPoint(radius, radius));
            }
        }
        protected internal override string ContentString {
            get {
                return ImageName;
            }
        }

        public Body(CoordPoint location, float diameter, string imageName, StarSystem system) : base(system) {
            radius = diameter / 2.0f;
            Location = location;
            Mass = diameter;
            ImageName = imageName;
        }

        protected internal override float GetRotation() {
            return SelfRotation;
        }
        protected internal override void Move() {
            SelfRotation += .001f;
        }
    }

    public class Planet: Body {
        bool clockwise;
        static Random rnd = new Random();
        float starRotation = 0;

        Body RotateCenter { get { return CurrentSystem.Star; } }
        float DistanceToSun {
            get {
                return CoordPoint.Distance(RotateCenter.Location, Location);
            }
        }

        public Planet(CoordPoint location, float diameter, float rotation, string imageName, bool clockwise, StarSystem system)
            : base(location, diameter, imageName, system) {
            this.starRotation = rotation;

            this.clockwise = clockwise;
        }

        protected internal override void Move() {
            if(starRotation >= 2 * Math.PI)
                starRotation = 0;
            Location = new CoordPoint((float)(DistanceToSun * Math.Cos(starRotation) + RotateCenter.Location.X), (float)(DistanceToSun * Math.Sin(starRotation) + RotateCenter.Location.Y));

            starRotation += clockwise ? .0001f : -.0001f;
            SelfRotation += .005f;
        }
    }
}
