using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class Character : GameObject {
        const float gravitationConstant = .6f;
        const float inertiaFactor = .5f;

        List<AttractingObject> AttractingObjects;
        float enginePower = 0;

        public CoordPoint direction = new CoordPoint(1, 0);

        protected internal override Bounds Bounds {
            get {
                return new Bounds(Location - new CoordPoint(5, 5), Location + new CoordPoint(5, 5));
            }
        }
        protected internal override string ContentString {
            get {
                return "ship1";
            }
        }

        public Character(Viewport viewport, List<AttractingObject> objects, CoordPoint location)
            : base(viewport) {
            AttractingObjects = objects;
            Location = location;
            Mass = 10;
        }

        protected internal override float GetRotation() {
            return (float)(direction.Angle);
        }
        protected internal override void Move() {
            Location += direction * enginePower;
        }

        public void AccselerateF() {
            enginePower = 10;
        }
        public void RotateL() {
            direction.Rotate(-.1f);
        }
        public void RotateR() {
            direction.Rotate(.1f);
        }
        public void Stop() {
            enginePower = 0;
        }
    }
}
