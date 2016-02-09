using System;

namespace Core {
    public abstract class GameObjectBase {
        public float Mass { get; set; }
        internal CoordPoint Location { get; set; }
        public float Size { get; set; }
        public string Image { get; set; }
        public abstract Bounds Bounds { get; }
        protected Viewport Viewport { get; }
        public bool IsVisible { get { return Viewport.Bounds.isIntersect(Bounds); } }

        public GameObjectBase(Viewport viewport) {
            Viewport = viewport;
        }
        public CoordPoint GetLocation() {
            return new CoordPoint(Bounds.LeftTop.X - Viewport.Bounds.X, Bounds.Y - Viewport.Bounds.Y);
        }
        public abstract void Move();
    }
    public class AttractingObject : GameObjectBase {
        float radius;


        public override Bounds Bounds
        {
            get
            {
                return new Bounds(Location - new CoordPoint(radius, radius), Location + new CoordPoint(radius, radius));
            }
        }
        public AttractingObject(CoordPoint location, float diameter, Viewport viewport)
            : base(viewport) {

            Size = diameter;
            this.radius = diameter / 2.0f;
            this.Location = location - new CoordPoint(radius, radius);
            this.Mass = diameter;

        }
        public override void Move() {
            // do nothing yet
            return;
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
        public CoordPoint RotateCenter
        {
            get { return rotateCenter; }
            set
            {
                rotateCenter = value;
                distanceToSun = CoordPoint.Distance(rotateCenter, Location);
            }
        }
        public Planet(CoordPoint location, float diameter, Viewport viewport, float T, AttractingObject sun)
            : base(location, diameter, viewport) {
            this.t = T;
            this.RotateCenter = sun.Location;
        }
        public override void Move() {

            if(t >= 2 * Math.PI)
                t = 0;
            Location = new CoordPoint((float)(distanceToSun * Math.Cos(t) + RotateCenter.X), (float)(distanceToSun * Math.Sin(t) + RotateCenter.Y));

            t += 0.01f;
        }
    }
}